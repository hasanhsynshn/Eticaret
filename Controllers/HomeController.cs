using ETicaret2020.WebUI.AppClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaret2020.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Sepet()
        {
            return PartialView();
        }
        public PartialViewResult Slider()
        {
            var data = Context.Baglanti.Resim.Where(x => x.BuyukYol.Contains("Slider")).ToList();
            return PartialView(data);
        }

        public PartialViewResult YeniUrunler()
        {

            try
            {
                var sonuc = Context.Baglanti.Urun.ToList();
                return PartialView(sonuc);
            }
            catch (Exception)
            {

                throw;
            }


        }
        public PartialViewResult Servisler()
        {
            return PartialView();
        }

        public PartialViewResult Markalar()
        {
            var data = Context.Baglanti.Marka.ToList();
            return PartialView(data);
        }
        public void SepeteEkle(int id)
        {
            SepetItem si = new SepetItem();
            Urun u = Context.Baglanti.Urun.FirstOrDefault(x => x.Id == id);

            si.Urun = u;
            si.Adet = 1;
            si.Indirim = 0;
            Sepet s = new Sepet();
            s.SepeteEkle(si);
           
        }
        public void SepetUrunAdetDusur(int id)
        {
            if (HttpContext.Session["AktifSepet"]!=null)
            {
                Sepet s = (Sepet)HttpContext.Session["AktifSepet"];
                if (s.Urunler.FirstOrDefault(x=>x.Urun.Id==id).Adet>1)
                {
                    s.Urunler.FirstOrDefault(x => x.Urun.Id == id).Adet--;
                }
                else
                {
                    SepetItem si = s.Urunler.FirstOrDefault(x => x.Urun.Id == id);
                    s.Urunler.Remove(si);
                }
            }
        }
        public PartialViewResult MiniSepetWidget()
        {
            if (HttpContext.Session["AktifSepet"] != null)
            {
                return  PartialView((Sepet)HttpContext.Session["AktifSepet"]);
            }
            else
            {
                return PartialView();
            }
        }
        public ActionResult UrunDetay(string id)
        {
            Urun u = Context.Baglanti.Urun.FirstOrDefault(x=>x.Adi==id);
            List<UrunOzellik> uos = Context.Baglanti.UrunOzellik.Where(x => x.UrunID == u.Id).ToList();
            List<OzellikTip> tips = new List<OzellikTip>();
            List<OzellikDeger> degers = new List<OzellikDeger>();
            foreach (UrunOzellik uo in uos)
            {
                OzellikTip ot = Context.Baglanti.OzellikTip.FirstOrDefault(x => x.Id == uo.OzellikTipID);
                tips.Add(ot);
                OzellikDeger od = Context.Baglanti.OzellikDeger.FirstOrDefault(x => x.OzellikTipID == ot.Id && x.Id == uo.OzellikDegerID);
                degers.Add(od);
            }
            ViewBag.OzellikTipler = tips;
            ViewBag.OzellikDegeler = degers;
            return View(u);
        }
    }
}