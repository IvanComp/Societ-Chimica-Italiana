using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sci.Models
{
    public class MemsSearchViewModel
    {
        public string Cognome { get; set; }
        public string Email { get; set; }
        public int? Codice { get; set; }
        public string CurrentSort { get; set; }
    }
}