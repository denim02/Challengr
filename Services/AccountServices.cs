using Firebase.Database.Query;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services
{
    public class AccountServices
    {
        private readonly BaseClient _baseServices;

        public AccountServices(BaseClient baseServices)
        {
            _baseServices = baseServices;
        }
        public async Task<Account> GetAccount(string id)
        {
            return await _baseServices.Client
                    .Child("accounts")
                    .Child(id)
                    .OnceSingleAsync<Account>();
        }

        public async Task<ObservableCollection<Account>> GetAllAccounts()
        {
            ObservableCollection<Account> accountsList = new ObservableCollection<Account>();

            try
            {
                var collection = _baseServices.Client
                    .Child("accounts")
                    .OnceAsync<Account>();

                foreach (var account in await collection)
                {
                    accountsList.Add(account.Object);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return accountsList;
        }
    }
}
