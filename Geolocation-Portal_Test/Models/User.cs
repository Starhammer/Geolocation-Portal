//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Geolocation_Portal_Test.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class user
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Rollen ID")]
        public int role_id { get; set; }

        [DisplayName("Arbeitsstätten ID")]
        public int department_id { get; set; }

        [DisplayName("Vorname")]
        public string first_name { get; set; }

        [DisplayName("Nachname")]
        public string last_name { get; set; }

        [DisplayName("Benutzername")]
        public string username { get; set; }

        [DisplayName("Passwort")]
        public string password { get; set; }

        [DisplayName("Letzte Passwortänderung")]
        public Nullable<System.DateTime> last_password_change { get; set; }

        [DisplayName("Erstelldatum")]
        public System.DateTime create_date { get; set; }
        
        [DisplayName("Accountaktivierungsstatus")]
        public bool account_active { get; set; }

        [DisplayName("Anmeldeversuche")]
        public Nullable<int> login_attempts { get; set; }

        [DisplayName("Letzter Anmeldevorgang")]
        public Nullable<System.DateTime> last_login { get; set; }
    
        public virtual role role { get; set; }
    }
}
