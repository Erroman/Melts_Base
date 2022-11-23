using SingletonSean.Stores;
using SingletonSean.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonSean.ViewModels
{
    internal class YouTubeViewersListingViewModel:ViewModelBase
    { 
        private readonly SelectedYouTubeViewerStore _selectedYouTubeViewerStore;
        
        private readonly ObservableCollection<YouTubeViewersListingItemViewModel> _youTubeViewersListingItemViewModels;
       
        public IEnumerable<YouTubeViewersListingItemViewModel> YouTubeViewersListingItemViewModels => 
            _youTubeViewersListingItemViewModels;

        private YouTubeViewersListingItemViewModel _selectedYouTubeViewersListingItemViewModel;
        public YouTubeViewersListingItemViewModel SelectedYouTubeViewersListingItemViewModel
        {
            get { return _selectedYouTubeViewersListingItemViewModel; }
            set {
                _selectedYouTubeViewersListingItemViewModel = value;
                OnPropertyChanged(nameof(SelectedYouTubeViewersListingItemViewModel));

                _selectedYouTubeViewerStore.SelectedYouTubeViewer = _selectedYouTubeViewersListingItemViewModel?.YouTubeViewer   ;
           }
        }

        public YouTubeViewersListingViewModel(SelectedYouTubeViewerStore selectedYouTubeViewerStore) 
        {
            _selectedYouTubeViewerStore = selectedYouTubeViewerStore;
            _youTubeViewersListingItemViewModels = new ObservableCollection<YouTubeViewersListingItemViewModel>();

            _youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel(new YouTubeViewer("Mary",true,false)));
            _youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel(new YouTubeViewer("Sean",false,false)));
            _youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel(new YouTubeViewer("Alan", true, true)));
        }
    }
}
