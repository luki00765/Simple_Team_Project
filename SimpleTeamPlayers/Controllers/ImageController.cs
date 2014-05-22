using SimpleTeamPlayers.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SimpleTeamPlayers.Models;
using System.IO;
using System.Data.Entity.Infrastructure;

namespace SimpleTeamPlayers.Controllers
{
    public class ImageController : Controller
    {
        private SimpleTeamPlayersContext db = new SimpleTeamPlayersContext();

        // GET: /Image/
        public ActionResult Index()
        {
            var query = from i in db.Images
                        orderby i.FileName
                        select i;

            return View(query.ToList());
        }

        // GET: /Image/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Images
                        where i.ID == id
                        select i;

            var image = query.FirstOrDefault();

            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: /Image/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Image/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file)
        {
            Image image = new Image();

            try
            {
                if (file == null || file.ContentLength <= 0)
                {
                    throw new Exception("Plik jest pusty");
                }

                if (file.ContentLength > 512000)
                {
                    throw new Exception("Wielkość pliku nie może przekraczać 512kb");
                }

                var fileName = Path.GetFileName(file.FileName);
                var imageName = Path.GetFileNameWithoutExtension(fileName);
                var imageExtension = "";
                var contentLength = file.ContentLength;
                var contentType = file.ContentType;

                if(contentType == "image/jpeg")
                {
                    imageExtension = "jpg";
                }
                else if (contentType == "image/png")
                {
                    imageExtension = "png";
                }

                if(imageExtension == "")
                {
                    throw new Exception("Niewłaściwy format zdjęcia. Dopuszczalne formaty to: jpeg/png");
                }

                byte[] imageBytes = new byte[contentLength - 1];

                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    imageBytes = binaryReader.ReadBytes(file.ContentLength);
                }

                image.FileName = String.Format("{0:yyyMMddHHmmss}.{1}", DateTime.Now, imageExtension);
                image.Data = imageBytes;
                image.Extensions = contentType;
                db.Images.Add(image);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                String.Format("Błąd" + e.Message);
            }

            return View(image);
        }

        public ActionResult Show(string imageName = "")
        {
            if (String.IsNullOrEmpty(imageName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Images
                        where i.FileName == imageName
                        select i;

            var img = query.FirstOrDefault();

            if(img == null)
            {
                return HttpNotFound();
            }

            return File(img.Data, img.Extensions);
        }

        // GET: /Image/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Images
                        where i.ID == id
                        select i;

            var image = query.FirstOrDefault();

            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: /Image/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, HttpPostedFile file)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Images
                        where i.ID == id
                        select i;

            var image = query.FirstOrDefault();

            try
            {
                if(file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var imageName = Path.GetFileNameWithoutExtension(fileName);
                    var imageExtension = "";
                    var contentLength = file.ContentLength;
                    var contentType = file.ContentType;

                    byte[] imageBytes = new byte[contentLength - 1];

                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        imageBytes = binaryReader.ReadBytes(file.ContentLength);
                    }

                    image.FileName = String.Format("{0:yyyMMddHHmmss}.{1}", DateTime.Now, imageExtension);
                    image.Data = imageBytes;
                    image.Extensions = contentType;
                }

                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Dane zostały zaktualizowane przez inną osobę.");
            }

            catch (Exception e)
            {
                String.Format("Błąd" + e.Message);
            }

            return View(image);
        }

        // GET: /Image/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Images
                        where i.ID == id
                        select i;

            var image = query.FirstOrDefault();

            if (image == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.Images.Remove(image);
                db.SaveChanges();
            }
            catch(Exception)
            {
                throw new Exception("Błąd przy usuwaniu obrazka.");
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
