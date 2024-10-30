using GoodbyeDPI_Configurator.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows.Forms;

namespace GoodByeDPI_Configurator
{
    public partial class MainForm : Form
    {
        // Handles the GoodbyeDPI Service.
        private readonly RunHandler RunHandler = new RunHandler();
        // Handles the Profiles.
        private readonly ProfileManager ProfileManager = new ProfileManager();


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Fancy hover text.
            LoadToolTips();

            BindProfileListToProfileManager();

            // Bind Stop WinDivert checkbox to settings.
            // Note: Haven't connected yet.
            BindForceStopWinDivertToSettings();

            // Clear the binds of the UI.
            ClearControlBindsToProfile();

            // Bind the profile to the UI.
            BindControlsToProfile();
        }


        // TODO: There must be a better way of doing this.
        private void LoadToolTips()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(BlockPassiveCBox, "Block passive DPI.");
            toolTip.SetToolTip(ReplaceHostCBox, "Replace Host with hoSt.");
            toolTip.SetToolTip(RemoveSpaceBetweenHeaderValueCBox, "Remove space between host header and its value.");
            toolTip.SetToolTip(MixHeaderCBox, "Mix Host header case. (test.com -> tEsT.cOm)");
            toolTip.SetToolTip(ExtraSpaceBetweenMethodURICBox, "Additional space between Method and Request-URI. (enables -s, may break sites)");
            toolTip.SetToolTip(HTTPFragmentationCBox, "Set HTTP fragmentation to value.");
            toolTip.SetToolTip(HTTPFragmentationNBox, "HTTP fragmentation value.");
            toolTip.SetToolTip(PHTTPFragmentationCBox, "Enable HTTP persistent (keep-alive) fragmentation and set it to value.");
            toolTip.SetToolTip(PHTTPFragmentationNBox, "HTTP persistent (keep-alive) fragmentation value.");
            toolTip.SetToolTip(HTTPSFragmentationCBox, "Set HTTPS fragmentation to value.");
            toolTip.SetToolTip(HTTPSFragmentationNBox, "HTTPS fragmentation value.");
            toolTip.SetToolTip(DontWaitForFirstAckCBox, "Do not wait for first segment ACK when -k is enabled");
            toolTip.SetToolTip(ExtraTCPPortToFragmentCBox, "Additional TCP port to perform fragmentation on. (and HTTP tricks with -w)");
            toolTip.SetToolTip(ParseHTTPAllPortsCBox, "Try to find and parse HTTP traffic on all processed ports. (not only on port 80)");
            toolTip.SetToolTip(CircumventWhenNoSNICBox, "Perform circumvention if TLS SNI can't be detected with --blacklist enabled.");
            toolTip.SetToolTip(FragmentSNICBox, "If SNI is detected in TLS packet, fragment the packet right before SNI value.");
            toolTip.SetToolTip(NativeFragmentationCBox, "Fragment (split) the packets by sending them in smaller packets, without shrinking the Window Size. Works faster (does not slow down the connection) and better.");
            toolTip.SetToolTip(ReverseFragmentationCBox, "Fragment (split) the packets just as --native-frag, but send them in the reversed order. Works with the websites which could not handle segmented HTTPS TLS ClientHello. (because they receive the TCP flow \"combined\")");
            toolTip.SetToolTip(MaxPayloadCBox, "Packets with TCP payload data more than [value] won't be processed. Use this option to reduce CPU usage by skipping huge amount of data (like file transfers) in already established sessions. May skip some huge HTTP requests from being processed.\r\nDefault (if set): --max-payload 1200.");
            toolTip.SetToolTip(MaxPayloadNBox, "Maximum payload data size.");
            toolTip.SetToolTip(IPV4DNSAddressCBox, "Redirect UDP DNS requests to the supplied IP address. (experimental)");
            toolTip.SetToolTip(IPV4DNSAddressVBox, "IPv4 DNS address.");
            toolTip.SetToolTip(IPV4DNSPortCBox, "Redirect UDP DNS requests to the supplied port. (53 by default)");
            toolTip.SetToolTip(IPV4DNSPortNBox, "IPv4 DNS port.");
            toolTip.SetToolTip(IPV6DNSAddressCBox, "Redirect UDPv6 DNS requests to the supplied IPv6 address. (experimental)");
            toolTip.SetToolTip(IPV6DNSAddressVBox, "IPv6 DNS address.");
            toolTip.SetToolTip(IPV6DNSPortCBox, "Redirect UDPv6 DNS requests to the supplied port. (53 by default)");
            toolTip.SetToolTip(IPV6DNSPortNBox, "IPv6 DNS port.");
            toolTip.SetToolTip(VerboseDNSRedirectMessagesCBox, "Print verbose DNS redirection messages.");
            toolTip.SetToolTip(NoTTLChangeRButton, "Keep TTL as is.");
            toolTip.SetToolTip(CustomTTLRButton, "Activate Fake Request Mode and send it with supplied TTL value.\r\nDANGEROUS! May break websites in unexpected ways. Use with care. (or --blacklist)");
            toolTip.SetToolTip(CustomTTLNBox, "TTL value.");
            toolTip.SetToolTip(AutoTTLRButton, "Activate Fake Request Mode, automatically detect TTL and decrease it based on a distance. If the distance is shorter than a2, TTL is decreased by a2. If it's longer, (a1; a2) scale is used with the distance as a weight. If the resulting TTL is more than m(ax), set it to m.\r\nDefault (if set): --auto-ttl 1-4-10. Also sets --min-ttl 3.\r\nDANGEROUS! May break websites in unexpected ways. Use with care. (or --blacklist)");
            toolTip.SetToolTip(AutoTTLScalerBaseNBox, "Base value for the --auto-ttl scaler. (a1)");
            toolTip.SetToolTip(AutoTTLScalerTopNBox, "Top value for the --auto-ttl scaler. (a2)");
            toolTip.SetToolTip(AutoTTLMaxNBox, "Maximum TTL value for the --auto-ttl scaler. (m)");
            toolTip.SetToolTip(MinimumTTLCBox, "Minimum TTL distance (128/64 - TTL) for which to send Fake Request in --set-ttl and --auto-ttl modes.");
            toolTip.SetToolTip(MinimumTTLNBox, "Minimum TTL value.");
            toolTip.SetToolTip(WrongChecksumCBox, "Activate Fake Request Mode and send it with incorrect TCP checksum.\r\nMay not work in a VM or with some routers, but is safer than set-ttl.\r\nNote: Combination of --wrong-seq and --wrong-chksum generates two different fake packets.");
            toolTip.SetToolTip(WrongSequenceCBox, "Activate Fake Request Mode and send it with TCP SEQ/ACK in the past.\r\nNote: Combination of --wrong-seq and --wrong-chksum generates two different fake packets.");
            toolTip.SetToolTip(RemoveButton, "Removes the service and installed files.");
            toolTip.SetToolTip(InstallButton, "Installs the service.");

