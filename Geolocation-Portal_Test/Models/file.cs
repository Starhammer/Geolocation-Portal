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
    
    public partial class file
    {
        public int Id { get; set; }
        public int record_id { get; set; }
        public System.DateTime file_upload_date { get; set; }
        public Nullable<int> download_count { get; set; }
        public string file_icon { get; set; }
        public Nullable<bool> diagram_data { get; set; }
        public Nullable<bool> map_data { get; set; }
        public Nullable<double> file_size { get; set; }
    
        public virtual record record { get; set; }
        public virtual searchtag searchtag { get; set; }
    }
}
