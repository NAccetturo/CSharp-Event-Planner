using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class ActivityForm
    {
        [Key]
        public int ActivityId {get;set;}

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required]
        public string Duration2 { get; set; }

        [Required]
        public DateTime DateAndTime { get; set; }

        [Required]
        [MaxLength(1200)]
        public string Description { get; set; }
        public List<RSVP> RSVPS { get; set; }

        [Required]
        public int UserId { get; set; }
        public User Creator { get; set; }

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}