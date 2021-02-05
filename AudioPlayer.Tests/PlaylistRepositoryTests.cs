using System;
using Xunit;
using AudioPlayer.Data;
using AudioPlayer.Data.Repositories;
using Moq;
using System.Collections.Generic;
using AudioPlayer.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayer.Tests
{
    public class PlaylistRepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContext;
        private PlaylistRepository _playlistRepo;
        private MusicRepository _musicRepo;

        [Fact]
        public void Test1()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieListDatabase")
            .Options;

            var data = new List<Playlist>
            {
                new Playlist
                {
                    Id = new Guid("D6439971-6030-4983-A531-08D8C6A0B4A2"),
                    PlaylistName =  "Just a playlist",
                    ApplicationUserId = "1265ace9-c141-4837-aab7-32eb45d1af2d"
                },
                new Playlist
                {
                    Id = new Guid("2B363996-E8AA-4710-70CA-08D8C6CBFC5B"),
                    PlaylistName =  "Just a playlist",
                    ApplicationUserId = "937bcaf9-3a2a-4136-a5b6-f3c8c6945b55"
                }
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Playlist>>();
            mockSet.As<IQueryable<Playlist>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Playlist>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Playlist>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Playlist>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var userData = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "John Doe",
                    Id = "1265ace9-c141-4837-aab7-32eb45d1af2d"
                },
                new ApplicationUser
                {
                    UserName = "John Snow",
                    Id = "937bcaf9-3a2a-4136-a5b6-f3c8c6945b55"
                }
            }.AsQueryable();
            var mockSetUser = new Mock<DbSet<ApplicationUser>>();
            mockSetUser.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockSetUser.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            mockSetUser.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            mockSetUser.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());

            _dbContext = new Mock<ApplicationDbContext>(options);
            _dbContext.Setup(c => c.Playlists).Returns(mockSet.Object);
            _dbContext.Setup(c => c.Users).Returns(mockSetUser.Object);

            _playlistRepo = new PlaylistRepository(_dbContext.Object);

            _playlistRepo.Create(new Playlist
            {
                Id = new Guid("CEEE9729-2B65-4292-7F8F-08D8C6D2FA9D"),
                PlaylistName = "Just a playlist",
                ApplicationUserId = "1265ace9-c141-4837-aab7-32eb45d1af2d"
            });

            _playlistRepo.SaveChanges();

            Assert.Equal(2, _playlistRepo.GetAllFromUser("1265ace9-c141-4837-aab7-32eb45d1af2d").Count);
        }

        [Fact]
        public void Test2()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieListDatabase")
            .Options;

            var data = new List<Playlist>
            {
                new Playlist
                {
                    Id = new Guid("D6439971-6030-4983-A531-08D8C6A0B4A2"),
                    PlaylistName =  "Just a playlist",
                    ApplicationUserId = "1265ace9-c141-4837-aab7-32eb45d1af2d",
                    Musics = new List<Music>
                    {
                        new Music
                        {
                            Id = new Guid("C80F178B-D710-48B6-BFAA-08D8C87ECF9B"),
                            MusicName = "Song",
                            filePath = "/audio/cca3e07d-55b8-4a17-bb9b-08d8c84436ea/Rick Astley - Never Gonna Give You Up (Video).mp3",
                            PlaylistId = new Guid("D6439971-6030-4983-A531-08D8C6A0B4A2")
                        }
                    }
                },
                new Playlist
                {
                    Id = new Guid("2B363996-E8AA-4710-70CA-08D8C6CBFC5B"),
                    PlaylistName =  "Just a playlist",
                    ApplicationUserId = "937bcaf9-3a2a-4136-a5b6-f3c8c6945b55",
                    Musics = new List<Music>
                    {
                        new Music
                        {
                            Id = new Guid("309CDBC0-5DA6-443F-EC25-08D8C875CA44"),
                            MusicName = "Music",
                            filePath = "/audio/cca3e07d-55b8-4a17-bb9b-08d8c84436ea/Vicetone & Tony Igy - Astronomia.mp3",
                            PlaylistId = new Guid("2B363996-E8AA-4710-70CA-08D8C6CBFC5B")
                        },
                        new Music
                        {
                            Id = new Guid("C80F178B-D710-48B6-BFAA-08D8C87ECF9B"),
                            MusicName = "Song",
                            filePath = "/audio/cca3e07d-55b8-4a17-bb9b-08d8c84436ea/Rick Astley - Never Gonna Give You Up (Video).mp3",
                            PlaylistId = new Guid("2B363996-E8AA-4710-70CA-08D8C6CBFC5B")
                        }
                    }
                }
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Playlist>>();
            mockSet.As<IQueryable<Playlist>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Playlist>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Playlist>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Playlist>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            Guid pIdOne = new Guid("D6439971-6030-4983-A531-08D8C6A0B4A2");
            Guid pIdTwo = new Guid("2B363996-E8AA-4710-70CA-08D8C6CBFC5B");

            var musicData = new List<Music>
            {
                new Music
                {
                    Id = new Guid("309CDBC0-5DA6-443F-EC25-08D8C875CA44"),
                    MusicName = "Music",
                    filePath = "/audio/cca3e07d-55b8-4a17-bb9b-08d8c84436ea/Vicetone & Tony Igy - Astronomia.mp3",
                    PlaylistId = new Guid("2B363996-E8AA-4710-70CA-08D8C6CBFC5B"),
                    Playlist = data.FirstOrDefault(p => p.Id == pIdTwo)
                },
                new Music
                {
                    Id = new Guid("C80F178B-D710-48B6-BFAA-08D8C87ECF9B"),
                    MusicName = "Song",
                    filePath = "/audio/cca3e07d-55b8-4a17-bb9b-08d8c84436ea/Rick Astley - Never Gonna Give You Up (Video).mp3",
                    PlaylistId = new Guid("2B363996-E8AA-4710-70CA-08D8C6CBFC5B"),
                    Playlist = data.FirstOrDefault(p => p.Id == pIdTwo)
                }
            }.AsQueryable();
            var mockSetMusic = new Mock<DbSet<Music>>();
            mockSetMusic.As<IQueryable<Music>>().Setup(m => m.Provider).Returns(musicData.Provider);
            mockSetMusic.As<IQueryable<Music>>().Setup(m => m.Expression).Returns(musicData.Expression);
            mockSetMusic.As<IQueryable<Music>>().Setup(m => m.ElementType).Returns(musicData.ElementType);
            mockSetMusic.As<IQueryable<Music>>().Setup(m => m.GetEnumerator()).Returns(musicData.GetEnumerator());

            _dbContext = new Mock<ApplicationDbContext>(options);
            _dbContext.Setup(c => c.Playlists).Returns(mockSet.Object);
            _dbContext.Setup(c => c.Musics).Returns(mockSetMusic.Object);

            _musicRepo = new MusicRepository(_dbContext.Object);
            _musicRepo.Create(new Music
            {
                Id = new Guid("ABD44931-3DEA-4B43-238F-08D8C8848C7D"),
                MusicName = "Yes",
                filePath = "/audio/cca3e07d-55b8-4a17-bb9b-08d8c84436ea/Smash Mouth - All Star.mp3",
                PlaylistId = new Guid("D6439971-6030-4983-A531-08D8C6A0B4A2"),
                Playlist = data.FirstOrDefault(p => p.Id == pIdOne)
            });

            _musicRepo.SaveChanges();
            Assert.Equal(2, _musicRepo.GetAllFromPlaylist(pIdTwo).Count);

        }
    }
}
