using Auction_BD.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction_BD.Controllers
{
    public class UserController : Controller
    {

        Auction_BDEntities db = new Auction_BDEntities();
        // GET: User
        public ActionResult Index(int? page)
        {
            
            DateTime date = DateTime.Now.AddDays(-2);

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.products.Where(x => x.aprv == 1 && (date< x.date_of_post)).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<product> stu = list.ToPagedList(pageindex, pagesize);

           


            return View(stu);
        }

        [HttpGet]
        public ActionResult cetagory(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.cetagories.Where(x => x.sts == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<cetagory> stu = list.ToPagedList(pageindex, pagesize);



            return View(stu);
        }



        [HttpGet]
        public ActionResult login()
        {
            if(Session["u_id"]==null)
            {
                return View();
            }

            return RedirectToAction("Index");


        }



        [HttpPost]
        public ActionResult login(user avm)
        {

            user ad = db.users.Where(x => x.u_email == avm.u_email && x.u_password == avm.u_password).SingleOrDefault();

            if (ad != null)
            {

                Session["u_id"] = ad.u_id.ToString();
                Session["u_name"] = ad.u_name.ToString();
                return RedirectToAction("index");

            }
            else
            {
                ViewBag.error = "Invalid Username or password";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Signout()
        {
            Session["u_id"] = null;
            return RedirectToAction("Index");
        }




        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(user uvm, HttpPostedFileBase imgfile)
        {
            string path = uploadimagefile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could be uploaded";
            }
            else
            {
                user u = new user();
                u.u_name = uvm.u_name;
                u.u_email = uvm.u_email;
                u.u_password = uvm.u_password;
                u.u_phone = uvm.u_phone;
                u.u_image = path;
                db.users.Add(u);
                db.SaveChanges();
                return RedirectToAction("login");
            }

            return View();
        }

        [HttpGet]
        public ActionResult createAd()
        {
            List<cetagory> li = db.cetagories.ToList();
            ViewBag.cetagories = new SelectList(li, "cat_id", "cat_name");

            return View();
        }

        [HttpPost]

        public ActionResult createAd(product pvm, HttpPostedFileBase imgfile)
        {
            List<cetagory> li = db.cetagories.ToList();
            ViewBag.cetagories = new SelectList(li, "cat_id", "cat_name");

            string path = uploadimagefile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could be uploaded";
            }
            else
            {
                DateTime localDate = DateTime.Now;
                product p = new product();

                p.pro_name = pvm.pro_name;
                p.pro_price = pvm.pro_price;
                p.pro_iamge = path;
                p.pro_fk_cat = pvm.pro_fk_cat;
                p.pro_des = pvm.pro_des;
                p.pro_fk_user = Convert.ToInt32(Session["u_id"].ToString());
                p.bid_price = 0;
                p.aprv = 0;
                p.u_id = null;
                p.date_of_post = localDate;
                db.products.Add(p);
                db.SaveChanges();
                Response.Redirect("index");
            }

            return View();
        }




        public ActionResult ViewAd(int? id)
        {
            viewadmodel ad = new viewadmodel();
            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();

            user b = db.users.Where(x => x.u_id == p.u_id).SingleOrDefault();

            ad.pro_id = p.pro_id;
            ad.pro_name = p.pro_name;
            ad.pro_iamge = p.pro_iamge;
            ad.pro_price = p.pro_price;
            ad.bid_price = p.bid_price;
            ad.pro_des = p.pro_des;
            ad.u_id = p.u_id;
            ad.pro_fk_user = p.pro_fk_user;
            if(b!=null)
            {
                ad.bider_name = b.u_name;
            }
            
            

            cetagory cat = db.cetagories.Where(x => x.cat_id == p.pro_fk_cat).SingleOrDefault();

            ad.cat_name = cat.cat_name;
            ad.cat_id = cat.cat_id;


            user u = db.users.Where(x => x.u_id == p.pro_fk_user).SingleOrDefault();

            ad.appv = p.aprv;
            ad.u_name = u.u_name;
            ad.u_phone = u.u_phone;
            ad.u_email = u.u_email;
            ad.u_image = u.u_image;
            
            ad.time = (TimeSpan)( p.date_of_post- DateTime.Now.AddDays(-2));

            



            return View(ad);
        }

        [HttpPost]
        public ActionResult bid(viewadmodel ad)
        {
            if (Session["u_id"] == null)
            {
                return RedirectToAction("login");
            }
            viewadmodel v = new viewadmodel();

            v.bid_price = ad.bid_price;
            v.pro_id = ad.pro_id;
            string str = (string)Session["u_id"];

            v.u_id = Int16.Parse(str);

            product p = db.products.Where(x => x.pro_id == ad.pro_id).SingleOrDefault();

            if (ad.bid_price < p.bid_price || ad.bid_price< p.pro_price)
            {
                ViewBag.error = "Bid with higher price than the last price";
                TempData["ErrorMessage"] = "Bid with higher price than the last price";

                return RedirectToAction("ViewAd", "User", new { @id = ad.pro_id });
            }
            else
            {
                p.u_id = Int16.Parse(str);
                p.bid_price = ad.bid_price;
                // db.products.Add(p);
                db.SaveChanges();

                //  Response.Redirect("ViewAd("+ad.pro_id+")");
                return RedirectToAction("ViewAd", "User", new { @id = ad.pro_id });
            }

            return Content("<script language='javascript' type='text/javascript'>alert('Thanks for Feedback!');</script>");


        }

        [HttpGet]
        public ActionResult viewprofile(int id)
        {


            //string str = (string)id;
            string str = (string)Session["u_id"];
            DateTime date = DateTime.Now.AddDays(-2);

            int us = Convert.ToInt32(Session["u_id"]);

            Profileviewmodel ad = new Profileviewmodel();

            user u = db.users.Where(x => x.u_id == us).SingleOrDefault();

            var p = db.products.Where(x => x.pro_fk_user == us && x.aprv==1).OrderByDescending(x => x.pro_id).ToList();

            var pen = db.products.Where(x => x.pro_fk_user == us && x.aprv == 0).OrderByDescending(x => x.pro_id).ToList();

            var w = db.products.Where(x => x.u_id == us && (date > x.date_of_post)).OrderByDescending(x => x.pro_id).ToList();

            var sold= db.products.Where(x => x.pro_fk_user == us && (date > x.date_of_post) && x.bid_price>x.pro_price).OrderByDescending(x => x.pro_id).ToList();
            ad.u_name = u.u_name;
            ad.u_image = u.u_image;
            ad.u_phone = u.u_phone;
            ad.u_email = u.u_email;
            ad.pro_list = p;
            ad.won_list = w;
            ad.pen_list = pen;
            ad.Sold_list = sold;
            

            return View(ad);
        }

        public ActionResult Viewotherprofile(int id)
        {
            Profileviewmodel ad = new Profileviewmodel();

            user u = db.users.Where(x => x.u_id == id).SingleOrDefault();

            var p = db.products.Where(x => x.pro_fk_user == id && x.aprv==1).OrderByDescending(x => x.pro_id).ToList();

           
            ad.u_name = u.u_name;
            ad.u_image = u.u_image;
            ad.u_phone = u.u_phone;
            ad.u_email = u.u_email;
            ad.pro_list = p;

            return View(ad);



        }
        public ActionResult ViewAdbycetagory(int? id,int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.products.Where(x => x.aprv == 1 && x.pro_fk_cat==id).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<product> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
           

            
        }

        public ActionResult ViewAdbysearch(string search, int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.products.Where(x => x.aprv == 1 && x.pro_name.Contains(search)).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<product> stu = list.ToPagedList(pageindex, pagesize);

            return View(stu);



        }

        







        public string uploadimagefile(HttpPostedFileBase file)
        {
            Random r = new Random();
            int random = r.Next();
            string path = "-1";
            if (file != null && file.ContentLength > 0)
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
                else
                {
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






    }
}