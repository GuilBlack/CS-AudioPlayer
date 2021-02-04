using AudioPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Data.Repositories
{
    public class MusicRepository : IMusicRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public MusicRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Music Create(Music Music)
        {
            _dbContext.Add(Music);
            return Music;
        }

        public List<Music> GetAllFromPlaylist(Guid playlistId)
        {
            List<Music> musics = _dbContext.Playlists
                .Where(p => p.Id == playlistId)
                .SelectMany(p => p.Musics)
                .OrderBy(m => m.MusicName)
                .ToList();
            return musics;
        }

        public Music Get(Guid id)
        {
            return _dbContext.Musics.FirstOrDefault(m => m.Id == id);
        }

        public Music Update(Music Music)
        {
            _dbContext.Musics.Update(Music);
            return Music;
        }

        public void Delete(Guid id)
        {
            _dbContext.Musics.Remove(Get(id));
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
