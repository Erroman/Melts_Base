using SingletonSean.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SingletonSean.ViewModels
{
    class YouTubeViewersViewModel:ViewModelBase
    {
        public YouTubeViewersListingViewModel YouTubeViewersListingViewModel { get; set; }
        public YouTubeViewersDetailsViewModel YouTubeViewersDetailsViewModel { get; set; }

        public ICommand AddYouTubeViewersCommand { get;}

        public YouTubeViewersViewModel(SelectedYouTubeViewerStore _selectedYouTubeViewerStore) 
        {
            YouTubeViewersListingViewModel = new YouTubeViewersListingViewModel(_selectedYouTubeViewerStore);
            YouTubeViewersDetailsViewModel = new YouTubeViewersDetailsViewModel(_selectedYouTubeViewerStore);
        }

    }
}
