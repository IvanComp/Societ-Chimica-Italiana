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
    
    public partial class Posta_Messaggi
    {
        public int Codice { get; set; }
        public bool FlagInvio { get; set; }
        public bool FlagRicevuta { get; set; }
        public bool FlagSpedito { get; set; }
        public bool FlagDettaglio { get; set; }
        public bool FlagNotificaServer { get; set; }
        public string Note { get; set; }
        public string Messaggio { get; set; }
        public int CodP { get; set; }
        public short AnnoP { get; set; }
        public string Tiscr { get; set; }
        public string Cat { get; set; }
        public Nullable<decimal> Pagato { get; set; }
        public Nullable<decimal> Dovuto { get; set; }
        public Nullable<System.DateTime> DatPag { get; set; }
        public Nullable<System.DateTime> DatReg { get; set; }
        public Nullable<System.DateTime> DatCassa { get; set; }
        public string Tpag { get; set; }
        public string Fuori { get; set; }
        public Nullable<bool> Lode { get; set; }
        public string Carica { get; set; }
        public string Sez { get; set; }
        public string Set { get; set; }
        public string Pos { get; set; }
        public Nullable<int> Ent { get; set; }
        public string CodSocCol { get; set; }
        public Nullable<short> AnnoCat { get; set; }
        public string CodCat { get; set; }
        public string NCodCat { get; set; }
        public string DescrCat { get; set; }
        public Nullable<decimal> ImportoCat { get; set; }
        public Nullable<bool> SelCat { get; set; }
        public string CodTIscr { get; set; }
        public string DescrTIscr { get; set; }
        public Nullable<bool> SelTIscr { get; set; }
        public string CodTPag { get; set; }
        public string DescrTPag { get; set; }
        public Nullable<bool> SelTPag { get; set; }
        public string CodSez { get; set; }
        public string NCodSez { get; set; }
        public string DescrSez { get; set; }
        public Nullable<bool> SelSez { get; set; }
        public string CodCarica { get; set; }
        public string DescrCarica { get; set; }
        public Nullable<bool> SelCarica { get; set; }
        public string CodEnt { get; set; }
        public string NCodEnt { get; set; }
        public string DescrEnt { get; set; }
        public Nullable<bool> SelEnt { get; set; }
        public string CodPos { get; set; }
        public string NCodPos { get; set; }
        public string DescrPos { get; set; }
        public Nullable<bool> SelPos { get; set; }
        public string CodSet { get; set; }
        public string NCodSet { get; set; }
        public string DescrSet { get; set; }
        public Nullable<bool> SelSet { get; set; }
        public Nullable<short> AnnoV { get; set; }
        public string FormatoV { get; set; }
        public Nullable<byte> DecimaliV { get; set; }
        public string Simbolo { get; set; }
        public Nullable<int> Cod { get; set; }
        public string Sesso { get; set; }
        public string Nom { get; set; }
        public string Ist { get; set; }
        public string Via { get; set; }
        public string CAP { get; set; }
        public string Cit { get; set; }
        public string Naz { get; set; }
        public Nullable<System.DateTime> DaNa { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string E_Mail { get; set; }
        public Nullable<short> DimAn { get; set; }
        public Nullable<bool> NoStampa { get; set; }
        public Nullable<System.DateTime> Modificato { get; set; }
        public Nullable<int> PresentatoDa { get; set; }
        public Nullable<bool> FlagRegalo { get; set; }
        public Nullable<int> PresentatoDa2 { get; set; }
        public Nullable<bool> FlagRegalo2 { get; set; }
        public Nullable<short> AnnoPresentazione { get; set; }
        public string CodTitStu { get; set; }
        public string CodSex { get; set; }
        public string DescrSex { get; set; }
        public Nullable<bool> SelSex { get; set; }
        public string CodNaz { get; set; }
        public string DescrNaz { get; set; }
        public Nullable<bool> SelNaz { get; set; }
        public string SocioPresentatore1 { get; set; }
        public string SocioPresentatore2 { get; set; }
        public Nullable<double> Eta { get; set; }
        public string Fascia { get; set; }
        public Nullable<int> VeraEta { get; set; }
        public string CodDiv { get; set; }
        public Nullable<bool> Effettiva { get; set; }
        public Nullable<short> NCodDiv { get; set; }
        public string DescrDiv { get; set; }
        public Nullable<decimal> ImportoDiv { get; set; }
        public Nullable<bool> SelDiv { get; set; }
        public Nullable<short> AnnoDiv { get; set; }
        public string CodGru { get; set; }
        public Nullable<short> AnnoGru { get; set; }
        public string DescrGruppo { get; set; }
        public Nullable<decimal> ImportoGru { get; set; }
        public Nullable<bool> SelGru { get; set; }
        public string CodRiv { get; set; }
        public Nullable<byte> NCopie { get; set; }
        public Nullable<short> AnnoRiv { get; set; }
    
        public virtual Posta_Prototipi Posta_Prototipi { get; set; }
    }
}
