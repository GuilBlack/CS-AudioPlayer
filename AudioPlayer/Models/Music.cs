using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Models
{
    public class Music
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        [MaxLength(30, ErrorMessage = "The playlist name that be more than 40 characters")]
        [MinLength(1, ErrorMessage = "You must input something")]
        public string MusicName { get; set; }

        [Display(Name = "Artist")]
        [MaxLength(30, ErrorMessage = "The playlist name that be more than 40 characters")]
        public string Artist { get; set; }

        [Display(Name = "Album")]
        [MaxLength(30, ErrorMessage = "The playlist name that be more than 40 characters")]
        public string Album { get; set; }

        public string filePath { get; set; }

        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
