using Notes.Models;
using Notes.Services;
using Notes.Stores;
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
        private readonly AccountServices _accountServices;
        private readonly ChallengeServices _challengeServices;


        private Account _currentAccount;
        public Account CurrentAccount
        {
            get
            {
                return _currentAccount;
            }
            set
            {
                _currentAccount = value;
                OnPropertyChanged(nameof(CurrentAccount));
            }
        }

        private string _greeting = "Welcome, ";
        public string Greeting
        {
            get
            {
                return _greeting;
            }
            set
            {
                _greeting = value;
                OnPropertyChanged(nameof(Greeting));
            }
        }


        public ObservableCollection<Challenges> UsersChallenges { get; set; }

        public ObservableCollection<Challenges> SponsoredChallenges { get; set; }


        public HomeViewModel()
        {
            _client = new BaseClient();
            _accountServices = new AccountServices(_client);
            _challengeServices = new ChallengeServices(_client);

            _currentAccount = new Account();
            UsersChallenges = new ObservableCollection<Challenges>();
            SponsoredChallenges= new ObservableCollection<Challenges>();

            GetAccount();
            GetAllChallenges();
        }

        private async void GetAccount()
        {
            CurrentAccount = await _accountServices.GetAccount("account_1");
            Greeting = Greeting + $"{CurrentAccount.User.FirstName}";
        }

        private async void GetAllChallenges()
        {
            IEnumerable<Challenges> challenges = await _challengeServices.GetAllChallenges();

            UsersChallenges.Clear();
            SponsoredChallenges.Clear();
            foreach (Challenges challenge in challenges)
            {
                UsersChallenges.Add(challenge);

                if (challenge.IsActive)
                {
                    SponsoredChallenges.Add(challenge);
                }
            }
        }
    }
}
