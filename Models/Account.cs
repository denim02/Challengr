using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class Account : DomainObject
    {
        public User User { get; set; }
        public ICollection<Challenges> SavedChallenges { get; set; }
        public double RankingPoints { get; set; }
        public double Balance { get; set; }
    }
}
