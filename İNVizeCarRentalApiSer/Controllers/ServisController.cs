using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using İNVizeCarRentalApiSer.Controllers;
using İNVizeCarRentalApiSer.ViewModel;
using İNVizeCarRentalApiSer.Models;
using System.Data.Entity;

namespace İNVizeCarRentalApiSer.Controllers
{
    public class ServisController : ApiController
    {
        DBEntities1 db = new DBEntities1();
        SonucModel sonuc = new SonucModel();


        #region Kategori
        [HttpGet]
        [Route("api/kategoriliste")]
        public List<KategoriModel> KategoriListe()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel()
            {
                KategoriId = x.KategoriId,
                KategoriAdi = x.KategoriAdi,
                KategoriArabaSay = x.Araba.Count()
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/kategoribyid/{kategoriId}")]
        public KategoriModel KategoriById(string KategoriId)
        {
            KategoriModel kayit = db.Kategori.Where(s => s.KategoriId == KategoriId).Select(x =>
            new KategoriModel()
            {
                KategoriId = x.KategoriId,
                KategoriAdi = x.KategoriAdi,
                KategoriArabaSay = x.Araba.Count()
            }).FirstOrDefault();
            return kayit;
        }
        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel KategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Count(s => s.KategoriAdi == model.KategoriAdi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kategori Adı Kayıtlıdır!";
                return sonuc;
            }
            Kategori yeni = new Kategori();
            yeni.KategoriId = Guid.NewGuid().ToString();
            yeni.KategoriAdi = model.KategoriAdi;
            db.Kategori.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/kategoriduzenle")]
        public SonucModel KategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s
            .KategoriId == model.KategoriId)
            .FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.KategoriAdi = model.KategoriAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi";
            return sonuc;
        }


        [HttpDelete]
        [Route("api/kategorisil/{KategoriId}")]
        public SonucModel KategoriSil(string KategoriId)
        {
            Kategori kayit = db.Kategori.Where(s => s.KategoriId == KategoriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            if (db.Araba.Count(s => s.ArabaKatId == KategoriId) > 0) //kategori üzerinde araba vareken kategori silinmaz

            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Ürün Kaydı Olan Kategori Silinemez!";
                return sonuc;
            }
            db.Kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";
            return sonuc;
        }
        #endregion




