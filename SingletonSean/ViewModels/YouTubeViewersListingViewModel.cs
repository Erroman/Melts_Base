using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonSean.ViewModels
{
    internal class YouTubeViewersListingViewModel:ViewModelBase
    {
        public IEnumerable<YouTubeViewersListingItemViewModel> YouTubeViewersListingItemViewModels { get; }
    }
}
