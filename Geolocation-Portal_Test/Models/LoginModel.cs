using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Geolocation_Portal_Test.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Gib bitte deinen Benutzernamen ein.")]
        [Display(Name ="Benutzername:")]
        [StringLength(60, MinimumLength = 3)]
        public string username { get; set; }

        [Required(ErrorMessage ="Gib bitte dein Passwort ein.")]
        [Display(Name ="Passwort:")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 3)]
        public string password { get; set; }

        [Display(Name ="Angemeldet bleiben:")]
        public bool RememberMe { get; set; }
    }
}