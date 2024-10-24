using GoodbyeDPI_Configurator.Enums;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace GoodbyeDPI_Configurator.Classes
{
    internal class ServiceHandler: INotifyPropertyChanged
    {
        // Event handler for service status change.
        public event PropertyChangedEventHandler PropertyChanged;

        // Default configuration.
        internal string ServiceName = "GoodbyeDPI";
        internal string ServiceArguments = "-p";
        internal string ServiceDescription = "Passive Deep Packet Inspection blocker and Active DPI circumvention utility, installed via Configurator";
        internal string ServiceAutoStart = "auto";
        private EServiceStatus _ServiceStatus = EServiceStatus.Unkown;


        // Check if the system is 64-bit.
        internal static bool IsX64 = System.Environment.Is64BitOperatingSystem;

        // Folder path to extract service.
        internal static string FolderPath = Path.Combine(Path.GetTempPath(), "GoodbyeDPI");

        internal EServiceStatus ServiceStatus
        {
            get => _ServiceStatus;
            set
            {
                _ServiceStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ServiceStatus"));
            }
        }


        public ServiceHandler()
        {
            ServiceStatus = CheckServiceExists() ? EServiceStatus.Running_Service : EServiceStatus.Unkown;
        }

        /// <summary>
        /// Removes old files if found, then unpacks GoodbyeDPI and WinDivert. Install it as service.
        /// </summary>
        public void InstallService()
        {
            // Remove the service.
            RemoveService();

            // Unpack all the files.
            PUnpackAll();

            // Install the service.
            PInstallService();
        }

        public void RemoveService()
        {
            // https://github.com/ValdikSS/GoodbyeDPI/issues/378
            // "This approach is used to ensure that several programs using WinDivert work correctly at the same time,
            // so that closing one program does not stop the other. (WinDivert.sys / WinDivert64.sys)"

            // Problem: Can't delete the files because they are in use by WinDivert1.4 service.
            // https://youtu.be/umvgwXINJBE?t=118

            // Try to stop the services.
            PStopService("GoodbyeDPI");
            PStopService("WinDivert1.4");

            // Try to remove the service.
            PRemoveService();

            // Clear the folder.
            PClearFolder(true);
        }

        /// <summary>
        /// Creates a service with the name GoodbyeDPI and starts it.
        /// </summary>
        private void PInstallService()
        {
            // Get a new process.
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;

            // Load the arguments and start the service.
            process.StartInfo.Arguments += $"/C sc create \"GoodbyeDPI\" ";
            process.StartInfo.Arguments += $"binPath=\"{Path.Combine(FolderPath, "goodbyedpi.exe")} {ServiceArguments}\" ";
            process.StartInfo.Arguments += $"start=\"{ServiceAutoStart}\" ";
            process.StartInfo.Arguments += $"&& sc start \"GoodbyeDPI\" ";

            // DEBUG:
            //process.StartInfo.Arguments += $"&& sc description \"GoodbyeDPI\" \"{ServiceDescription}\"";
            process.StartInfo.Arguments += $"&& sc description \"GoodbyeDPI\" \"{ServiceArguments}\"";

            // Run the program.
            process.Start();
            process.WaitForExit();
            process.Dispose();

            // Update Service Status.
            ServiceStatus = EServiceStatus.Running_Service;
        }

        /// <summary>
        /// Sends STOP signal to the GoodbyeDPI service.
        /// </summary>
        private void PStopService(string service_name)
        {
            ServiceController Service = new ServiceController(service_name);

            // Ignore exceptions.
            try {Service.Stop(); }
            catch (Exception) { }

            Service.Close();
            Service.Dispose();
        }

        /// <summary>
        /// Removes the GoodbyeDPI service.
        /// </summary>
        private void PRemoveService()
        {
            // Set up a process to remove the service.
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C delete GoodbyeDPI && delete WinDivert1.4";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Run the program.
            process.Start();
            process.WaitForExit();
            process.Dispose();

            // Update the service status.
            ServiceStatus = EServiceStatus.Not_Installed;
        }

        // This function is complete.
        private void PClearFolder(bool eraseFolder = true)
        {
            // Delete old folder with its contents, if it exists.
            if (Directory.Exists(FolderPath))
            {
                Directory.Delete(FolderPath, true);
            }

            // Create a new folder.
            if (!eraseFolder)
            {
                Directory.CreateDirectory(FolderPath);
            }
        }

        /// <summary>
        /// Copy embedded resource into the folder.
        /// </summary>
        private void PUnpackEmbeddedFile(string fileName)
        {
            // Set resource path based on architecture.
            string architecture = !IsX64 ? "x86" : "x86_64";

            // Get stream to access resource file.
            using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"GoodbyeDPI_Configurator.GoodbyeDPIFiles.{architecture}.{fileName}"))
            {
                // Create file on the disk.
                using (FileStream fileStream = new FileStream(Path.Combine(FolderPath, fileName), FileMode.Create, FileAccess.ReadWrite))
                {
                    // Copy contents into the file.
                    resourceStream.CopyTo(fileStream);
                }
            }
        }

        /// <summary>
        /// Unpacks all the required files to run the service.
        /// </summary>
        private void PUnpackAll()
        {
            // Create a new folder.
            PClearFolder(false);

            // Unpack all the files we need to run the service.
            // Resource location will be derived from the OS architecture.
            PUnpackEmbeddedFile("goodbyedpi.exe");
            PUnpackEmbeddedFile("WinDivert.dll");
            PUnpackEmbeddedFile("WinDivert64.sys");

            // This file depends on system architecture.
            if (!IsX64)
            {
                PUnpackEmbeddedFile("WinDivert32.sys");
            }
        }

        /// <summary>
        /// Check whether or not service exists.
        /// </summary>
        internal bool CheckServiceExists()
        {
            ServiceController[] ServiceList = ServiceController.GetServices();

            return ServiceList.Any(Service => Service.ServiceName.Equals("GoodbyeDPI"));
        }
    }
}
