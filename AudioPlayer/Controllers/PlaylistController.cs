using AudioPlayer.Data;
using AudioPlayer.Data.Repositories;
using AudioPlayer.Helpers;
using AudioPlayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace AudioPlayer.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private readonly PlaylistRepository _playlistRepo;
        private readonly IWebHostEnvironment _appEnv;

        public PlaylistController(
            ApplicationDbContext dbContext,
            IWebHostEnvironment appEnv)
        {
            _playlistRepo = new PlaylistRepository(dbContext);
            _appEnv = appEnv;
        }

        // GET: PlaylistController
        public ActionResult Index()
        {
            var userId = this.User.GetUserId();
            IEnumerable<Playlist> playlists = _playlistRepo.GetAllFromUser(userId);
            return View(playlists);
        }

        // GET: PlaylistController/Details/5
        public ActionResult Details(Guid id)
        {
            Playlist playlist = _playlistRepo.Get(id);
            if (playlist.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();
            return RedirectToAction("Index", "Music", new { playlistId = id });
        }

        // GET: PlaylistController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlaylistController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Playlist playlist)
        {
            playlist.ApplicationUserId = this.User.GetUserId();
            try
            {
                _playlistRepo.Create(playlist);
                _playlistRepo.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlaylistController/Edit/5
        public ActionResult Edit(Guid id)
        {
            Playlist playlist = _playlistRepo.Get(id);
            if (playlist.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();

            return View(playlist);
        }

        // POST: PlaylistController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Playlist playlist)
        {
            Playlist playlistToSave = _playlistRepo.Get(playlist.Id);
            playlistToSave.PlaylistName = playlist.PlaylistName;
            if (playlistToSave.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();

            try
            {
                _playlistRepo.Update(playlistToSave);
                _playlistRepo.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: PlaylistController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            Playlist playlist = _playlistRepo.Get(id);
            if (playlist.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();
            var folderPath = Path.Combine(_appEnv.WebRootPath, "audio", id.ToString());
            if (Directory.Exists(folderPath))
            {
                Task.Run(() => {
                    Directory.Delete(folderPath, true);
                });
            }
            _playlistRepo.Delete(id);
            _playlistRepo.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
