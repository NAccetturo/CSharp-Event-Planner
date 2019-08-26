using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class Event
    {
        [Key]
        public int EventId {get;set;}
        public string Name { get; set; }
        public string Duration { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Description { get; set; }
        public List<RSVP> RSVPS { get; set; }
        public int UserId { get; set; }
        public User Creator { get; set; }

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}