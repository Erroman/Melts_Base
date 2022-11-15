﻿using SingletonSean.Stores;
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
        private readonly ObservableCollection<YouTubeViewersListingItemViewModel> _youTubeViewersListingItemViewModels;
        public IEnumerable<YouTubeViewersListingItemViewModel> YouTubeViewersListingItemViewModels => 
            _youTubeViewersListingItemViewModels;
        public YouTubeViewersListingViewModel(SelectedYouTubeViewerStore selectedYouTubeViewerStore) 
        {
            _youTubeViewersListingItemViewModels = new ObservableCollection<YouTubeViewersListingItemViewModel>();
            _youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel("Mary"));
            _youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel("Sean"));
            _youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel("Alan"));
        }
    }
}
