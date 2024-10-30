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
    internal class RunHandler
    {
        // Default configuration.
        internal string ServiceName = "GoodbyeDPI";
        internal string ServiceDescription = "Passive Deep Packet Inspection blocker and Active DPI circumvention utility, installed via Configurator";
        internal string ServiceAutoStart = "auto";

        // Check if the system is 64-bit.
        internal static bool IsX64 = System.Environment.Is64BitOperatingSystem;

        // Folder path to extract service.
        internal static string FolderPath = Path.Combine(Path.GetTempPath(), "GoodbyeDPI");

        /// <summary>
        /// Launches the goodbyedpi.exe with the given profile.
        /// </summary>
        public void Launch(Profile ProfileToRun)
        {
            // Cleanup.
            Uninstall();

            // Unpack all the files.
            PUnpackAll();

            // Launch the program.
            PLaunch(ProfileToRun.ToArguments());
        }

        /// <summary>
        /// Install the service with the given profile.
        /// </summary>
        public void InstallService(Profile ProfileToInstall)
        {
            // Cleanup.
            Uninstall();

            // Unpack all the files.
            PUnpackAll();

            // Install the service.
            PInstallGoodbyeDPIService(ProfileToInstall.ToArguments());
        }

        /// <summary>
        /// Stops the goodbyedpi.exe, will be called on the application exit.
        /// </summary>
        public void Stop()
        {
            // Try to kill the process.
            PKillProcess("goodbyedpi");
        }

        /// <summary>
        /// Stops the services, removes them, kills the related process and deletes the folder.
        /// </summary>
        public void Uninstall()
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
            PRemoveService("GoodbyeDPI");
            PRemoveService("WinDivert1.4");

            // Try to kill the process.
            PKillProcess("goodbyedpi.exe");

            // Clear the folder.
            PClearFolder(true);
        }

        /// <summary>
        /// Creates a service with the name GoodbyeDPI and starts it.
        /// </summary>
        private void PInstallGoodbyeDPIService(string ServiceArguments)
        {
            // Get a new process.
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Load the arguments and start the service.
            process.StartInfo.Arguments += $"/C sc create \"GoodbyeDPI\" ";
            process.StartInfo.Arguments += $"binPath=\"{Path.Combine(FolderPath, "goodbyedpi.exe")} {ServiceArguments}\" ";
            process.StartInfo.Arguments += $"start=\"{ServiceAutoStart}\" ";
            process.StartInfo.Arguments += $"&& sc description \"GoodbyeDPI\" \"{ServiceDescription}\" ";
            process.StartInfo.Arguments += $"&& sc start \"GoodbyeDPI\"";

            // Run the program.
            process.Start();
            process.WaitForExit(500);
            process.Dispose();
        }

        /// <summary>
        /// Launches the goodbyedpi.exe with the given arguments.
        /// </summary>
        /// <param name="Arguments"></param>
        private void PLaunch(string Arguments)
        {
            // Set up a process to run the program.
            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(FolderPath, "goodbyedpi.exe");
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.UseShellExecute = false;
            // DEBUG purposes.
            // process.StartInfo.CreateNoWindow = true;

            // Run the program.
            process.Start();
            process.Dispose();
        }

        /// <summary>
        /// Kills the process with the given name.
        /// </summary>
        /// <param name="ProcessName"></param>
        private void PKillProcess(string ProcessName)
        {
            Process[] processes = Process.GetProcessesByName("goodbyedpi");
            
            foreach (Process byebye in processes)
            {
                byebye.Kill();
                byebye.WaitForExit();
                byebye.Dispose();
            }
        }

        /// <summary>
        /// Stops the given service.
        /// </summary>
        private void PStopService(string ServiceName)
        {
            // Set up a process to stop the service.
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C sc stop {ServiceName}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Run the program.
            process.Start();
            process.WaitForExit(500);
            process.Dispose();
        }

        /// <summary>
        /// Removes the given service.
        /// </summary>
        private void PRemoveService(string ServiceName)
        {
            // Set up a process to remove the service.
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C sc delete {ServiceName}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Run the program.
            process.Start();
            process.WaitForExit(500);
            process.Dispose();
        }

        /// <summary>
        /// Deletes the folder with all its contents.
        /// </summary>
        /// <param name="EraseFolder">Default is deleting the folder.</param>
        private void PClearFolder(bool EraseFolder = true)
        {
            // Delete old folder with its contents, if it exists.
            if (Directory.Exists(FolderPath))
            {
                Directory.Delete(FolderPath, true);
            }

            // Create a new folder.
            if (!EraseFolder)
            {
                Directory.CreateDirectory(FolderPath);
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
        /// Copy embedded resource into the folder.
        /// </summary>
        private void PUnpackEmbeddedFile(string FileName)
        {
            // Set resource path based on architecture.
            string architecture = !IsX64 ? "x86" : "x86_64";

            // Get stream to access resource file.
            using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"GoodbyeDPI_Configurator.GoodbyeDPIFiles.{architecture}.{FileName}"))
            {
                // Create file on the disk.
                using (FileStream fileStream = new FileStream(Path.Combine(FolderPath, FileName), FileMode.Create, FileAccess.ReadWrite))
                {
                    // Copy contents into the file.
                    resourceStream.CopyTo(fileStream);
                }
            }
        }
    }
}
