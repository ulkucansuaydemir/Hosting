using kayzer.project.Context;
using kayzer.project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace kayzer.project.Controllers
{
    public class ConfigureController : Controller
    {
        KayzerDbContext db = new KayzerDbContext();
        // GET: Configure
        public ActionResult Index(int? productID)
        {
            var selectedPro = this.db.Product.FirstOrDefault(x => x.ProductID == productID);

            var proView = new ProductViewModel() {
            ProductID = selectedPro.ProductID,
            ProductName = selectedPro.ProductName,
            ProductDesc = selectedPro.ProductDesc,
            Processor = selectedPro.Processor,
            Ram = selectedPro.Ram,
            DefaultProductModel = selectedPro.DefaultProductModel,
            HardDisk = selectedPro.HardDisk,
            ExtraSoftware = selectedPro.ExtraSoftware,
            IntAcces = selectedPro.IntAcces
            };

            var securityAnd = db.ExtraProduct.Where(x => x.IsDeleted == false && x.ExtraProductType == "4").Select(e => new ExtraProductViewModel() {

                ExtraProductID = e.ExtraProductID,
                ExtraProductName = e.ExtraProductName,
                ExtraProductPrice = e.ExtraProductPrice,
                ExtraProductDesc = e.ExtraProductDesc,
                OsType = e.OsType
            });


            var Ram = db.Rams.Where(x => x.IsDeleted == false).Select(e => new RamViewModel()
            {
                RamName = e.RamName,
                RamPrice = e.RamPrice
            });

            var Hard = db.HardDisks.Where(x => x.IsDeleted == false).Select(e => new HardDiskViewModel()
            {
                HardDiskName = e.HardDiskName,
                HardDiskPrice = e.HardDiskPrice
            });
            var Pro = db.Processors.Where(x => x.IsDeleted == false).Select(e => new ProcessorViewModel()
            {
                ProcessorName = e.ProcessorName,
                ProcessorPrice = e.ProcessorPrice
            });
            var Int = db.InternetAccesses.Where(x => x.IsDeleted == false).Select(e => new InternetAccessViewModel()
            {
                IntAccName = e.IntAccName,
                IntAccPrice = e.IntAccPrice
            });


            MainViewModel main = new MainViewModel()
            {
                ProductViewModel = proView,
                ExtraProductViewModel = securityAnd,
                RamViewModel = Ram,
                HardDiskViewModel = Hard,
                ProcessorViewModel = Pro,
                InternetAccessViewModel = Int
                
                
            };

            return View(main);
        }

        public JavaScriptResult GetFeatures(int id)
        {
            var extraisltm = this.db.ExtraProduct.Where(x => x.OsType == id.ToString()).ToList();
            //var extracntrl = this.db.ExtraProduct.Where(x => x.OsType == id.ToString() && x.ExtraProductType == "2").ToList();
            //var extraekyazlm = this.db.ExtraProduct.Where(x => x.OsType == id.ToString() && x.ExtraProductType == "3").ToList();


            var extras = extraisltm.Select(x => new { @id = x.ExtraProductID, @name = x.ExtraProductName, @type = x.ExtraProductType, @price =x.ExtraProductPrice }).ToList();
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            var result = serialize.Serialize(extras);
            return JavaScript(result);
        } 

        public JavaScriptResult GetExtras()
        {
            var extraproduct = this.db.ExtraProduct.Where(x => x.IsDeleted == false && x.ExtraProductType == "3").ToList();
            var ext = extraproduct.Select(x => new { @id = x.ExtraProductID, @name = x.ExtraProductName }).ToList();
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            var respns = serialize.Serialize(ext);
            return JavaScript(respns);
        }

        public JavaScriptResult GetOrders(OrderViewModel model)
        {
            if (SendMail(model))
            {
                var newOrder = new OrderModel()
                {

                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    CntrlPnl = model.CntrlPnl,
                    CPU = model.CPU,
                    HardDisc = model.HardDisc,
                    IntAcc = model.IntAcc,
                    IPSec = model.IPSec,
                    OrderID = model.OrderID,
                    OS = model.OS,
                    OSType = model.OSType,
                    Price = model.Price,
                    Ram = model.Ram,
                    SecPer = model.SecPer,
                    Server = model.Server,
                    ExtraSoft = model.ExtraSoft
                };

                this.db.Orders.Add(newOrder);
                db.SaveChanges();

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                var res = serialize.Serialize(newOrder);
                return JavaScript(res);
            }

            return JavaScript("başarız");
           
        }

        public bool SendMail(OrderViewModel model)
        {
            MailMessage ePosta = new MailMessage() {
                From = new MailAddress(model.Email),
                
            };

            MailMessage ePostaCustomer = new MailMessage()
            {
                From = new MailAddress("ulkuaydemir@admatictechnology.com")

            };

            ePosta.To.Add("ulkuaydemir@admatictechnology.com");
            ePostaCustomer.To.Add(model.Email);
            //
           
            //ePosta.To.Add("alan2@gmail.com");
            //ePosta.To.Add("alan3@hotmail.com");
            //
            //ePosta.Attachments.Add(new Attachment(@"C:\deneme.txt"));
            //
            
            ePosta.Subject = "Kayzer Sipariş";
            ePostaCustomer.Subject = "Kayzer Sipariş";
            //
            ePosta.IsBodyHtml = true;
            var body = "<h4>Müşteri Bilgileri</h4><p>Adı Soyadı: {0}</p><p>Email: {1}</p><p>Telefonu: {2}</p><h4>Sipariş Detayları</h4><h5>Donanımsal Özellikleri</h5><p>Sunucu Adı: {3}</p><p>İşlemci: {4}</p><p>Ram: {5}</p><p>Sabit Disk: {6}</p><p>İnternet Erişimi: {7}</p><p>IP Seçeneği: {8}</p><h5>Yazılım Özellikleri</h5><p>Windows/Linux: {9}</p><p>İşletim Sistemi: {10}</p><p>Kontrol Paneli: {11}</p><p>Ek Yazılımları: {12}</p><h5>Güvenlik ve Performans</h5><p>Güv-Per Ürünleri: {13}</p><h4>Ücret: {14} TL </h4>";
            ePosta.Body = string.Format(body,model.Name,model.Email,model.Phone,model.Server,model.CPU,model.Ram,model.HardDisc,model.IntAcc,model.IPSec,model.OSType,model.OS,model.CntrlPnl,model.ExtraSoft,model.SecPer,model.Price);
            ePostaCustomer.Body = model.OrderID.ToString() +"nolu siparişiniz bize ulaşmıştır. En kısa sürede size geri dönüş sağlanacaktır. İyi Günler dileriz.";
            //
            SmtpClient smtp = new SmtpClient() {
                Credentials = new System.Net.NetworkCredential("hostingkayzer@gmail.com", "Kayzer1234*"),
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true
            };
            //
            
            object userState = ePosta;
            bool kontrol = true;
            try
            {
                smtp.Send(ePosta);
                smtp.Send(ePostaCustomer);
            }
            catch (SmtpException ex)
            {
                kontrol = false;
                string exp =  ex.Message;
               // System.Windows.Forms.MessageBox.Show(ex.Message, "Mail Gönderme Hatasi");
            }
            return kontrol;
        }

        public ActionResult Colocation(int? coID)
        {
            return View();
        }

        public JavaScriptResult GetCollocationOrders(OrderColocationViewModel model)
        {
            string nasf , loc , ips;
            switch (model.NasFtp)
            {
                case "4":
                    nasf = "250GB NAS / FTP ALANI";
                    break;
                case "2":
                    nasf = "50GB NAS / FTP ALANI";
                    break;
                case "3":
                    nasf = "100GB NAS / FTP ALANI";
                    break;
                case "5":
                    nasf = "500GB NAS / FTP ALANI";
                    break;
                case "6":
                    nasf = "1000GB NAS / FTP ALANI";
                    break;

                default:
                    nasf = " ";
                    break;
            }

            switch (model.Location)
            {
                case "1":
                    loc = "1U PAYLAŞIMLI KABİN";
                    break;
                case "2":
                    loc = "2U PAYLAŞIMLI KABİN";
                    break;
                case "3":
                    loc = "3U PAYLAŞIMLI KABİN";
                    break;
                case "4":
                    loc = "4U PAYLAŞIMLI KABİN";
                    break;
                case "5":
                    loc = "5U PAYLAŞIMLI KABİN";
                    break;

                default:
                    loc = " ";
                    break;
            }

            switch (model.IpSection)
            {
                case "1":
                    ips = "1IP ADRESİ";
                    break;
                case "2":
                    ips = "5IP ADRESİ (/29VLAN)";
                    break;
                case "3":
                    ips = "13IP ADRESİ (/28VLAN)";
                    break;
                default:
                    ips = " ";
                    break;
            }
           
            var CoOrdermodel = new OrderColocationModel()
            {
                Email = model.Email,
                IpSection = ips,
                Location = loc,
                Name = model.Name,
                NasFtp = nasf,
                NetworkSecurity = model.NetworkSecurity,
                Performs = model.Performs,
                Phone = model.Phone,
                Power = model.Power,
                Price = model.Price,
                RedundantIntAcc = model.RedundantIntAcc
            };

            if (SendCoMail(CoOrdermodel))
            {
                db.OrderColocation.Add(CoOrdermodel);
                db.SaveChanges();

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                var res = serialize.Serialize(CoOrdermodel);
                return JavaScript(res);
     

            }
            return JavaScript("error");
        }

        public bool SendCoMail(OrderColocationModel model)
        {
            MailMessage ePosta = new MailMessage() {
                From = new MailAddress(model.Email)

            };

            MailMessage ePostaCustomer = new MailMessage()
            {
                From = new MailAddress("ulkuaydemir@admatictechnology.com")

            };
            //
            ePosta.To.Add("ulkuaydemir@admatictechnology.com");
            ePostaCustomer.To.Add(model.Email);
            //ePosta.To.Add("alan2@gmail.com");
            //ePosta.To.Add("alan3@hotmail.com");
            //
            //ePosta.Attachments.Add(new Attachment(@"C:\deneme.txt"));
            //

            ePosta.Subject = "Kayzer Sipariş _ Colocation";
            ePostaCustomer.Subject = "Kayzer Sipariş _ Colocation";
            //
            ePosta.IsBodyHtml = true;
            var body = "<h4>Müşteri Bilgileri</h4><p>Adı Soyadı: {0}</p><p>Email: {1}</p><p>Telefonu: {2}</p><h4>Sipariş Detayları</h4><p>Co-Location Adı: {3}</p><p>Barındırma Alanı: {4}</p><p>Güç: {5}</p><p>Yedekli İnternet Erişimi: {6}</p><p>IP Seçenekleri: {7}</p><p>NAS / FTP Alanı: {8}</p><p>Network ve Güvenlik Ürünleri: {9}</p><p>Performans Ürünleri: {10}</p><p>Ücreti: {11}</p>";
            ePosta.Body = string.Format(body, model.Name, model.Email, model.Phone, model.Name, model.Location, model.Power, model.RedundantIntAcc, model.IpSection, model.NasFtp, model.NetworkSecurity, model.Performs, model.Price);
            ePostaCustomer.Body = model.OrderID.ToString() + "nolu siparişiniz bize ulaşmıştır. En kısa sürede size geri dönüş sağlanacaktır. İyi Günler dileriz.";
            //
            SmtpClient smtp = new SmtpClient() {
                Credentials = new System.Net.NetworkCredential("hostingkayzer@gmail.com", "Kayzer1234*"),
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true
            };
            //
            object userState = ePosta;
            bool kontrol = true;
            try
            {
                smtp.Send(ePosta);
                smtp.Send(ePostaCustomer);
            }
            catch (SmtpException ex)
            {
                kontrol = false;
                string exp = ex.Message;
                // System.Windows.Forms.MessageBox.Show(ex.Message, "Mail Gönderme Hatasi");
            }
            return kontrol;
        }

        public JavaScriptResult GetPrices(ProductViewModel model)
        {
            var product = db.Product.Where(x => x.ProductID == model.ProductID).FirstOrDefault();
            var modelProPrice = new ProductViewModel()
            {
                HardDisk = product.HardDisk.Select(y => new HardDiskModel { HardDiskID = y.HardDiskID, HardDiskName = y.HardDiskName, HardDiskPrice = y.HardDiskPrice }).ToList(),
                IntAcces = product.IntAcces.Select(z => new InternetAccessModel { IntAccID = z.IntAccID, IntAccPrice = z.IntAccPrice, IntAccName = z.IntAccName }).ToList(),
                Processor = product.Processor.Select(t => new ProcessorModel { ProcessorID = t.ProcessorID, ProcessorName = t.ProcessorName, ProcessorPrice = t.ProcessorPrice }).ToList(),
                Ram = product.Ram.Select( u => new RamModel { RamID = u.RamID, RamName = u.RamName, RamPrice = u.RamPrice}).ToList()
            };
            var modelExtra = db.ExtraProduct.Where(e => e.IsDeleted == false).Select(e => new ExtraProductViewModel()
            {
                ExtraProductName = e.ExtraProductName,
                ExtraProductPrice = e.ExtraProductPrice,
                OsType = e.OsType
              
            }).ToList();

            var main = new MainViewModel()
            {
                ExtraProductViewModel = modelExtra,
                ProductViewModel = modelProPrice,

            };



            //var process = prod.Processor.ToList();
            //var rams = prod.Ram.ToList();
            //var ints = prod.IntAcces.ToList();
            //var hards = prod.HardDisk.ToList();
            //var product = this.db.Rams.Where(x => x.IsDeleted == false && x.RamName == model.Ram).FirstOrDefault();
            //var ViewRam = new RamViewModel() {
            //    RamPrice = product.RamPrice,
            //    RamName = product.RamName,
            //    RamID = product.RamID

            //};
            ////var ext = extraproduct.Select(x => new { @id = x.ExtraProductID, @name = x.ExtraProductName }).ToList();
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            var respns = serialize.Serialize(main);
       

            return JavaScript(respns);

        }

        //public JavaScriptResult GetPriceHard(DefaultProductViewModel model)
        //{
        //    var product = this.db.HardDisks.Where(x => x.IsDeleted == false && x.HardDiskName == model.HardDisk).FirstOrDefault();
        //    var ViewHard = new HardDiskViewModel()
        //    {
        //        HardDiskPrice = product.HardDiskPrice,
        //        HardDiskName = product.HardDiskName,
        //        HardDiskID = product.HardDiskID

        //    };
        //    //var ext = extraproduct.Select(x => new { @id = x.ExtraProductID, @name = x.ExtraProductName }).ToList();
        //    JavaScriptSerializer serialize = new JavaScriptSerializer();
        //    var respns = serialize.Serialize(ViewHard);
        //    return JavaScript(respns);

        //}

    }
}