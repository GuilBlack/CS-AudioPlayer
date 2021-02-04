using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Playlist> Playlists { get; set; }
    }
}
