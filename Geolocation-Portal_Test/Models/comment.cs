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
    
    public partial class comment
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string person_name { get; set; }
        public int bewertung { get; set; }
        public int record_id { get; set; }
    
        public virtual record record { get; set; }
    }
}
