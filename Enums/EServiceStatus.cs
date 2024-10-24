namespace GoodbyeDPI_Configurator.Enums
{
    public enum EServiceStatus
    {
        Not_Installed,
        Running_Service,
        Running,
        Unkown
    }

    public static class EServiceStatusExtensions
    {
        public static string ToFriendlyString(this EServiceStatus status)
        {
            switch (status)
            {
                case EServiceStatus.Not_Installed:
                    return "Not Installed";
                case EServiceStatus.Running_Service:
                    return "Running as Service";
                case EServiceStatus.Running:
                    return "Running";
                default:
                    return "Unkown";
            }
        }
    }
}
