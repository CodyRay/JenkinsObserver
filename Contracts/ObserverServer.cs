using System;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    [ImplementPropertyChanged]
    public class ObserverServer
    {
        private bool _health;

        public bool Healthy
        {
            get { return _health && ValidUrl; }
            set { _health = value; }
        }

        public bool NotHealthy => !Healthy;

        public string Name { get; set; }
        public string Url { get; set; }
        public ObservableCollection<ObserverJob> Jobs { get; set; }

        public bool ValidUrl
        {
            get
            {
                if (Url == @"http://example.com/")
                    return false;
                try
                {
                    var uri = new Uri(Url);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public ObserverServer()
        {
            Jobs = new ObservableCollection<ObserverJob>();
        }
    }
}