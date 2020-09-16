using ETicaret2020.WebUI.AppClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ETicaret2020.WebUI.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Urunler()
        {
            return View(Context.Baglanti.Urun.ToList());
        }
        public ActionResult UrunEkle()
        {
            ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
            ViewBag.Markalar = Context.Baglanti.Marka.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult UrunEkle(Urun urn)
        {

            urn.EklenmeTarihi = DateTime.Now;
            Context.Baglanti.Urun.Add(urn);
            
            Context.Baglanti.SaveChanges();
            
            return RedirectToAction("Urunler");
        }
        public ActionResult UrunOzellikleri()
        {
            return View(Context.Baglanti.UrunOzellik.ToList());
        }
        public ActionResult Markalar()
        {
            return View(Context.Baglanti.Marka.ToList());
        }
        public ActionResult MarkaEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MarkaEkle(Marka mrk, HttpPostedFileBase fileUpload)
        {
            int resimId = -1;
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                int width = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaWidth"].ToString());
                int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaHeight"].ToString());
                string name = "/Content/MarkaResim/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);

                Bitmap bm = new Bitmap(img, width, height);
                bm.Save(Server.MapPath(name));
                Resim rsm = new Resim();
                rsm.OrtaYol = name;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                if (rsm.Id != null)

                    resimId = rsm.Id;
            }
            if (resimId != -1)

                mrk.ResimID = resimId;
            Context.Baglanti.Marka.Add(mrk);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");


        }


        public ActionResult Kategoriler()
        {
            return View(Context.Baglanti.Kategori.ToList());
        }
       public ActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KategoriEkle(Kategori ktg)
        {
            try
            {
                Context.Baglanti.Kategori.Add(ktg);
                Context.Baglanti.SaveChanges();
                return RedirectToAction("Kategoriler");
            }
            catch (Exception )
            {

                throw;
            }
        }
        public ActionResult OzellikTipleri()
        {

            return View(Context.Baglanti.OzellikTip.ToList());
        }
    public ActionResult OzellikTipEkle()
        {

            return View(Context.Baglanti.Kategori.ToList());
        }
    [HttpPost]
    public ActionResult OzellikTipEkle(OzellikTip ot)
        {
            Context.Baglanti.OzellikTip.Add(ot);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikTipleri");
        }
        public ActionResult OzellikDegerleri()
        {
            return View(Context.Baglanti.OzellikDeger.ToList());
        }
        public ActionResult OzellikDegerEkle()
        {
            return View(Context.Baglanti.OzellikTip.ToList());
        }
        [HttpPost]
        public ActionResult OzellikDegerEkle(OzellikDeger od)
        {
            Context.Baglanti.OzellikDeger.Add(od);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikDegerleri");

        }
        public ActionResult UrunOzellikSil(int urunId,int tipId,int degerId)
        {
            UrunOzellik uo = Context.Baglanti.UrunOzellik.FirstOrDefault(x=>x.UrunID==urunId&&x.OzellikTipID==tipId&&x.OzellikDegerID==degerId);
            Context.Baglanti.UrunOzellik.Remove(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        
        }
        public ActionResult UrunOzellikEkle()
        {

            return View(Context.Baglanti.Urun.ToList());
        }
        [HttpPost]
        public ActionResult UrunOzellikEkle(UrunOzellik uo)
        {
            Context.Baglanti.UrunOzellik.Add(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("UrunOzellikleri");

        }

        public PartialViewResult UrunOzellikTipWidget(int? katId)
        {
            if (katId!=null)
            {

                return PartialView(Context.Baglanti.OzellikTip.Where(x => x.KategoriID == katId).ToList());
            }
            else
            {
                var data = Context.Baglanti.OzellikTip.ToList();
                return PartialView(data);
            }
        }
        public PartialViewResult UrunOzellikDegerWidget(int? tipId)
        {
            if (tipId!=null)
            {
                var data = Context.Baglanti.OzellikDeger.Where(x=>x.OzellikTipID==tipId).ToList();
                return PartialView(data);
            }
            else
            {
                var data = Context.Baglanti.OzellikDeger.ToList();
                return PartialView(data);
            }
        }
        public string UrunOzellikTipListele(object list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='form-group'>< label class='col-lg-12 control-label'>Özellik</label><div class='col-lg-10'><select id = 'urun' class='form-control m-bot15' name='Id'>");
            List<OzellikTip> ot = (List<OzellikTip>)list;
            foreach (OzellikTip tip in ot)
            {
                sb.Append("<option value="+tip.Id+">"+tip.Adi+"</option>");
            }
            sb.Append("</select></ div ></ div > ");
            return sb.ToString();
        }
        public ActionResult UrunResimEkle(int id)
        {
            return View(id);
        }
            [HttpPost]
            public ActionResult UrunResimEkle(int uId, HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                Bitmap ortaResim = new Bitmap(img, Settings.UrunOrtaBoyut);
                Bitmap buyukResim = new Bitmap(img, Settings.UrunBuyukBoyut);
                string ortaYol = "/Content/UrunResim/Orta/" + Guid.NewGuid()
                + Path.GetExtension(fileUpload.FileName);
                string buyukYol = "/Content/UrunResim/Buyuk/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);
                ortaResim.Save(Server.MapPath(ortaYol));
                buyukResim.Save(Server.MapPath(buyukYol));
                Resim rsm = new Resim();
                rsm.BuyukYol = buyukYol;
                rsm.OrtaYol = ortaYol;
                rsm.UrunID = uId;

                if (Context.Baglanti.Resim.FirstOrDefault(x => x.UrunID == uId && x.Varsayılan == false) != null)
                {
                    rsm.Varsayılan = true;
                }
                else
                {
                    rsm.Varsayılan = false;
                }
              Context.Baglanti.Resim.Add(rsm);
              Context.Baglanti.SaveChanges();

                return View(uId);

            }
            return View(uId);
        }
        public ActionResult SliderResimleri()
        {
            return View();
        }
      [HttpPost]
      public ActionResult SliderResimEkle(HttpPostedFileBase fileUpload)
        {
            if (fileUpload!=null)
            {
                Image ımg = Image.FromStream(fileUpload.InputStream);
                Bitmap bmp = new Bitmap(ımg, Settings.SliderResimBoyut);
                string yol = "/Content/SliderResim" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);
                bmp.Save(Server.MapPath(yol));
                Resim rsm = new Resim();
                rsm.BuyukYol = yol;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();

            }
            return RedirectToAction("SliderResimleri");
        }
    }

 
    

}