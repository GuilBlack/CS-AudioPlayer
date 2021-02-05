using AudioPlayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AudioPlayer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<Music> Musics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Playlist>()
                .HasOne(p => p.ApplicationUser)
                .WithMany(b => b.Playlists)
                .IsRequired();

            builder.Entity<Music>()
                .HasOne(p => p.Playlist)
                .WithMany(b => b.Musics)
                .IsRequired();
        }
    }
}
