using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsWeb.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace NewsWeb.Controllers
{
    public class AdminController : Controller
    {
        QLTintucDataContext data = new QLTintucDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Tintuc(int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(data.TINTUCs.ToList().OrderBy(n => n.Matintuc).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var username = collection["username"];
            var password = collection["password"];
            if (String.IsNullOrEmpty(username))
            {
                ViewData["Error_username"] = "Please enter your username.";
            }
            else if (String.IsNullOrEmpty(password))
            {
                ViewData["Error_password"] = "Please enter your password";
            }
            else
            {
                Admin admin = data.Admins.SingleOrDefault(n => n.UserAdmin == username && n.PassAdmin == password);
                if (admin != null)
                {
                    Session["Taikhoanadmin"] = admin;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(data.CHUDETINTUCs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TINTUC tintuc, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDETINTUCs.ToList().GroupBy(n => n.TenChuDe), "MaCD", "TenChude");
            if(fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/News_images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình đã tồn tại!";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    tintuc.Anhbia = fileName;
                    data.TINTUCs.InsertOnSubmit(tintuc);
                    data.SubmitChanges();                    
                }
                return RedirectToAction("Tintuc");
            }
        }

        public ActionResult detailsTintuc(int id)
        {
            TINTUC tintuc = data.TINTUCs.SingleOrDefault(n => n.Matintuc == id);
            ViewBag.Matintuc = tintuc.Matintuc;
            if(tintuc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tintuc);
        }

        public ActionResult deleteTintuc(int id)
        {
            TINTUC tintuc = data.TINTUCs.SingleOrDefault(n => n.Matintuc == id);
            ViewBag.Matintuc = tintuc.Matintuc;
            if(tintuc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tintuc);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult confirmDelete(int id)
        {
            TINTUC tintuc = data.TINTUCs.SingleOrDefault(n => n.Matintuc == id);
            ViewBag.Matintuc = tintuc.Matintuc;
            if(tintuc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.TINTUCs.DeleteOnSubmit(tintuc);
            data.SubmitChanges();
            return RedirectToAction("Tintuc");
        }

        [HttpGet]
        public ActionResult editTintuc(int id)
        {
            TINTUC tintuc = data.TINTUCs.SingleOrDefault(n => n.Matintuc == id);
            ViewBag.Matintuc = tintuc.Matintuc;
            if (tintuc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.Matintuc = new SelectList(data.CHUDETINTUCs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude", tintuc.MaCD);
            return View(tintuc);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editTintuc(TINTUC tintuc, HttpPostedFileBase fileUpload)
        {
            ViewBag.Matintuc = new SelectList(data.CHUDETINTUCs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/News_images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình đã tồn tại!";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    tintuc.Anhbia = fileName;
                    data.TINTUCs.InsertOnSubmit(tintuc);
                    data.SubmitChanges();
                }
                return RedirectToAction("Tintuc");
            }
        }
    }
}