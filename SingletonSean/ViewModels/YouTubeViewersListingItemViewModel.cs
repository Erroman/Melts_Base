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
        public string Username { get; }

        public ICommand  EditCommand { get; }
        public ICommand  DeleteCommand { get; }

        YouTubeViewersListingItemViewModel(string username) 
        {
            Username = username;
        }

    }
}
