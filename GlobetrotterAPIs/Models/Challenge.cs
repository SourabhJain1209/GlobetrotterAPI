using System;
using System.Collections.Generic;

namespace GlobetrotterAPIs.Models
{
    public partial class Challenge
    {
        public int InviteCode { get; set; }
        public int? ChallengerId { get; set; }
        public string? ImageUrl { get; set; }
        public int? ChallengedTo { get; set; }

        public virtual User? ChallengedToNavigation { get; set; }
        public virtual User? Challenger { get; set; }
    }
}
