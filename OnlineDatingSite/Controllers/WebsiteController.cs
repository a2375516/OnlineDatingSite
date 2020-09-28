using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineDatingSite.Models;

namespace OnlineDatingSite.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly IHostingEnvironment he;
        public WebsiteController(IHostingEnvironment e)
        {
            he = e;
        }
        public IActionResult Login(string username, string password)
        {
            //簡易的登入系統
            LoginModel user = new LoginModel();
            if (user.Login(username, password))
            {
                HttpContext.Session.SetString("Online", username);
                return LocalRedirect("~/Website/HomePage");
            }
                
            else
                return View();
        }
        public IActionResult HomePage()
        {
            return View();
        }
        
        public IActionResult PersonalInformation()
        {
            try
            {
                //顯示個人資料
                string username="";
                if (HttpContext.Session.Keys.Contains("Online"))
                {
                    username = HttpContext.Session.GetString("Online");
                }
                SInformationModel sInformationModel = new SInformationModel();
                sInformationModel.Select(username);
                List<Data> DataList= sInformationModel.DataList;
                ViewBag.DataList = DataList;

                return View(model:sInformationModel);
            }
            catch
            {
                return View();
            }
            
        }
        
        public IActionResult AlterInformation(string nickname, string address, string job, string[] interest, string resume,IFormFile pic)
        {
            try
            {
                string username = "";
                if (HttpContext.Session.Keys.Contains("Online"))//判斷是哪位使用者
                {
                    username = HttpContext.Session.GetString("Online");
                }
            
                SInformationModel sInformationModel = new SInformationModel();//連接資料庫
                //顯示資料
                sInformationModel.Select(username);//搜尋個人資料
                sInformationModel.SelectList();//搜尋興趣清單
                List<Data> DataList = sInformationModel.DataList;//宣告一個List存model裡的DataList
                List<string> InterestList = sInformationModel.InterestList;//宣告一個List存model裡的InterestList
                ViewBag.InterestList = InterestList;//用ViewBag存InterestList
                string First = "";//原本的photo路徑
                string FirstInterest="";//原本興趣的項目
                foreach(Data data in DataList)//抓取每一個值
                {
                    ViewData["name"] = data.name;
                    ViewData["nickname"] = data.nickname;
                    ViewData["sex"] = data.sex;
                    ViewData["address"] = data.address;
                    ViewData["birth"] = data.birth;
                    ViewData["email"] = data.email;
                    ViewData["job"] = data.job;
                    ViewData["interest"] = data.interest;
                    FirstInterest = data.interest;
                    ViewData["resume"] = data.resume;
                    ViewData["personalphoto"] = data.personalphoto;
                    First = data.personalphoto;
                }
                //新增資料
                if (nickname == null)
                    nickname = "";
                if (job == null)
                    job = "";
                if (resume == null)
                    resume = "";
                
                string picname = "";//宣告照片路經
                string allinterest= "";//宣告興趣清單
                ViewData["errorinterest"] = "";
                if (interest.Count() <= 5)//判斷興趣有沒有大於五個
                {
                    for(int i = 0; i < interest.Count(); i++)//用FOR迴圈抓取SQL的興趣清單
                    {
                        if(i<interest.Count()-1)
                            allinterest += interest[i] + ",";
                        else
                            allinterest += interest[i];
                    }
                }
                else
                {
                    ViewData["errorinterest"] = "興趣最多選五個!!";
                    return View();
                    //allinterest = FirstInterest;
                }
                

                if (address != null)//判斷一定要有地址才會跑後續動作,防止第一次產生頁面錯誤
                {


                    if (pic != null)//判斷有沒有照片
                    {
                        //判斷照片類型,類型不對將跳回原本的照片
                        if (Path.GetFileName(pic.FileName).Contains(".jpg"))
                        {
                            picname = username + ".jpg";
                            var filename = Path.Combine(he.WebRootPath, @"image\", picname);//宣告照片路徑和黨名
                           
                            FileStream A = new FileStream(filename, FileMode.Create);//宣告創建或覆蓋檔案的路徑
                            pic.CopyTo(A);//複製進指定路徑
                            A.Close();//將FileStream暫時關閉
                            
                            ViewData["personalphoto"] = picname;//顯示View的照片
                            sInformationModel.Alter(username, nickname, address, job, allinterest.ToString(), resume, picname);//更新資料庫
                            return LocalRedirect("/Website/PersonalInformation");//返回個人資訊
                        }
                        else
                        {
                            picname = First;
                            ViewData["personalphoto"] = picname;//顯示View的照片
                            sInformationModel.Alter(username, nickname, address, job, allinterest.ToString(), resume, picname);//更新資料庫
                            return LocalRedirect("/Website/PersonalInformation");//返回個人資訊
                        }
                    }
                    else
                    {
                        picname = First;//沒加照片會指定為原照片
                        ViewData["personalphoto"] = First;//顯示在View的照片
                        sInformationModel.Alter(username, nickname, address, job, allinterest.ToString(), resume, picname);//更新資料庫
                        return LocalRedirect("/Website/PersonalInformation");//返回個人資訊
                    }
                }          
                return View(model:sInformationModel);
            }
            catch
            {
                return View();
            }
            
        }
        public IActionResult AlterPassword(string password,string newpassword,string newpassword2)
        {
            try
            {
                string username = "";
                if (HttpContext.Session.Keys.Contains("Online"))//用session記住是哪個使用者
                {
                    username = HttpContext.Session.GetString("Online");
                }
                SInformationModel sInformationModel = new SInformationModel();//連接資料庫
                sInformationModel.Select(username);//搜尋該使用者的所有資料
                List<Data> DataList = sInformationModel.DataList;//宣告一個List存Model裡的資料
            
                string userpassword ="";
                foreach (Data data in DataList)//抓取使用者密碼
                {
                    userpassword = data.password;
                }
                ViewData["erroruser"] = "";
                if (password != null&&newpassword!=null&&newpassword2!=null)
                {
                    if(password!= userpassword)//判斷原密碼是否輸入正確
                    {
                        ViewData["errorpassword"] = "原密碼輸入錯誤!";
                        return View();
                    }
                    else
                    {
                        bool checkPassword = Regex.IsMatch(newpassword, @"^.*(?=.{10,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$");//密碼原則
                        if (!checkPassword)//判斷新密碼是否符合原則
                        {
                            ViewData["errornewpassword"] = "密碼原則:十個字符以上,必須包含大小寫英文字母.數字.特殊符號";
                            return View();
                        }
                        else
                        {
                            if (newpassword != newpassword2)//判斷新密碼和確認密碼是否相同
                            {
                                ViewData["errornewpassword2"] = "請確認密碼是否相同!";
                                return View();
                            }
                            else
                            {
                                return LocalRedirect("/Website/AlterInformation");//返回更改資訊頁面
                            }
                        }
                    }
                }
                else
                {
                    ViewData["erroruser"] = "請輸入完整資訊,欄位不可空白";
                }
                    return View();
            }
            catch
            {
                return View();
            }
        }
        /*public IActionResult Album(List<IFormFile> album)
        {
            string username = "";
            if (HttpContext.Session.Keys.Contains("Online"))//用session記住是哪個使用者
            {
                username = HttpContext.Session.GetString("Online");
            }
            SInformationModel sInformationModel = new SInformationModel();//連接資料庫
            sInformationModel.SelectAlbum(username);//搜尋相簿
            List<Data> DataList = sInformationModel.DataList;//宣告List存相簿名稱
            foreach(Data data in DataList)//抓取相簿名稱
            {
                ViewData["album1"] = data.album1;
                ViewData["album2"] = data.album2;
                ViewData["album3"] = data.album3;
                ViewData["album4"] = data.album4;
                ViewData["album5"] = data.album5;
                ViewData["album6"] = data.album6;
                ViewData["album7"] = data.album7;
                ViewData["album8"] = data.album8;
            }
            List<string> albumname = new List<string>();//宣告List存檔案名稱
            string picnames = "";//宣告照片路經
            int i = 1;//宣告數字,存1~8的照片到相簿

            if (album.Count > 8)
            {
                ViewData["erroralbum1"] = "相簿僅提供放八張照片";
                return View();
            }

            foreach (IFormFile files in album)//將每一張照片存進主控台,並定義名為username1~8
            {
                //if (files != null)//判斷有沒有照片
                //{
                    //判斷照片類型,類型不對將跳回原本的照片
                    if (Path.GetFileName(files.FileName).Contains(".jpg"))
                        picnames = username +i+ ".jpg";
                    else if (Path.GetFileName(files.FileName).Contains(".bmp"))
                        picnames = username +i+ ".bmp";
                    else if (Path.GetFileName(files.FileName).Contains(".gif"))
                        picnames = username + i + ".gif";
                    else if (Path.GetFileName(files.FileName).Contains(".jpeg"))
                        picnames = username + i + ".jpeg";
                    else if (Path.GetFileName(files.FileName).Contains(".png"))
                        picnames = username + i + ".png";
                    else if (Path.GetFileName(files.FileName).Contains(".tif"))
                        picnames = username + i + ".tif";
                    else
                        picnames = "PresetPhoto.bmp";

                    var filename = Path.Combine(he.WebRootPath, @"image\", picnames);//宣告照片路徑和黨名
                    files.CopyTo(new FileStream(filename, FileMode.Create));//在指定路徑放入照片
                    albumname.Add(filename);//將檔名加LList<string> filename

                    i++;
                //}
                
            }
            int j = 1;
            string albumname1 = "";//更新相簿照片名稱
            string albumname2 = "";
            string albumname3 = "";
            string albumname4 = "";
            string albumname5 = "";
            string albumname6 = "";
            string albumname7 = "";
            string albumname8 = "";
            foreach (string albumnames in albumname)//給每個照片檔名
            {
                albumname1 = albumnames[0].ToString();
                albumname2 = albumnames[1].ToString();
                albumname3 = albumnames[2].ToString();
                albumname4 = albumnames[3].ToString();
                albumname5 = albumnames[4].ToString();
                albumname6 = albumnames[5].ToString();
                albumname7 = albumnames[6].ToString();
                albumname8 = albumnames[7].ToString();
            }
            //ViewData["personalphoto"] = picname;//顯示View的照片
            sInformationModel.AlterAlbum(username,albumname1, albumname2, albumname3, albumname4, albumname5, albumname6, albumname7, albumname8);//更新資料庫
            //return LocalRedirect("/Website/PersonalInformation");//返回個人資訊
            return View();
        }*/
    }
}
