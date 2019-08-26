using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class RSVP
    {
        [Key]
        public int RSVPId {get;set;}

        [Required]
        public int UserId {get;set;}
        
        [Required]
        public int EventId {get;set;}

        public User User { get; set; }

        public Event Event { get; set; }

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}