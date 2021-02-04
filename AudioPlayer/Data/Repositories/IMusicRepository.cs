using AudioPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Data.Repositories
{
    interface IMusicRepository
    {
        List<Music> GetAllFromPlaylist(Guid playlistId);

        Music Get(Guid id);

        Music Create(Music Music);

        Music Update(Music Music);

        void Delete(Guid id);

        void SaveChanges();
    }
}
