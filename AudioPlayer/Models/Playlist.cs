using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AudioPlayer.Models
{
    public class Playlist
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Playlist Name")]
        [MaxLength(40, ErrorMessage = "The playlist name that be more than 40 characters")]
        [MinLength(1, ErrorMessage ="You must input something")]
        public string PlaylistName { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<Music> Musics { get; set; }

    }
}
