using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace İNVizeCarRentalApiSer.ViewModel
{
    public class CarModel
    {
        public string ArabaId { get; set; }
        public string ArabaMarkasi { get; set; }
        public string ArabaUcreti { get; set; }
        public string ArabaNo { get; set; }
       
        public string ArabaKatId { get; set; }
        public string ArabaKatAdi { get; set; }
        public bool ArabaDurum { get; set; }

    }
}