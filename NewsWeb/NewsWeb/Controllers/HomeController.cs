using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsWeb.Models;
using PagedList;

namespace NewsWeb.Controllers
{
    public class HomeController : Controller
    {
        QLTintucDataContext data = new QLTintucDataContext();

        private List<TINTUC> getNews(int count)
        {
            return data.TINTUCs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        
        public ActionResult Index(int ? page)
        {
            int pageSizes = 6;
            int pageNum = (page ?? 1);

            var news = getNews(6);
            return View(news.ToPagedList(pageNum, pageSizes));
        }

        public ActionResult Chudetintuc()
        {
            var chudetintuc = from cd in data.CHUDETINTUCs select cd;
            return PartialView(chudetintuc);
        }

        public ActionResult TinTheochude(int id, int? page)
        {
            int pageSizes = 6;
            int pageNum = (page ?? 1);

            var news = from n in data.TINTUCs where n.MaCD == id select n;
            return View(news.ToPagedList(pageNum, pageSizes));
        }

        public ActionResult Details(int id)
        {
            var tintuc = from t in data.TINTUCs where t.Matintuc == id select t;
            return View(tintuc.Single());
        }
    }
}