            toolTip.SetToolTip(ProfileLoadButton, "Load selected profile.");
            toolTip.SetToolTip(ProfileSaveButton, "Save current profile to selected profile.");
            toolTip.SetToolTip(ProfileDeleteButton, "Delete selected profile.");
            toolTip.SetToolTip(ProfileAddButton, "Add empty profile with given name.");
            toolTip.SetToolTip(ProfileAddVBox, "Name of the new profile.");
            toolTip.SetToolTip(ProfileListBox, "Double click on a profile to load\nCTRL: Double-click to rename.");

            toolTip.SetToolTip(ProfileCopyButton, "Copy the current profile as arguments to the clipboard.\nSHIFT: Copy current profile in JSON format.\nCTRL: Copy all profiles in JSON format.");
            toolTip.SetToolTip(ProfilePasteButton, "Add profile(s) from the clipboard.");

            // TODO: Add more tooltips.

        }

        /*
           FUNCTIONS RELATED TO RUNHANDLER.
        */
        private void InstallButton_Click(object sender, EventArgs e)
        {
            RunHandler.InstallService(ProfileManager.CurrentProfile);
        }
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            RunHandler.Uninstall();
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            RunHandler.Launch(ProfileManager.CurrentProfile);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            RunHandler.Stop();
        }

        /*
           FUNCTIONS RELATED TO PROFILE MANAGEMENT.
        */

        /// <summary>
        /// Loads the <see cref="Profile"/> stored in the <see cref="ProfileManager"/> to the UI elements.
        /// </summary>
        private void ProfileLoadButton_Click(object sender, EventArgs e)
        {
            // Clear the binds of the UI.
            ClearControlBindsToProfile();

            // Load the profile.
            ProfileManager.LoadProfile(ProfileListBox.SelectedIndex);

            // Bind the profile to the UI.
            BindControlsToProfile();
        }

        /// <summary>
        /// Saves the changes made to the <see cref="Profile"/> in the UI elements to the <see cref="ProfileManager"/>.
        /// </summary>
        private void ProfileSaveButton_Click(object sender, EventArgs e)
        {
            ProfileManager.SaveProfile(ProfileManager.CurrentProfile, ProfileListBox.SelectedIndex);

            if (ProfileListBox.SelectedIndex != -1) { ProfileListBox.SelectedIndex -= 1; }
        }

        /// <summary>
        /// Deletes the selected <see cref="Profile"/> from the <see cref="ProfileManager"/>.
        /// </summary>
        private void ProfileDeleteButton_Click(object sender, EventArgs e)
        {
            ProfileManager.DeleteProfile(ProfileListBox.SelectedIndex);
        }

        // TODO: Selected index handle.

        /// <summary>
        /// Copies the current profile to the clipboard depending on the modifier keys.
        /// Ctrl: All profiles as JSON.
        /// Shift: Current profile as JSON.
        /// None: Current profile as arguments.
        /// </summary>
        private void ProfileCopyButton_Click(object sender, EventArgs e)
        {
            switch (ModifierKeys)
            {
                // User wants all profiles.
                case Keys.Control:
                    Clipboard.SetText(JsonSerializer.Serialize(ProfileManager.Profiles));
                    break;
                // User wants the current profile.
                case Keys.Shift:
                    Clipboard.SetText(JsonSerializer.Serialize<Profile>(ProfileManager.CurrentProfile));
                    break;
                // User wants the current profile as arguments.
                default:
                    Clipboard.SetText(ProfileConverter.ProfileToArguments(ProfileManager.CurrentProfile));
                    break;
                }
            }

        /// <summary>
        /// Tries to create the profile(s) from the clipboard.
        /// </summary>
        private void ProfilePasteButton_Click(object sender, EventArgs e)
        {
            ProfileConverter.ClipboardToProfile(ProfileManager, Clipboard.GetText());
        }

        /// <summary>
        /// Only enables the Add button if the text box has text.
        /// </summary>
        private void ProfileAddVBox_TextChanged(object sender, EventArgs e)
        {
            ProfileAddButton.Enabled = ProfileAddVBox.Text.Length > 0;
        }

        /// <summary>
        /// Creates a new <see cref="Profile"/> with the name in the text box then loads it.
        /// </summary>
        private void ProfileAddButton_Click(object sender, EventArgs e)
        {
            ProfileManager.AddProfile(ProfileManager.CurrentProfile);
            ProfileManager.LoadProfile(ProfileManager.Profiles.Count - 1);
        }

        /*
           FUNCTIONS RELATED TO BINDS.
        */

        /// <summary>
        /// Binds the <see cref="KillWinDivertCBox"/> to the <see cref="GoodbyeDPI_Configurator.Properties.Settings"/>.
        /// </summary>
        private void BindForceStopWinDivertToSettings()
        {
            KillWinDivertCBox.DataBindings.Add("Enabled", GoodbyeDPI_Configurator.Properties.Settings.Default, "KillWinDivert", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        /// <summary>
        /// Binds the <see cref="ProfileManager"/> to the <see cref="ProfileListBox"/>.
        /// </summary>
        private void BindProfileListToProfileManager()
        {
            // Profile List
            ProfileListBox.DataBindings.Add("DataSource", ProfileManager, "Profiles", false, DataSourceUpdateMode.OnPropertyChanged);
            ProfileListBox.DisplayMember = "Name";
        }

        private void ClearControlBindsToProfile()
        {
            BlockPassiveCBox.DataBindings.Clear();
            ReplaceHostCBox.DataBindings.Clear();
            RemoveSpaceBetweenHeaderValueCBox.DataBindings.Clear();
            MixHeaderCBox.DataBindings.Clear();
            ExtraSpaceBetweenMethodURICBox.DataBindings.Clear();
            DontWaitForFirstAckCBox.DataBindings.Clear();
            ParseHTTPAllPortsCBox.DataBindings.Clear();
            CircumventWhenNoSNICBox.DataBindings.Clear();
            FragmentSNICBox.DataBindings.Clear();
            NativeFragmentationCBox.DataBindings.Clear();
            ReverseFragmentationCBox.DataBindings.Clear();
            VerboseDNSRedirectMessagesCBox.DataBindings.Clear();
            WrongChecksumCBox.DataBindings.Clear();
            WrongSequenceCBox.DataBindings.Clear();
            HTTPFragmentationCBox.DataBindings.Clear();
            HTTPFragmentationNBox.DataBindings.Clear();
            PHTTPFragmentationCBox.DataBindings.Clear();
            PHTTPFragmentationNBox.DataBindings.Clear();
            HTTPSFragmentationCBox.DataBindings.Clear();
            HTTPSFragmentationNBox.DataBindings.Clear();
            ExtraTCPPortToFragmentCBox.DataBindings.Clear();
            ExtraTCPPortToFragmentNBox.DataBindings.Clear();
            MaxPayloadCBox.DataBindings.Clear();
            MaxPayloadNBox.DataBindings.Clear();
            IPV4DNSAddressCBox.DataBindings.Clear();
            IPV4DNSAddressVBox.DataBindings.Clear();
            IPV4DNSPortCBox.DataBindings.Clear();
            IPV4DNSPortNBox.DataBindings.Clear();
            IPV6DNSAddressCBox.DataBindings.Clear();
            IPV6DNSAddressVBox.DataBindings.Clear();
            IPV6DNSPortCBox.DataBindings.Clear();
            IPV6DNSPortNBox.DataBindings.Clear();
            NoTTLChangeRButton.DataBindings.Clear();
            CustomTTLRButton.DataBindings.Clear();
            CustomTTLNBox.DataBindings.Clear();
            AutoTTLRButton.DataBindings.Clear();
            AutoTTLScalerBaseNBox.DataBindings.Clear();
            AutoTTLScalerTopNBox.DataBindings.Clear();
            AutoTTLMaxNBox.DataBindings.Clear();
            MinimumTTLCBox.DataBindings.Clear();
            MinimumTTLNBox.DataBindings.Clear();
        }

        /// <summary>
        /// Binds the controls to the loaded profile.
        /// </summary>
        private void BindControlsToProfile()
        {
            // Block Passive -p
            BlockPassiveCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "BlockPassiveDPI", false, DataSourceUpdateMode.OnPropertyChanged);
            // Replace Host -r
            ReplaceHostCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "ReplaceHost", false, DataSourceUpdateMode.OnPropertyChanged);
            // Remove Space Between Header Value -s
            RemoveSpaceBetweenHeaderValueCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "RemoveSpaceBetweenHeaderValue", false, DataSourceUpdateMode.OnPropertyChanged);
            // Mix Header -m
            MixHeaderCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "MixHeaderCase", false, DataSourceUpdateMode.OnPropertyChanged);
            // Extra Space Between Method and URI -a
            ExtraSpaceBetweenMethodURICBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "ExtraSpaceBetweenMethodURI", false, DataSourceUpdateMode.OnPropertyChanged);
            // Don't Wait for First ACK -n
            DontWaitForFirstAckCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "DontWaitForFirstAck", false, DataSourceUpdateMode.OnPropertyChanged);
            // Parse HTTP on All Ports -w
            ParseHTTPAllPortsCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "ParseHTTPAllPorts", false, DataSourceUpdateMode.OnPropertyChanged);
            // Circumvent When No SNI --allow-no-sni
            CircumventWhenNoSNICBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "CircumventWhenNoSNI", false, DataSourceUpdateMode.OnPropertyChanged);
            // Fragment SNI --frag-by-sni
            FragmentSNICBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "FragmentSNI", false, DataSourceUpdateMode.OnPropertyChanged);
            // Native Fragmentation --native-frag
            NativeFragmentationCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "NativeFragmentation", false, DataSourceUpdateMode.OnPropertyChanged);
            // Reverse Fragmentation --reverse-frag
            ReverseFragmentationCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "ReverseFragmentation", false, DataSourceUpdateMode.OnPropertyChanged);
            // Verbose DNS Redirect Messages --dns-verb
            VerboseDNSRedirectMessagesCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "VerboseDNSRedirectMessages", false, DataSourceUpdateMode.OnPropertyChanged);
            // Wrong Checksum --wrong-chksum
            WrongChecksumCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "WrongChecksum", false, DataSourceUpdateMode.OnPropertyChanged);
            // Wrong Sequence --wrong-seq
            WrongSequenceCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "WrongSequence", false, DataSourceUpdateMode.OnPropertyChanged);
            // HTTP Fragmentation -f <value>
            HTTPFragmentationCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "HTTPFragmentationEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            HTTPFragmentationNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "HTTPFragmentationValue", false, DataSourceUpdateMode.OnPropertyChanged);
            // HTTP Persistent Fragmentation -k <value>
            PHTTPFragmentationCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "PHTTPFragmentationEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            PHTTPFragmentationNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "PHTTPFragmentationValue", false, DataSourceUpdateMode.OnPropertyChanged);
            // HTTPS Fragmentation -e <value>
            HTTPSFragmentationCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "HTTPSFragmentationEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            HTTPSFragmentationNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "HTTPSFragmentationValue", false, DataSourceUpdateMode.OnPropertyChanged);
            // Extra TCP Port to Fragment --port <value>
            ExtraTCPPortToFragmentCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "ExtraTCPPortToFragmentEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            ExtraTCPPortToFragmentNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "ExtraTCPPortToFragmentValue", false, DataSourceUpdateMode.OnPropertyChanged);
            // Max Payload --max-payload <value>
            MaxPayloadCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "MaxPayloadEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            MaxPayloadNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "MaxPayloadValue", false, DataSourceUpdateMode.OnPropertyChanged);
            // IPv4 DNS Address --dns-addr <value> --dns-port <value>
            IPV4DNSAddressCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "IPV4DNSEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            IPV4DNSAddressVBox.DataBindings.Add("Text", ProfileManager.CurrentProfile, "IPV4DNSHost", false, DataSourceUpdateMode.OnPropertyChanged);
            IPV4DNSPortCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "IPV4DNSPortEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            IPV4DNSPortNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "IPV4DNSPortValue", false, DataSourceUpdateMode.OnPropertyChanged);
            // IPv6 DNS Address --dnsv6-addr <value> --dnsv6-port <value>
            IPV6DNSAddressCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "IPV6DNSEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            IPV6DNSAddressVBox.DataBindings.Add("Text", ProfileManager.CurrentProfile, "IPV6DNSHost", false, DataSourceUpdateMode.OnPropertyChanged);
            IPV6DNSPortCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "IPV6DNSPortEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            IPV6DNSPortNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "IPV6DNSPortValue", false, DataSourceUpdateMode.OnPropertyChanged);
            // Custom TTL --set-ttl <value>
            CustomTTLRButton.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "CustomTTLEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            CustomTTLNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "CustomTTLValue", false, DataSourceUpdateMode.OnPropertyChanged);
            // Auto TTL --auto-ttl [a1-a2-m]
            AutoTTLRButton.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "AutoTTLBaseTopMaxEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            AutoTTLScalerBaseNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "AutoTTLBase", false, DataSourceUpdateMode.OnPropertyChanged);
            AutoTTLScalerTopNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "AutoTTLTop", false, DataSourceUpdateMode.OnPropertyChanged);
            AutoTTLMaxNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "AutoTTLMax", false, DataSourceUpdateMode.OnPropertyChanged);
            // Minimum TTL --min-ttl <value>
            MinimumTTLCBox.DataBindings.Add("Checked", ProfileManager.CurrentProfile, "MinimumTTLEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            MinimumTTLNBox.DataBindings.Add("Value", ProfileManager.CurrentProfile, "MinimumTTLValue", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        /*
           FUNCTIONS RELATED TO THE UI.
        */

        /// <summary>
        /// Creates a <see cref="TextBox"/> for user to rename their profile.
        /// </summary>
        private void ProfileListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (ProfileListBox.SelectedIndex != -1)
            {
                switch (ModifierKeys)
                {
                    case Keys.Control:
                        // Create a temporary textbox.
                        TextBox EditProfileNameVBox = new TextBox();

                        // Get the location of selected item.
                        Rectangle itemRectangle = ProfileListBox.GetItemRectangle(ProfileListBox.SelectedIndex);

                        // Event handlers to handle the editing.
                        EditProfileNameVBox.KeyDown += EditProfileNameVBox_KeyDown;
                        EditProfileNameVBox.LostFocus += EditProfileNameVBox_LostFocus;

                        // Set the bounds of the textbox.
                        EditProfileNameVBox.SetBounds(itemRectangle.X, itemRectangle.Y, itemRectangle.Width, itemRectangle.Height - 15);

                        // Set the text of the textbox.
                        EditProfileNameVBox.Text = (ProfileListBox.SelectedItem as Profile).Name;
                        EditProfileNameVBox.BorderStyle = BorderStyle.None;
                        EditProfileNameVBox.Visible = true;

                        // Add the textbox to the listbox as control.
                        ProfileListBox.Controls.Add(EditProfileNameVBox);

                        // Focus and select all the text in the textbox.
                        EditProfileNameVBox.Focus();
                        EditProfileNameVBox.SelectAll();
                        break;
                    default:
                        ProfileLoadButton_Click(sender, e);
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the keydown event of the <see cref="TextBox"/> to save the edit on Enter key.
        /// </summary>
        private void EditProfileNameVBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Save the edit on Enter key.
            if (e.KeyCode == Keys.Enter)
            {
                SaveEdit(sender);
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Handles the lost focus event of the <see cref="TextBox"/> to save the edit.
        /// </summary>
        private void EditProfileNameVBox_LostFocus(object sender, EventArgs e)
        {
            SaveEdit(sender);
        }

        /// <summary>
        /// Saves the edit made to the profile name and disposes the <see cref="TextBox"/>. 
        /// </summary>
        private void SaveEdit(object sender)
        {
            // Get the textbox.
            TextBox EditProfileNameVBox = sender as TextBox;

            // Unbind the event handlers.
            EditProfileNameVBox.KeyDown -= EditProfileNameVBox_KeyDown;
            EditProfileNameVBox.LostFocus -= EditProfileNameVBox_LostFocus;

            if (ProfileListBox.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(EditProfileNameVBox.Text))
            {
                (ProfileListBox.SelectedItem as Profile).Name = EditProfileNameVBox.Text;
            }

            ProfileListBox.Controls.Remove(EditProfileNameVBox);
            EditProfileNameVBox.Dispose();
        }
    }
}

