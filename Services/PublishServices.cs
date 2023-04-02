using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using System.Net.Http;

namespace Notes.Services
{
    internal class PublishServices
    {

        private static readonly HttpClient client = new HttpClient();
        private readonly BaseClient _baseServices;
        public PublishServices(BaseClient baseServices)
        {
            _baseServices = baseServices;
        }
        public async Task PublishStartTime()
        {
            await _baseServices.Client
                .Child("user-challenges")
                .Child("userChallenge_1")
                .Child("DateStarted")
                .PutAsync(DateTime.Now);
        }

        public async Task PublishEndTime()
        {
            await _baseServices.Client
                .Child("user-challenges")
                .Child("userChallenge_1")
                .Child("DateCompleted")
                .PutAsync(DateTime.Now);
        }

        public async Task<List<DateTime>> ReadTimes()
        {
            var startTime = await _baseServices.Client
                .Child("user-challenges")
                .Child("userChallenge_1")
                .Child("DateStarted")
                .OnceSingleAsync<DateTime>();

            var endTime = await _baseServices.Client
                .Child("user-challenges")
                .Child("userChallenge_1")
                .Child("DateCompleted")
                .OnceSingleAsync<DateTime>();

            List<DateTime> times = new List<DateTime>();
            times.Add(startTime);
            times.Add(endTime);

            return times;
        }

        public async void SendCommandToDispenser()
        {
            try
            {
                var response = await client.GetAsync("http://192.168.247.109/finished");
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }


    }
}
