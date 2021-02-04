using AudioPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Data.Repositories
{
    interface IPlaylistRepository
    {
        List<Playlist> GetAllFromUser(string userId);

        Playlist Get(Guid id);

        Playlist Create(Playlist playlist);

        Playlist Update(Playlist playlist);

        void Delete(Guid id);

        void SaveChanges();

    }
}
