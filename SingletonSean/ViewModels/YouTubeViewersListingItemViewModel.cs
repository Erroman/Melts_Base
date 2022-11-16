using SingletonSean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SingletonSean.ViewModels
{
    internal class YouTubeViewersListingItemViewModel:ViewModelBase
    {
        public readonly YouTubeViewer YouTubeViewer;
        public string Username => YouTubeViewer.Username;

        public ICommand  EditCommand { get; }
        public ICommand  DeleteCommand { get; }

        public YouTubeViewersListingItemViewModel(YouTubeViewer youTubeViewer) 
        {
            YouTubeViewer=youTubeViewer;
        }

    }
}
