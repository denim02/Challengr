using Notes.Models;
using Notes.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly BaseClient _client;
        private readonly UserService _userService;

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private User _currentUser;
        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public HomeViewModel()
        {
            _client = new BaseClient();
            _userService = new UserService(_client);

            GetUsers();
            
        }

        public async void GetUsers()
        {
            CurrentUser = await _userService.GetUser("denimastori02");
        }
    }
}
