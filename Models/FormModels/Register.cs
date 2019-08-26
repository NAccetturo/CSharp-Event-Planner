using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class Register
    {

        [Required(ErrorMessage="A first name is required.")]
        [MinLength(2)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage="A last name is required.")]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required(ErrorMessage="An email is required.")]
        [EmailAddress]
        [MinLength(4)]
        public string Email { get; set; }

        [Required(ErrorMessage="A password of eight characters or more is required.")]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage="Please confirm your password.")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}