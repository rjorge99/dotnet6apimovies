using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesApi.Migrations
{
    public partial class MovieMovieTheater : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Character",
                table: "MoviesActors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Poster",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "MovieTheaters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTheaters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoviesMovieTheaters",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    MovieTheaterId = table.Column<int>(type: "int", nullable: false),
                    MovieTheatersMovieId = table.Column<int>(type: "int", nullable: true),
                    MovieTheatersMovieTheaterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesMovieTheaters", x => new { x.MovieId, x.MovieTheaterId });
                    table.ForeignKey(
                        name: "FK_MoviesMovieTheaters_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesMovieTheaters_MoviesMovieTheaters_MovieTheatersMovieId_MovieTheatersMovieTheaterId",
                        columns: x => new { x.MovieTheatersMovieId, x.MovieTheatersMovieTheaterId },
                        principalTable: "MoviesMovieTheaters",
                        principalColumns: new[] { "MovieId", "MovieTheaterId" });
                    table.ForeignKey(
                        name: "FK_MoviesMovieTheaters_MovieTheaters_MovieTheaterId",
                        column: x => x.MovieTheaterId,
                        principalTable: "MovieTheaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoviesMovieTheaters_MovieTheaterId",
                table: "MoviesMovieTheaters",
                column: "MovieTheaterId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesMovieTheaters_MovieTheatersMovieId_MovieTheatersMovieTheaterId",
                table: "MoviesMovieTheaters",
                columns: new[] { "MovieTheatersMovieId", "MovieTheatersMovieTheaterId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviesMovieTheaters");

            migrationBuilder.DropTable(
                name: "MovieTheaters");

            migrationBuilder.AlterColumn<string>(
                name: "Character",
                table: "MoviesActors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Poster",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
