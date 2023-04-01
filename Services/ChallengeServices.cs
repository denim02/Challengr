using Firebase.Database.Query;
using Notes.Models;
using Notes.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services
{
    internal class ChallengeServices
    {
        private readonly BaseClient _baseServices;

        public ChallengeServices(BaseClient baseServices)
        {
            _baseServices = baseServices;
        }

        public async Task<Challenges> GetChallenge(string id)
        {
            return await _baseServices.Client
                .Child("challenges")
                .Child(id)
                .OnceSingleAsync<Challenges>();
        }

        public async Task<IEnumerable<Challenges>> GetAllChallenges()
        {
            ObservableCollection<Challenges> challengesList = new ObservableCollection<Challenges>();

            try
            {
                var collection = _baseServices.Client
                    .Child("challenges")
                    .OnceAsync<Challenges>();

                foreach (var challenge in await collection)
                {
                    challengesList.Add(challenge.Object);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return challengesList;
        }
    }
}
