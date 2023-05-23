using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    LongitudeDelta = table.Column<double>(type: "float", nullable: false),
                    GreenwichDelta = table.Column<double>(type: "float", nullable: false),
                    Offset = table.Column<double>(type: "float", nullable: false),
                    StandartMeridian = table.Column<double>(type: "float", nullable: false),
                    LongitudeDeltaFromService = table.Column<double>(type: "float", nullable: false),
                    GreenwichtenZamanCinsindenFarki = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IhlasTemkin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SehirKisa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandartBoylamQuery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DaylightSavingTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    End = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDiff = table.Column<int>(type: "int", nullable: true),
                    EndDiff = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaylightSavingTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaylightSavingTimes_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrayTimes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Imsak = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fajr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tulu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Israk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zuhr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Asr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Maghrib = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Isha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrayTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrayTimes_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DaylightSavingTimes_CityId",
                table: "DaylightSavingTimes",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PrayTimes_CityId",
                table: "PrayTimes",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaylightSavingTimes");

            migrationBuilder.DropTable(
                name: "PrayTimes");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
