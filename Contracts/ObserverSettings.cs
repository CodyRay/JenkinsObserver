using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    [ImplementPropertyChanged]
    public class ObserverSettings
    {
        public static ObserverSettings DefaultSettings => new ObserverSettings
        {
            EnableNotifications = true,
            EnableSounds = true,
            PollingPeriod = 1, //1 Min
            AlertOnChangesOnly = false,
        };

        [Key]
        public long Id { get; set; }

        public ObservableCollection<ObserverServer> Servers { get; set; }
        public bool EnableNotifications { get; set; }
        public bool EnableSounds { get; set; }
        public bool AlertOnChangesOnly { get; set; }
        public int PollingPeriod { get; set; }

        public ObserverSettings()
        {
            Servers = new ObservableCollection<ObserverServer>();
        }
    }
}