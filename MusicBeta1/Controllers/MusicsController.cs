using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicBeta1.Models;
using System.IO;
using PagedList;

namespace MusicBeta1.Controllers
{
    public class MusicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Musics
        public ActionResult Index(string musicGenre, string searchString, string currentFilter, string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var GenreLst = new List<string>();

            var GenreQry = from d in db.Musics
                           orderby d.Genre
                           select d.Genre;
            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.musicGenre = new SelectList(GenreLst);

            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else if (musicGenre != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var musics = from m in db.Musics
                         select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                musics = musics.Where(s => s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(musicGenre))
            {
                musics = musics.Where(x => x.Genre == musicGenre);
            }
            if (ViewBag.CurrentFilter != null)
            {
                musics = musics.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    musics = musics.OrderByDescending(s => s.Title);
                    ViewBag.SortDescTitle = "(Desc)";
                    break;
                case "Date":
                    musics = musics.OrderBy(s => s.UploadDate);
                    ViewBag.SortAscDate = "(Asc)";
                    break;
                case "date_desc":
                    musics = musics.OrderByDescending(s => s.UploadDate);
                    ViewBag.SortDescDate = "(Desc)";
                    break;
                default:
                    musics = musics.OrderBy(s => s.Title);
                    ViewBag.SortAscTitle = "(Asc)";
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(musics.ToPagedList(pageNumber, pageSize));

        }


        // GET: Musics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Music music = db.Musics.Find(id);
            if (music == null)
            {
                return HttpNotFound();
            }
            return View(music);
        }

        // GET: Musics/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Musics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Artist,Genre,UploadDate,MusicUpload")] MusicViewModel mus)
        {
            if (mus.MusicUpload == null)
            {
                ModelState.AddModelError("MusicUpload", "This field is required.");
            }

            if (ModelState.IsValid)
            {
                var path = Path.GetFileName(mus.MusicUpload.FileName); 
             
                string MusicName = System.IO.Path.GetFileName(mus.MusicUpload.FileName);
                string physicalPath = Server.MapPath("~/Content/files/" + MusicName);
                // save image in folder
                mus.MusicUpload.SaveAs(physicalPath);

                var music = new Music();
                music.Title = mus.Title;
                music.Artist = mus.Artist;
                music.Genre = mus.Genre;
                music.UploadDate = mus.UploadDate;
                music.OriginalFileName = path;
                music.MusicPath = "~/Content/files/" + path;

                db.Musics.Add(music);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mus);
        }

        // GET: Musics/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Music music = db.Musics.Find(id);
            if (music == null)
            {
                return HttpNotFound();
            }
            return View(music);
        }

        // POST: Musics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "ID,Title,Artist,Genre,UploadDate,OriginalFileName,MusicPath")] Music music)
        {
            if (ModelState.IsValid)
            {
                db.Entry(music).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(music);
        }

        // GET: Musics/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Music music = db.Musics.Find(id);
            if (music == null)
            {
                return HttpNotFound();
            }
            return View(music);
        }

        // POST: Musics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Music music = db.Musics.Find(id);
            db.Musics.Remove(music);
            db.SaveChanges();
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
        public ActionResult UploadTooBig()
        {
            return View(new UploadTooBigViewModel());
        }
    }
}
