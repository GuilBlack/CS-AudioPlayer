using AudioPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Data.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PlaylistRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Playlist Create(Playlist playlist)
        {
            _dbContext.Playlists.Add(playlist);
            return playlist;
        }

        public List<Playlist> GetAllFromUser(string userId)
        {
            List<Playlist> playlists = _dbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Playlists)
                .OrderBy(p => p.Id)
                .ToList();

            return playlists;
        }

        public Playlist Get(Guid id)
        {
            return _dbContext.Playlists.FirstOrDefault(p => p.Id == id);
        }

        public Playlist Update(Playlist playlist)
        {
            _dbContext.Playlists.Update(playlist);
            return playlist;
        }

        public void Delete(Guid id)
        {
            _dbContext.Playlists.Remove(Get(id));
        }
        
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
