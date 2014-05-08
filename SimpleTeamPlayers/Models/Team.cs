using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleTeamPlayers.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime CreationDateClub { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }

    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
    }
}