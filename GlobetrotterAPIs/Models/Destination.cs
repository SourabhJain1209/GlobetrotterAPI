using System;
using System.Collections.Generic;

namespace GlobetrotterAPIs.Models
{
    public partial class Destination
    {
        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Clues { get; set; } = null!;
        public string FunFacts { get; set; } = null!;
        public string Trivia { get; set; } = null!;
    }
}
