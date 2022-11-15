using SingletonSean.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonSean.ViewModels
{
    internal class YouTubeViewersDetailsViewModel:ViewModelBase
    {
        private readonly SelectedYouTubeViewerStore _selectedYouTubeViewersStore;

        public string Username { get; }
        public string IsSubscribedDisplay { get; }
        public string IsMemberDisplay { get; }
        public YouTubeViewersDetailsViewModel(SelectedYouTubeViewerStore selectedYouTubeViewerStore ) 
        {
               _selectedYouTubeViewersStore = selectedYouTubeViewerStore;
        }
    }
}
