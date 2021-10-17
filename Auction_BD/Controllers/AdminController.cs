using Auction_BD.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Auction_BD.Controllers
{
    public class AdminController : Controller
    {
        Auction_BDEntities db = new Auction_BDEntities();
        // GET: Admin


        [HttpGet]
        public ActionResult index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(admin avm)
        {
           
            admin ad = db.admins.Where(x => x.ad_username == avm.ad_username && x.ad_password == avm.ad_password).SingleOrDefault();

            if (ad!=null)
            {
                
                Session["ad_id"] = ad.ad_id.ToString();
                return RedirectToAction("index");

            }
            else
            {
                ViewBag.error = "Invalid Username or password";
            }

            return View();
        }
        public ActionResult Create()
        {
            if(Session["ad_id"]==null)
            {
                return RedirectToAction("login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(cetagory cvm,HttpPostedFileBase imgfile)
        {
            string path = uploadimagefile(imgfile);
            if(path.Equals("-1"))
            {
                ViewBag.error = "Image could be uploaded";
            }
            else
            {
                cetagory cat = new cetagory();
                cat.cat_name = cvm.cat_name;
                cat.cat_iamge = path;
                cat.cat_fk_ad = Convert.ToInt32(Session["ad_id"].ToString());
                cat.sts = 1;
                db.cetagories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("ViewCategory");
            }
            return View();
        }

        
        public ActionResult approve(int? page)
        {     
            if (Session["ad_id"] == null)
            {
               // return RedirectToAction("login");
            }
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.products.Where(x => x.aprv == 0).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<product> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        public ActionResult Delete(int ?id)
        {
            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();

            db.products.Remove(p);
            db.SaveChanges();

            return RedirectToAction("approve");

        }

        public ActionResult Deletes(int? id)
        {
            cetagory p = db.cetagories.Where(x => x.cat_id == id).SingleOrDefault();

            db.cetagories.Remove(p);
            db.SaveChanges();

            return RedirectToAction("ViewCategory");

        }

        public ActionResult appv(int? id)
        {
            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();

            p.aprv = 1;

            db.SaveChanges();

            return RedirectToAction("approve");

        }


        public string uploadimagefile(HttpPostedFileBase file)
        {
            Random r = new Random();
            int random = r.Next();
            string path = "-1";
            if(file!=null && file.ContentLength>0)
            {
                string extention = Path.GetExtension(file.FileName);
                if (extention.ToLower().Equals(".jpg") || extention.ToLower().Equals(".jpeg") || extention.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else {
                    Response.Write("<script>alert('Only jgb,jpeg and png formats are acceptable...'); </script>");
                
                }


            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }

            return path;
        }


        public ActionResult ViewCategory(int?page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.cetagories.Where(x => x.sts == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<cetagory> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);



            
        }

        public ActionResult ViewAd(int? id)
        {
            viewadmodel ad = new viewadmodel();
            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();

            

            ad.pro_id = p.pro_id;
            ad.pro_name = p.pro_name;
            ad.pro_iamge = p.pro_iamge;
            ad.pro_price = p.pro_price;
            ad.bid_price = p.bid_price;
            ad.pro_des = p.pro_des;
            ad.u_id = p.u_id;
            ad.pro_fk_user = p.pro_fk_user;
           


            cetagory cat = db.cetagories.Where(x => x.cat_id == p.pro_fk_cat).SingleOrDefault();

            ad.cat_name = cat.cat_name;
            ad.cat_id = cat.cat_id;


            user u = db.users.Where(x => x.u_id == p.pro_fk_user).SingleOrDefault();

            ad.appv = p.aprv;
            ad.u_name = u.u_name;
            ad.u_phone = u.u_phone;
            ad.u_email = u.u_email;
            ad.u_image = u.u_image;

            ad.time = (TimeSpan)(p.date_of_post - DateTime.Now.AddDays(-2));





            return View(ad);
        }

    }
}