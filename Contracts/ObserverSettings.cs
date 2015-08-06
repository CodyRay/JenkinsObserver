using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using PropertyChanged;

namespace Contracts
{
    [ImplementPropertyChanged]
    public class ObserverSettings
    {
        public static ObserverSettings DefaultSettings { get
        {
            return new ObserverSettings
            {
                EnableNotifications = true,
                EnableSounds = true,
#if DEBUG
                PollingPeriod = 10000, //10 Sec
#else
                PollingPeriod = 300000, //5 Minutes
#endif
                AlertOnChangesOnly = false,
            };
        } }

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
