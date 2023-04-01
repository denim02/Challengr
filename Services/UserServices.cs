using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Notes.Services;

namespace Notes.Services
{
    public class UserService
    {
        private readonly BaseClient _baseServices;

        public UserService(BaseClient baseServices)
        {
            _baseServices = baseServices;
        }

        public async Task<User> GetUser(string id)
        {
            var userRef = _baseServices.Client.Child("users").Child(id);
            var userData = new User();

            try
            {
               userData = await userRef.OnceSingleAsync<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return userData;
                
        }

        public async Task<ObservableCollection<User>> GetAllUsers()
        {
            ObservableCollection<User> usersList = new ObservableCollection<User>();

            try
            {
                var collection = _baseServices.Client
                    .Child("users")
                    .OnceAsync<User>();

                foreach (var user in await collection)
                {
                    usersList.Add(user.Object);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return usersList;
        }
    }
}
