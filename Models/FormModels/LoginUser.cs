using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltExam.Models
{
    public class LoginUser
    {
    // No other fields!
    [Required(ErrorMessage="An email is required.")]
    [MinLength(4)]
    [EmailAddress]
    public string E_mail {get; set;}
    [Required(ErrorMessage="A password of eight characters or more is required.")]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string P_assword { get; set; }
    }
}