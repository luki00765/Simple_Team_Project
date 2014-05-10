using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleTeamPlayers.Models
{
    public class Image
    {
        public int ID { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string Extensions { get; set; }
    }
}