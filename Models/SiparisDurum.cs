namespace ETicaret2020.WebUI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SiparisDurum")]
    public partial class SiparisDurum
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SiparisDurum()
        {
            Satis = new HashSet<Satis>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Adi { get; set; }

        public string Aciklama { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Satis> Satis { get; set; }
    }
}
