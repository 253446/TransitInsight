using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransitInsight.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImportedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    NumberOfDepartures = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StopPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    EnturId = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Locality = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopPlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StopPlaceId = table.Column<int>(type: "INTEGER", nullable: false),
                    LineName = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Destination = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    AimedDepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpectedDepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DelayMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    TransportMode = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departures_StopPlaces_StopPlaceId",
                        column: x => x.StopPlaceId,
                        principalTable: "StopPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteStops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StopPlaceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nickname = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteStops_StopPlaces_StopPlaceId",
                        column: x => x.StopPlaceId,
                        principalTable: "StopPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departures_StopPlaceId",
                table: "Departures",
                column: "StopPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteStops_StopPlaceId",
                table: "FavoriteStops",
                column: "StopPlaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departures");

            migrationBuilder.DropTable(
                name: "FavoriteStops");

            migrationBuilder.DropTable(
                name: "ImportLogs");

            migrationBuilder.DropTable(
                name: "StopPlaces");
        }
    }
}
