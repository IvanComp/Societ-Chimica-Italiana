//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sci.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Gruppi
    {
        public short AnnoP { get; set; }
        public int CodP { get; set; }
        public string CodGru { get; set; }
    
        public virtual Pag Pag { get; set; }
        public virtual TabGru TabGru { get; set; }
    }
}
