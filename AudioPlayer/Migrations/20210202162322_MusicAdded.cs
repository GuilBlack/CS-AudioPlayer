using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AudioPlayer.Migrations
{
    public partial class MusicAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Musics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MusicName = table.Column<string>(maxLength: 20, nullable: false),
                    Artist = table.Column<string>(maxLength: 20, nullable: true),
                    Album = table.Column<string>(maxLength: 20, nullable: true),
                    filePath = table.Column<string>(nullable: true),
                    PlaylistId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musics_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Musics_PlaylistId",
                table: "Musics",
                column: "PlaylistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Musics");
        }
    }
}
