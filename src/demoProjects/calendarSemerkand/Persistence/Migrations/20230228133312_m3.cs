using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class m3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "PrayTimes",
                newName: "CountryName");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "DaylightSavingTimes",
                newName: "CountryName");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Cities",
                newName: "CountryName");

            migrationBuilder.CreateTable(
                name: "Times",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Coordinate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbrevation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasicOffset = table.Column<double>(type: "float", nullable: true),
                    DstOffset = table.Column<double>(type: "float", nullable: true),
                    TotalOffset = table.Column<double>(type: "float", nullable: true),
                    F6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldLocalTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewLocalTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewTotalOffset = table.Column<double>(type: "float", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    OldLocalTime1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewLocalTime1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewTotalOffset1 = table.Column<double>(type: "float", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Times", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Times_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Times_CityId",
                table: "Times",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Times");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "PrayTimes",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "DaylightSavingTimes",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "Cities",
                newName: "Country");
        }
    }
}
