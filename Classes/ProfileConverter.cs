using System;
using System.Text;

namespace GoodbyeDPI_Configurator.Classes
{
    internal static class ProfileConverter
    {
        /// <summary>
        /// This method is responsible for converting a <see cref="Profile"/> object to a string of arguments that can be passed to the GoodbyeDPI executable.
        /// </summary>
        public static string ProfileToArguments(Profile profile)
        {
            StringBuilder arguments = new StringBuilder();

            // Single value flags.
            if (profile.BlockPassiveDPI)
            {
                arguments.Append(" -p");
            }
            if (profile.ReplaceHost)
            {
                arguments.Append(" -r");
            }
            if (profile.RemoveSpaceBetweenHeaderValue)
            {
                arguments.Append(" -s");
            }
            if (profile.MixHeaderCase)
            {
                arguments.Append(" -m");
            }
            if (profile.ExtraSpaceBetweenMethodURI)
            {
                arguments.Append(" -a");
            }
            if (profile.DontWaitForFirstAck)
            {
                arguments.Append(" -n");
            }
            if (profile.ParseHTTPAllPorts)
            {
                arguments.Append(" -w");
            }
            if (profile.CircumventWhenNoSNI)
            {
                arguments.Append(" --allow-no-sni");
            }
            if (profile.FragmentSNI)
            {
                arguments.Append(" --frag-by-sni");
            }
            if (profile.NativeFragmentation)
            {
                arguments.Append(" --native-frag");
            }
            if (profile.ReverseFragmentation)
            {
                arguments.Append(" --reverse-frag");
            }
            if (profile.VerboseDNSRedirectMessages)
            {
                arguments.Append(" --dns-verb");
            }
            if (profile.WrongChecksum)
            {
                arguments.Append(" --wrong-chksum");
            }
            if (profile.WrongSequence)
            {
                arguments.Append(" --wrong-seq");
            }

            // Tuple properties (e.g., HTTPFragmentation).
            if (profile.HTTPFragmentationEnabled)
            {
                arguments.Append($" -f {profile.HTTPFragmentationValue}");
            }
            if (profile.PHTTPFragmentationEnabled)
            {
                arguments.Append($" -k {profile.PHTTPFragmentationValue}");
            }
            if (profile.HTTPSFragmentationEnabled)
            {
                arguments.Append($" -e {profile.HTTPSFragmentationValue}");
            }
            if (profile.ExtraTCPPortToFragmentEnabled)
            {
                arguments.Append($" --port {profile.ExtraTCPPortToFragmentValue}");
            }
            if (profile.MaxPayloadEnabled)
            {
                arguments.Append($" --max-payload");

                if (profile.MaxPayloadValue > 0)
                {
                    arguments.Append($" {profile.MaxPayloadValue}");
                }
            }
            if (profile.CustomTTLEnabled)
            {
                arguments.Append($" --set-ttl {profile.CustomTTLValue}");
            }

            if (profile.AutoTTLBaseTopMaxEnabled)
            {
                arguments.Append($" --auto-ttl");
                
                if (profile.AutoTTLBase > profile.AutoTTLTop && profile.AutoTTLMax != 0)
                {
                    arguments.Append($" {profile.AutoTTLBase}-{profile.AutoTTLTop}-{profile.AutoTTLMax}");
                }
            }

            if (profile.MinimumTTLEnabled)
            {
                arguments.Append($" --min-ttl {profile.MinimumTTLValue}");
            }

            // DNS options.
            if (profile.IPV4DNSEnabled)
            {
                arguments.Append($" --dns-addr {profile.IPV4DNSHost} --dns-port {profile.IPV4DNSPortValue}");
            }
            if (profile.IPV6DNSEnabled)
            {
                arguments.Append($" --dnsv6-addr {profile.IPV6DNSHost} --dnsv6-port {profile.IPV6DNSPortValue}");
            }

            return arguments.ToString();
        }

        public static Profile ArgumentsToProfile(string arguments)
        {
            throw new NotImplementedException();
        }
    }
}
