using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Movify.Migrations
{
    public partial class saturday4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    email = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    phone = table.Column<string>(nullable: false),
                    dob = table.Column<DateTime>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    role = table.Column<string>(nullable: true, defaultValue: "customer"),
                    status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.email);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: false),
                    status = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Theater",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: false),
                    status = table.Column<bool>(nullable: false, defaultValue: true),
                    rows = table.Column<int>(nullable: false),
                    cols = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theater", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    posterURL = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    releaseDate = table.Column<DateTime>(nullable: true),
                    actors = table.Column<string>(nullable: true),
                    duration = table.Column<string>(nullable: true),
                    trailerURL = table.Column<string>(nullable: true),
                    genreid = table.Column<int>(nullable: false),
                    status = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.id);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_genreid",
                        column: x => x.genreid,
                        principalTable: "Genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    r = table.Column<int>(nullable: false),
                    c = table.Column<int>(nullable: false),
                    theaterid = table.Column<int>(nullable: false),
                    status = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.id);
                    table.ForeignKey(
                        name: "FK_Seat_Theater_theaterid",
                        column: x => x.theaterid,
                        principalTable: "Theater",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MovieShows",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    startTime = table.Column<DateTime>(nullable: false),
                    endTime = table.Column<DateTime>(nullable: false),
                    price = table.Column<double>(nullable: false),
                    theaterid = table.Column<int>(nullable: false),
                    movieid = table.Column<int>(nullable: false),
                    status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieShows", x => x.id);
                    table.ForeignKey(
                        name: "FK_MovieShows_Movies_movieid",
                        column: x => x.movieid,
                        principalTable: "Movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieShows_Theater_theaterid",
                        column: x => x.theaterid,
                        principalTable: "Theater",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    seatid = table.Column<int>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    movieshowid = table.Column<int>(nullable: false),
                    status = table.Column<bool>(nullable: false),
                    paymentStatus = table.Column<string>(nullable: true, defaultValue: "pending"),
                    paymentDate = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tickets_Customers_email",
                        column: x => x.email,
                        principalTable: "Customers",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_MovieShows_movieshowid",
                        column: x => x.movieshowid,
                        principalTable: "MovieShows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Seat_seatid",
                        column: x => x.seatid,
                        principalTable: "Seat",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_genreid",
                table: "Movies",
                column: "genreid");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShows_movieid",
                table: "MovieShows",
                column: "movieid");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShows_theaterid",
                table: "MovieShows",
                column: "theaterid");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_theaterid",
                table: "Seat",
                column: "theaterid");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_email",
                table: "Tickets",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_movieshowid",
                table: "Tickets",
                column: "movieshowid");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_seatid",
                table: "Tickets",
                column: "seatid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "MovieShows");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Theater");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
