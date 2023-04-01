using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace Notes.Services
{
    public class BaseClient
    {
        private FirebaseClient _client;
        public FirebaseClient Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
            }
        }

        public BaseClient()
        {
            Client = new FirebaseClient("https://hackathon-practice-aaaf2-default-rtdb.europe-west1.firebasedatabase.app/");
        }

    }
}
