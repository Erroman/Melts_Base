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

        public ICommand AddYouTubeViewers { get;}

        public YouTubeViewersViewModel() 
        {
            YouTubeViewersListingViewModel = new YouTubeViewersListingViewModel();
            YouTubeViewersDetailsViewModel = new YouTubeViewersDetailsViewModel();
        }

    }
}