        #region Araba
        [HttpGet]
        [Route("api/arabaliste")]
        public List<CarModel> ArabaListe()
        {
            List<CarModel> liste = db
            .Araba.Select(x => new
            CarModel()
            {
                ArabaId = x.ArabaId,
                ArabaMarkasi = x.ArabaMarkasi,
                ArabaNo = x.ArabaNo,
                ArabaUcreti = x.ArabaUcreti,
                ArabaKatAdi = x.Kategori.KategoriAdi,
                ArabaKatId = x.ArabaKatId,
                ArabaDurum = true
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/arababyid/{ArabaId}")]
        public CarModel ArabaById(string ArabaId)
        {
            CarModel kayit = db.Araba.Where(s => s.ArabaId == ArabaId
            ).Select(x => new
            CarModel()
            {
                ArabaId = x.ArabaId,
                ArabaMarkasi = x.ArabaMarkasi,
                ArabaNo = x.ArabaNo,
                ArabaUcreti = x.ArabaUcreti,
                ArabaKatAdi = x.Kategori.KategoriAdi,
                ArabaKatId= x.ArabaKatId,
                ArabaDurum = true
            })
            .FirstOrDefault();
            return kayit;
        }
        [HttpGet]
        [Route("api/arabalistebykatid/{KategoriId}")]
        public List<CarModel> ArabaListeByKatId(string KategoriId)
        {
            List<CarModel> liste = db.Araba.Where(s => s.ArabaKatId == KategoriId).Select(x =>
            new CarModel()
            {
                ArabaId = x.ArabaId,
                ArabaMarkasi = x.ArabaMarkasi,
                ArabaNo = x.ArabaNo,
                ArabaUcreti = x.ArabaUcreti,
                ArabaKatAdi = x.Kategori.KategoriAdi,
                ArabaKatId =x.ArabaKatId,
                 ArabaDurum = true
            }).ToList();
            return liste;
        }
        [HttpPost]
        [Route("api/arabaekle")]
        public SonucModel ArabaEkle(CarModel model)
        {
            if (db.Araba.Count(s => s.ArabaNo == model.ArabaNo) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girelen Araba Numarası Kayıtlıdır";
                return sonuc;
            }
            Araba yeni = new Araba();
            yeni.ArabaId = Guid.NewGuid().ToString();
            yeni.ArabaMarkasi = model.ArabaMarkasi;
            yeni.ArabaUcreti = model.ArabaUcreti;
            yeni.ArabaNo = model.ArabaNo;
            yeni.ArabaKatId = model.ArabaKatId;
            yeni.ArabaDurum = true;
            db.Araba.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Araba Eklendi ";
            return sonuc;



           
        }
        [HttpPut]
        [Route("api/arabaduzenle")]
        public SonucModel ArabaDuzenle(CarModel model)
        {
            Araba kayit = db.Araba.Where(s => s.ArabaId ==
            model.ArabaId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı";
                return sonuc;
            }
            kayit.ArabaNo = model.ArabaNo;
            kayit.ArabaMarkasi = model.ArabaMarkasi;
            kayit.ArabaUcreti = model.ArabaUcreti;
            
            db.SaveChanges();
            sonuc.mesaj = "Arabanın bilgileri Duzenlendi";
            sonuc.islem = true;
            return sonuc;
        }
        [HttpDelete]
        [Route("api/arabasil/{ArabaId}")]
        public SonucModel ArabaSil(string ArabaId)
        {
            Araba kayit = db.Araba.Where(s => s.ArabaId == ArabaId).
            SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "KayıtBulunamadı!";
                return sonuc;
            }
            if (db.Kiralama.Count(s => s.KiralamaArabaId == ArabaId) > 0)  //Araba kirlama oldukca araba silenmez
            {
                sonuc.islem = false;
                sonuc.mesaj = "Araba Kiralandı işlem yapılmaz!";
                return sonuc;
            }
            db.Araba.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Araba Silindi";
            return sonuc;
        }
        #endregion


        #region User
        [HttpGet]
        [Route("api/userliste")]
        public List<UserModel> UserListe()
        {
            List<UserModel> liste = db.User.Select(x => new UserModel()
            {
                UserId = x.UserId,
                UserAdiSoy = x.UserAdiSoy,
                UserMail = x.UserMail,
                UserTel = x.UserTel
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/userbyid/{UserId}")]
        public UserModel UserById(string UserId)
        {
            UserModel kayit = db.User.Where(s => s.UserId == UserId).Select(x => new
            UserModel()
            {
                UserId = x.UserId,
                UserAdiSoy = x.UserAdiSoy,
                UserMail = x.UserMail,
                UserTel = x.UserTel
            }).FirstOrDefault();
            return kayit;
        }
        [HttpPost]
        [Route("api/userekle")]
        public SonucModel UserEkle(UserModel model)
        {
            if (db.User.Count(s => s.UserTel == model.UserTel) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = " Girelen User Telefon numarası kayıtlıdır";
                return sonuc;
            }
            User yeni = new User();
            yeni.UserId = Guid.NewGuid().ToString();
            yeni.UserAdiSoy = model.UserAdiSoy;
            yeni.UserMail = model.UserMail;
            yeni.UserTel = model.UserTel;
            db.User.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İşlem Başarlı ";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/usersil/{UserId}")]
        public SonucModel UserSil(string UserId)
        {
            User kayit = db.User.Where(s => s.UserId == UserId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunmadı";
                return sonuc;
            }
            if (db.Kiralama.Count(s => s.KiralamaUserId == UserId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "User üzerind Araba kayıt deavm ettiği için işlem yaplımaz";
                return sonuc;
            }
            db.User.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İşlem Başarlı";
            return sonuc;
        }




        #endregion

        #region Kiralama
        //aratablo olduğu için özel
        [HttpGet]
        [Route("api/userarabaliste/{UserId}")]

        // kullancının Id'na bağlı kiralama kayıtları getir
        public List<KiralamaModel> UserArabaListe(string UserId)
        {
            List<KiralamaModel> liste = db.Kiralama.Where(s => s.
            KiralamaUserId == UserId).Select(x
            => new KiralamaModel()
            {
                KiralamaId = x.KiralamaId,
                KiralamaArabaId = x.KiralamaArabaId,
                KiralamaUserId = x.KiralamaUserId,

            }).ToList();
            foreach (var kayit in liste)
            {
                kayit.UserBilgi = UserById(kayit.KiralamaUserId);
                kayit.ArabaBilgi = ArabaById(kayit.KiralamaArabaId);
            }
            return liste;
        }

        [HttpGet]
        [Route("api/arabauserliste/{ArabaId}")]

        // Arabanın Id'na bağlı kiralama kayıtları getir
        public List<KiralamaModel> ArabaUserListe(string ArabaId)
        {
            List<KiralamaModel> liste = db.Kiralama.Where(s => s.
            KiralamaArabaId == ArabaId).Select(x
            => new KiralamaModel()
            {
                KiralamaId = x.KiralamaId,
                KiralamaUserId = x.KiralamaUserId,
                KiralamaArabaId = x.KiralamaArabaId,
                KiralamaTarih = x.KiralamaTarih,
                TeslimTarih = x.TeslimTarih
            }).ToList();
            foreach (var kayit in liste)
            {
                kayit.UserBilgi = UserById(kayit.KiralamaUserId);
                kayit.ArabaBilgi = ArabaById(kayit.KiralamaArabaId);
            }
            return liste;
        }


        [HttpPost]
        [Route("api/arabakirala")]
        public SonucModel ArabaKiralama(KiralamaModel model)
        {
            SonucModel sonuc = new SonucModel();
            Araba araba = db.Araba.Find(model.KiralamaArabaId);

            if (araba == null) //Araba bilgileri yanlışsa 
            {
                sonuc.islem = false;
                sonuc.mesaj = "Araba bulunamadı!";
                return sonuc;
            }
            if (araba.ArabaDurum == false) //Araba zaten kiralanmışsa 
            {
                sonuc.islem = false;
                sonuc.mesaj = "Araba zaten kiralandı!";
                return sonuc;
            }

            if (db.Kiralama.Count(s => s.KiralamaArabaId == model.KiralamaArabaId && 
            s.KiralamaUserId == model.KiralamaUserId) > 0)  //Araba Kiralaması ıd varsa kiralamasi işlem yapılamz

            {
                sonuc.islem = false;
                sonuc.mesaj = "Araba zaten kiralandı!";
                return sonuc;
            }
            

            if (model.TeslimTarih <= model.KiralamaTarih)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kiralama tarihi teslim tarihinden önce olmalıdır!";
                return sonuc;
            }

            Kiralama yeni = new Kiralama();
            yeni.KiralamaId = Guid.NewGuid().ToString();
            yeni.KiralamaArabaId = model.KiralamaArabaId;
            yeni.KiralamaUserId = model.KiralamaUserId;
            yeni.KiralamaTarih = model.KiralamaTarih;
            yeni.TeslimTarih = model.TeslimTarih;
            araba.ArabaDurum = false;
            db.Kiralama.Add(yeni);
            db.Entry(araba).State = EntityState.Modified;

           

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Araba kiralandı!";
            return sonuc;
        }

       [HttpPost]
[Route("api/arabateslim")]
public SonucModel ArabaTeslim(KiralamaModel model)
{
    SonucModel sonuc = new SonucModel();
           Kiralama kiralama = db.Kiralama.Where(k => k.KiralamaId == model.KiralamaId).FirstOrDefault();

if (kiralama == null)
{
    sonuc.islem = false;
    sonuc.mesaj = "Kiralama kaydı bulunamadı!";
    return sonuc;
}


            Araba araba = db.Araba.Find(kiralama.KiralamaArabaId);
    if (araba == null)
    {
        sonuc.islem = false;
        sonuc.mesaj = "Araba kaydı bulunamadı!";
        return sonuc;
    }

    if (kiralama.TeslimTarih > DateTime.Now)
    {
        sonuc.islem = false;
        sonuc.mesaj = "Teslim tarihi henüz gelmemiş!";
        return sonuc;
    }

            kiralama.TeslimTarih = model.TeslimTarih;

            araba.ArabaDurum = true; // Arabanın teslim edildiğinde durumunu değiştiriyoruz.
            db.Entry(araba).State = EntityState.Modified;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Araba teslim edildi!";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kayitsil/{KiralamaId}")]

        public SonucModel KayitSil(string KiralamaId)
        {

            Kiralama kiralama = db.Kiralama.Where(s => s.KiralamaId == KiralamaId).
                SingleOrDefault();
            if (kiralama == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "kayıt bulunmadı !";

            }

            db.Kiralama.Remove(kiralama);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Araba Kaydı Silindi ";
            return sonuc;

        }
         
        

        #endregion


    }
}
