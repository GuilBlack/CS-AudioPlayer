using AudioPlayer.Data;
using AudioPlayer.Data.Repositories;
using AudioPlayer.Helpers;
using AudioPlayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Controllers
{
    [Authorize]
    public class MusicController : Controller
    {
        private readonly MusicRepository _musicRepo;
        private readonly PlaylistRepository _playlistRepo;
        private readonly IWebHostEnvironment _appEnv;

        public MusicController(
            ApplicationDbContext dbContext,
            IWebHostEnvironment appEnv)
        {
            _musicRepo = new MusicRepository(dbContext);
            _playlistRepo = new PlaylistRepository(dbContext);
            _appEnv = appEnv;
        }

        public ActionResult Index(Guid playlistId)
        {
            Playlist playlist = _playlistRepo.Get(playlistId);
            if (playlist.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();
            ViewBag.playlistId = playlistId;
            ViewBag.playlistName = _playlistRepo.Get(playlistId).PlaylistName;
            List<Music> musics = _musicRepo.GetAllFromPlaylist(playlistId);
            return View(musics);
        }

        public ActionResult Create(Guid playlistId)
        {
            Playlist playlist = _playlistRepo.Get(playlistId);
            if (playlist.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();
            ViewBag.playlistId = playlistId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Music music, IFormFile musicFile , Guid playlistId)
        {
            Playlist playlist = _playlistRepo.Get(playlistId);
            if (playlist.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();
            if (musicFile == null || musicFile.Length == 0)
            {
                ViewBag.playlistId = playlistId;
                ViewBag.noFile = "You must upload a file";
                return View();
            }
            var folderPath = Path.Combine(_appEnv.WebRootPath, "audio", playlistId.ToString());
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileExtension = musicFile.FileName.Substring(musicFile.FileName.Length-4);
            if(fileExtension != ".mp3" && fileExtension != ".wav")
            {
                ViewBag.playlistId = playlistId;
                ViewBag.noFile = "Your file must be of type mp3 or wav";
                return View();
            }

            var filePath = Path.Combine(folderPath, musicFile.FileName);
            if(System.IO.File.Exists(filePath))
            {
                ViewBag.playlistId = playlistId;
                ViewBag.noFile = "The file that you want to upload already exists";
                return View();
            }
            try
            {

                music.filePath = String.Concat("/audio/", playlistId.ToString(), "/" , musicFile.FileName);
                music.PlaylistId = playlistId;
                _musicRepo.Create(music);
                _musicRepo.SaveChanges();

            }
            catch
            {
                ViewBag.playlistId = playlistId;
                return View();
            }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await musicFile.CopyToAsync(stream);
                }
                return RedirectToAction(nameof(Index), new { playlistId = playlistId });
        }

        public ActionResult Edit(Guid id)
        {
            Music music = _musicRepo.Get(id);
            Playlist playlist = _playlistRepo.Get(music.PlaylistId);
            if (playlist.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();
            return View(_musicRepo.Get(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Music music)
        {
            Music musicToSave = null;
            try
            {
                musicToSave = _musicRepo.Get(music.Id);
            } catch
            {
                return View();
            }
            if (musicToSave == null)
            {
                return View();
            }
            Playlist playlist = _playlistRepo.Get(musicToSave.PlaylistId);
            if (playlist.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();

            musicToSave.MusicName = music.MusicName;
            musicToSave.Artist = music.Artist;
            musicToSave.Album = music.Album;
            try
            {
                _musicRepo.Update(musicToSave);
                _musicRepo.SaveChanges();
                return RedirectToAction(nameof(Index), new { playlistId = musicToSave.PlaylistId });
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            Music music = _musicRepo.Get(id);

            Playlist playlist = _playlistRepo.Get(music.PlaylistId);
            if (playlist.ApplicationUserId != this.User.GetUserId())
                return Unauthorized();

            var fileName = music.filePath.Substring(44);
            var filePath = Path.Combine(_appEnv.WebRootPath, "audio", music.PlaylistId.ToString() , fileName);
            if (System.IO.File.Exists(filePath))
            {
                Task.Run(() => { 
                    System.IO.File.Delete(filePath); 
                });
            }

            _musicRepo.Delete(music.Id);
            _musicRepo.SaveChanges();
            return RedirectToAction(nameof(Index), new { playlistId = music.PlaylistId });
        }
    }
}
