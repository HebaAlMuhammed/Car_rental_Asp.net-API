using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace İNVizeCarRentalApiSer.ViewModel
{
    public class KiralamaModel
    {
        public string KiralamaId { get; set; }
        public string KiralamaArabaId { get; set; }
        public string KiralamaUserId { get; set; }
      
        public System.DateTime KiralamaTarih { get; set; }
        public System.DateTime TeslimTarih { get; set; }
        public UserModel UserBilgi { get; set; }
        public CarModel ArabaBilgi { get; set; }




    }
}