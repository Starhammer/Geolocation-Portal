using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Geolocation_Portal_Test.Models.API
{
    public class record
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public record()
        {
        }

        public int Id { get; set; }
        [DisplayName("Upload Datum")]
        public System.DateTime dataset_upload { get; set; }
        [DisplayName("Änderungsdatum")]
        public System.DateTime dataset_modified_date { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie einen Titel an.")]
        [StringLength(160)]
        [DisplayName("Titel")]
        public string title { get; set; }

        [StringLength(3000)]
        [DisplayName("Beschreibung")]
        public string description { get; set; }

        [DisplayName("Kategorie")]
        public Nullable<int> category_id { get; set; }

        [DisplayName("Lizenz")]
        public Nullable<int> licence_id { get; set; }

        [DisplayName("Datenbereitsteller")]
        public Nullable<int> publisher_id { get; set; }

        [DisplayName("Bewertung")]
        public int rating { get; set; }
        public bool dia_data { get; set; }
        public bool geo_data { get; set; }

        [DisplayName("Datensatzsichtbarkeit")]
        public Nullable<int> role_id { get; set; }

        [DisplayName("Datensatzveröffentlichungsstatus")]
        public bool record_active { get; set; }

        [DisplayName("Örtlichkeit")]
        public int location_id { get; set; }

        public virtual category category { get; set; }
        public virtual publisher publisher { get; set; }
        public virtual licence licence { get; set; }
        public virtual location location { get; set; }
    }
}
