using AudioPlayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Music> Musics { get; set; }
    }
}
