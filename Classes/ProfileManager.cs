using System;
using System.Linq;
using System.Text.Json;
using System.ComponentModel;

namespace GoodbyeDPI_Configurator.Classes
{
    internal class ProfileManager: INotifyPropertyChanged
    {
        // Event handler for profile change.
        public event PropertyChangedEventHandler PropertyChanged;

        private Profile _CurrentProfile;
        private BindingList<Profile> _Profiles = new BindingList<Profile>();

        // List of all profiles.
        public BindingList<Profile> Profiles
        {   
            get => _Profiles;
            set
            {
                _Profiles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Profiles"));
            }
        }

        // Currently selected profile. Everything happens to this profile.
        public Profile CurrentProfile
        {
            get => _CurrentProfile;
            set
            {
                _CurrentProfile = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentProfile"));
            }
        }

        // Get the stored profiles from the settings.
        internal ProfileManager()
        {
            Profiles = JsonSerializer.Deserialize<BindingList<Profile>>(Properties.Settings.Default.Profiles);

            Console.WriteLine(Properties.Settings.Default.Profiles);

            CurrentProfile = JsonSerializer.Deserialize<Profile>(Properties.Settings.Default.CurrentProfile);
        }

        internal void DeleteProfile(int index)
        {
            if (index < 0) { return; }
            Profiles.RemoveAt(index);
            SaveProfiles();
        }

        internal void SaveProfile(Profile Profile_To_Save, int index)
        {
            if (index < 0) { return; }
            Profiles.Insert(index, Profile_To_Save);
            Profiles.RemoveAt(index + 1);
            SaveProfiles();
        }

        internal void LoadProfile(int index)
        {
            if (index < 0) { return; }
            CurrentProfile = Profiles.ElementAt(index).DeepCopy();
        }

        internal void AddProfile(string new_name)
        {
            Profiles.Add(new Profile() { Name = new_name});
            CurrentProfile = Profiles.Last();
            SaveProfiles();
        }

        private void SaveProfiles()
        {
            Properties.Settings.Default.Profiles = JsonSerializer.Serialize(Profiles);
            Properties.Settings.Default.CurrentProfile = JsonSerializer.Serialize(CurrentProfile);
            Properties.Settings.Default.Save();
        }
    }
}
