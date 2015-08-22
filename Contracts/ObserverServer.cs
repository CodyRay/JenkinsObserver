using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    [ImplementPropertyChanged]
    public class ObserverServer
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ObservableCollection<ObserverJob> Jobs { get; set; }

        public ObserverServer()
        {
            Jobs = new ObservableCollection<ObserverJob>();
        }
    }
}