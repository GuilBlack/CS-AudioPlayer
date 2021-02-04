using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Models
{
    public class MusicToSave : Music
    {
        [Required]
        public IFormFile MusicFile { get; set; }
    }
}
