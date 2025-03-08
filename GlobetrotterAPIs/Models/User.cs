using System;
using System.Collections.Generic;

namespace GlobetrotterAPIs.Models
{
    public partial class User
    {
        public User()
        {
            ChallengeChallengedToNavigations = new HashSet<Challenge>();
            ChallengeChallengers = new HashSet<Challenge>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public int? Score { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? IncorrectAnswers { get; set; }

        public virtual ICollection<Challenge> ChallengeChallengedToNavigations { get; set; }
        public virtual ICollection<Challenge> ChallengeChallengers { get; set; }
    }
}
