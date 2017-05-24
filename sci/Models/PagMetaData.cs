using System;
using System.ComponentModel.DataAnnotations;

namespace sci.Models
{
    [MetadataType(typeof(PagMetadata))]
    public partial class Pag
    {
        // can add extra/new code in here
    }

    public class PagMetadata
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}", NullDisplayText = "")]
        public Nullable<decimal> Pagato { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}", NullDisplayText = "")]
        public Nullable<decimal> Dovuto { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DatPag { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DatReg { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DatCassa { get; set; }
    }
}