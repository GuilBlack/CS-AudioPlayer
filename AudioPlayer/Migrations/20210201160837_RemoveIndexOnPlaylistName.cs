using Microsoft.EntityFrameworkCore.Migrations;

namespace AudioPlayer.Migrations
{
    public partial class RemoveIndexOnPlaylistName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Playlists_PlaylistName",
                table: "Playlists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Playlists_PlaylistName",
                table: "Playlists",
                column: "PlaylistName",
                unique: true);
        }
    }
}
