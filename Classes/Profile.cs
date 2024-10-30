using System.ComponentModel;
using System.Text.Json.Serialization;
using System;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace GoodbyeDPI_Configurator.Classes
{
    [Serializable]
    public class Profile : INotifyPropertyChanged
    {
        // Private fields.
        [JsonIgnore] private string _name;
        [JsonIgnore] private bool _blockPassiveDPI = false;
        [JsonIgnore] private bool _replaceHost = false;
        [JsonIgnore] private bool _removeSpaceBetweenHeaderValue = false;
        [JsonIgnore] private bool _mixHeaderCase = false;
        [JsonIgnore] private bool _extraSpaceBetweenMethodURI = false;
        [JsonIgnore] private bool _dontWaitForFirstAck = false;
        [JsonIgnore] private bool _parseHTTPAllPorts = false;
        [JsonIgnore] private bool _circumventWhenNoSNI = false;
        [JsonIgnore] private bool _fragmentSNI = false;
        [JsonIgnore] private bool _nativeFragmentation = false;
        [JsonIgnore] private bool _reverseFragmentation = false;
        [JsonIgnore] private bool _verboseDNSRedirectMessages = false;
        [JsonIgnore] private bool _wrongChecksum = false;
        [JsonIgnore] private bool _wrongSequence = false;

        [JsonIgnore] private bool _httpFragmentationEnabled = false;
        [JsonIgnore] private int _httpFragmentationValue = 0;

        [JsonIgnore] private bool _pHttpFragmentationEnabled = false;
        [JsonIgnore] private int _pHttpFragmentationValue = 0;

        [JsonIgnore] private bool _httpsFragmentationEnabled = false;
        [JsonIgnore] private int _httpsFragmentationValue = 0;

        [JsonIgnore] private bool _extraTCPPortToFragmentEnabled = false;
        [JsonIgnore] private int _extraTCPPortToFragmentValue = 0;

        [JsonIgnore] private bool _maxPayloadEnabled = false;
        [JsonIgnore] private int _maxPayloadValue = 0;

        [JsonIgnore] private bool _originalTTL = true;

        [JsonIgnore] private bool _customTTLEnabled = false;
        [JsonIgnore] private int _customTTLValue = 0;

        [JsonIgnore] private bool _minimumTTLEnabled = false;
        [JsonIgnore] private int _minimumTTLValue = 0;

        [JsonIgnore] private bool _ipv4DNSEnabled = false;
        [JsonIgnore] private string _ipv4DNSHost = "";
        [JsonIgnore] private bool _ipv4DNSPortEnabled = false;
        [JsonIgnore] private int _ipv4DNSPortValue = 0;

        [JsonIgnore] private bool _ipv6DNSEnabled = false;
        [JsonIgnore] private string _ipv6DNSHost = "";
        [JsonIgnore] private bool _ipv6DNSPortEnabled = false;
        [JsonIgnore] private int _ipv6DNSPortValue = 0;

        [JsonIgnore] private bool _autoTTLBaseTopMaxEnabled = false;
        [JsonIgnore] private int _autoTTLBase = 0;
        [JsonIgnore] private int _autoTTLTop = 0;
        [JsonIgnore] private int _autoTTLMax = 0;



        // Public properties.
        [JsonRequired]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public bool BlockPassiveDPI
        {
            get => _blockPassiveDPI;
            set
            {
                if (_blockPassiveDPI != value)
                {
                    _blockPassiveDPI = value;
                    OnPropertyChanged(nameof(BlockPassiveDPI));
                }
            }
        }

        public bool ReplaceHost
        {
            get => _replaceHost;
            set
            {
                if (_replaceHost != value)
                {
                    _replaceHost = value;
                    OnPropertyChanged(nameof(ReplaceHost));
                }
            }
        }

        public bool RemoveSpaceBetweenHeaderValue
        {
            get => _removeSpaceBetweenHeaderValue;
            set
            {
                if (_removeSpaceBetweenHeaderValue != value)
                {
                    _removeSpaceBetweenHeaderValue = value;
                    OnPropertyChanged(nameof(RemoveSpaceBetweenHeaderValue));
                }
            }
        }

        public bool MixHeaderCase
        {
            get => _mixHeaderCase;
            set
            {
                if (_mixHeaderCase != value)
                {
                    _mixHeaderCase = value;
                    OnPropertyChanged(nameof(MixHeaderCase));
                }
            }
        }

        public bool ExtraSpaceBetweenMethodURI
        {
            get => _extraSpaceBetweenMethodURI;
            set
            {
                if (_extraSpaceBetweenMethodURI != value)
                {
                    _extraSpaceBetweenMethodURI = value;
                    OnPropertyChanged(nameof(ExtraSpaceBetweenMethodURI));
                }
            }
        }

        public bool DontWaitForFirstAck
        {
            get => _dontWaitForFirstAck;
            set
            {
                if (_dontWaitForFirstAck != value)
                {
                    _dontWaitForFirstAck = value;
                    OnPropertyChanged(nameof(DontWaitForFirstAck));
                }
            }
        }

        public bool ParseHTTPAllPorts
        {
            get => _parseHTTPAllPorts;
            set
            {
                if (_parseHTTPAllPorts != value)
                {
                    _parseHTTPAllPorts = value;
                    OnPropertyChanged(nameof(ParseHTTPAllPorts));
                }
            }
        }

        public bool CircumventWhenNoSNI
        {
            get => _circumventWhenNoSNI;
            set
            {
                if (_circumventWhenNoSNI != value)
                {
                    _circumventWhenNoSNI = value;
                    OnPropertyChanged(nameof(CircumventWhenNoSNI));
                }
            }
        }

        public bool FragmentSNI
        {
            get => _fragmentSNI;
            set
            {
                if (_fragmentSNI != value)
                {
                    _fragmentSNI = value;
                    OnPropertyChanged(nameof(FragmentSNI));
                }
            }
        }

        public bool NativeFragmentation
        {
            get => _nativeFragmentation;
            set
            {
                if (_nativeFragmentation != value)
                {
                    _nativeFragmentation = value;
                    OnPropertyChanged(nameof(NativeFragmentation));
                }
            }
        }

        public bool ReverseFragmentation
        {
            get => _reverseFragmentation;
            set
            {
                if (_reverseFragmentation != value)
                {
                    _reverseFragmentation = value;
                    OnPropertyChanged(nameof(ReverseFragmentation));
                }
            }
        }

        public bool VerboseDNSRedirectMessages
        {
            get => _verboseDNSRedirectMessages;
            set
            {
                if (_verboseDNSRedirectMessages != value)
                {
                    _verboseDNSRedirectMessages = value;
                    OnPropertyChanged(nameof(VerboseDNSRedirectMessages));
                }
            }
        }

        public bool WrongChecksum
        {
            get => _wrongChecksum;
            set
            {
                if (_wrongChecksum != value)
                {
                    _wrongChecksum = value;
                    OnPropertyChanged(nameof(WrongChecksum));
                }
            }
        }

        public bool WrongSequence
        {
            get => _wrongSequence;
            set
            {
                if (_wrongSequence != value)
                {
                    _wrongSequence = value;
                    OnPropertyChanged(nameof(WrongSequence));
                }
            }
        }

        // Replace Tuple properties with individual components.

        public bool HTTPFragmentationEnabled
        {
            get => _httpFragmentationEnabled;
            set
            {
                if (_httpFragmentationEnabled != value)
                {
                    _httpFragmentationEnabled = value;
                    OnPropertyChanged(nameof(HTTPFragmentationEnabled));
                }
            }
        }

        public int HTTPFragmentationValue
        {
            get => _httpFragmentationValue;
            set
            {
                if (_httpFragmentationValue != value)
                {
                    _httpFragmentationValue = value;
                    OnPropertyChanged(nameof(HTTPFragmentationValue));
                }
            }
        }

        public bool PHTTPFragmentationEnabled
        {
            get => _pHttpFragmentationEnabled;
            set
            {
                if (_pHttpFragmentationEnabled != value)
                {
                    _pHttpFragmentationEnabled = value;
                    OnPropertyChanged(nameof(PHTTPFragmentationEnabled));
                }
            }
        }

        public int PHTTPFragmentationValue
        {
            get => _pHttpFragmentationValue;
            set
            {
                if (_pHttpFragmentationValue != value)
                {
                    _pHttpFragmentationValue = value;
                    OnPropertyChanged(nameof(PHTTPFragmentationValue));
                }
            }
        }

        public bool HTTPSFragmentationEnabled
        {
            get => _httpsFragmentationEnabled;
            set
            {
                if (_httpsFragmentationEnabled != value)
                {
                    _httpsFragmentationEnabled = value;
                    OnPropertyChanged(nameof(HTTPSFragmentationEnabled));
                }
            }
        }

        public int HTTPSFragmentationValue
        {
            get => _httpsFragmentationValue;
            set
            {
                if (_httpsFragmentationValue != value)
                {
                    _httpsFragmentationValue = value;
                    OnPropertyChanged(nameof(HTTPSFragmentationValue));
                }
            }
        }

        public bool ExtraTCPPortToFragmentEnabled
        {
            get => _extraTCPPortToFragmentEnabled;
            set
            {
                if (_extraTCPPortToFragmentEnabled != value)
                {
                    _extraTCPPortToFragmentEnabled = value;
                    OnPropertyChanged(nameof(ExtraTCPPortToFragmentEnabled));
                }
            }
        }

        public int ExtraTCPPortToFragmentValue
        {
            get => _extraTCPPortToFragmentValue;
            set
            {
                if (_extraTCPPortToFragmentValue != value)
                {
                    _extraTCPPortToFragmentValue = value;
                    OnPropertyChanged(nameof(ExtraTCPPortToFragmentValue));
                }
            }
        }

        public bool MaxPayloadEnabled
        {
            get => _maxPayloadEnabled;
            set
            {
                if (_maxPayloadEnabled != value)
                {
                    _maxPayloadEnabled = value;
                    OnPropertyChanged(nameof(MaxPayloadEnabled));
                }
            }
        }

        public int MaxPayloadValue
        {
            get => _maxPayloadValue;
            set
            {
                if (_maxPayloadValue != value)
                {
                    _maxPayloadValue = value;
                    OnPropertyChanged(nameof(MaxPayloadValue));
                }
            }
        }

        public bool OriginalTTL
        {
            get => _originalTTL;
            set
            {
                if (_originalTTL != value)
                {
                    _originalTTL = value;
                    OnPropertyChanged(nameof(CustomTTLEnabled));
                }
            }
        }

        public bool CustomTTLEnabled
        {
            get => _customTTLEnabled;
            set
            {
                if (_customTTLEnabled != value)
                {
                    _customTTLEnabled = value;
                    OnPropertyChanged(nameof(CustomTTLEnabled));
                }
            }
        }

        public int CustomTTLValue
        {
            get => _customTTLValue;
            set
            {
                if (_customTTLValue != value)
                {
                    _customTTLValue = value;
                    OnPropertyChanged(nameof(CustomTTLValue));
                }
            }
        }

        public bool MinimumTTLEnabled
        {
            get => _minimumTTLEnabled;
            set
            {
                if (_minimumTTLEnabled != value)
                {
                    _minimumTTLEnabled = value;
                    OnPropertyChanged(nameof(MinimumTTLEnabled));
                }
            }
        }

        public int MinimumTTLValue
        {
            get => _minimumTTLValue;
            set
            {
                if (_minimumTTLValue != value)
                {
                    _minimumTTLValue = value;
                    OnPropertyChanged(nameof(MinimumTTLValue));
                }
            }
        }

        public bool IPV4DNSEnabled
        {
            get => _ipv4DNSEnabled;
            set
            {
                if (_ipv4DNSEnabled != value)
                {
                    _ipv4DNSEnabled = value;
                    OnPropertyChanged(nameof(IPV4DNSEnabled));
                }
            }
        }

        public string IPV4DNSHost
        {
            get => _ipv4DNSHost;
            set
            {
                if (_ipv4DNSHost != value)
                {
                    _ipv4DNSHost = value;
                    OnPropertyChanged(nameof(IPV4DNSHost));
                }
            }
        }

        public bool IPV4DNSPortEnabled
        {
            get => _ipv4DNSPortEnabled;
            set
            {
                if (_ipv4DNSPortEnabled != value)
                {
                    _ipv4DNSPortEnabled = value;
                    OnPropertyChanged(nameof(IPV4DNSPortEnabled));
                }
            }
        }

        public int IPV4DNSPortValue
        {
            get => _ipv4DNSPortValue;
            set
            {
                if (_ipv4DNSPortValue != value)
                {
                    _ipv4DNSPortValue = value;
                    OnPropertyChanged(nameof(IPV4DNSPortValue));
                }
            }
        }

        public bool IPV6DNSEnabled
        {
            get => _ipv6DNSEnabled;
            set
            {
                if (_ipv6DNSEnabled != value)
                {
                    _ipv6DNSEnabled = value;
                    OnPropertyChanged(nameof(IPV6DNSEnabled));
                }
            }
        }

        public string IPV6DNSHost
        {
            get => _ipv6DNSHost;
            set
            {
                if (_ipv6DNSHost != value)
                {
                    _ipv6DNSHost = value;
                    OnPropertyChanged(nameof(IPV6DNSHost));
                }
            }
        }

        public bool IPV6DNSPortEnabled
        {
            get => _ipv6DNSPortEnabled;
            set
            {
                if (_ipv6DNSPortEnabled != value)
                {
                    _ipv6DNSPortEnabled = value;
                    OnPropertyChanged(nameof(IPV6DNSPortEnabled));
                }
            }
        }

        public int IPV6DNSPortValue
        {
            get => _ipv6DNSPortValue;
            set
            {
                if (_ipv6DNSPortValue != value)
                {
                    _ipv6DNSPortValue = value;
                    OnPropertyChanged(nameof(IPV6DNSPortValue));
                }
            }
        }

        public bool AutoTTLBaseTopMaxEnabled
        {
            get => _autoTTLBaseTopMaxEnabled;
            set
            {
                if (_autoTTLBaseTopMaxEnabled != value)
                {
                    _autoTTLBaseTopMaxEnabled = value;
                    OnPropertyChanged(nameof(AutoTTLBaseTopMaxEnabled));
                }
            }
        }

        public int AutoTTLBase
        {
            get => _autoTTLBase;
            set
            {
                if (_autoTTLBase != value)
                {
                    _autoTTLBase = value;
                    OnPropertyChanged(nameof(AutoTTLBase));
                }
            }
        }

        public int AutoTTLTop
        {
            get => _autoTTLTop;
            set
            {
                if (_autoTTLTop != value)
                {
                    _autoTTLTop = value;
                    OnPropertyChanged(nameof(AutoTTLTop));
                }
            }
        }

        public int AutoTTLMax
        {
            get => _autoTTLMax;
            set
            {
                if (_autoTTLMax != value)
                {
                    _autoTTLMax = value;
                    OnPropertyChanged(nameof(AutoTTLMax));
                }
            }
        }

        // PropertyChanged event handler
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal Profile DeepCopy()
        {
            return JsonSerializer.Deserialize<Profile>(JsonSerializer.Serialize(this));
        }

        internal string ToArguments()
        {
            return ProfileConverter.ProfileToArguments(this);
        }
    }
}
