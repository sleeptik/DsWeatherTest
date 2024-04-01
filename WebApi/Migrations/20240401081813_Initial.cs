using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherActivities", x => x.Id);
                    table.UniqueConstraint("AK_WeatherActivities_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "WindDirections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WindDirections", x => x.Id);
                    table.UniqueConstraint("AK_WindDirections_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "WeatherRecords",
                columns: table => new
                {
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    Humidity = table.Column<float>(type: "real", nullable: false),
                    DewPoint = table.Column<float>(type: "real", nullable: false),
                    AtmospherePressure = table.Column<int>(type: "integer", nullable: false),
                    WindSpeed = table.Column<int>(type: "integer", nullable: true),
                    Overcast = table.Column<int>(type: "integer", nullable: true),
                    CloudBase = table.Column<int>(type: "integer", nullable: true),
                    HorizontalVisibility = table.Column<int>(type: "integer", nullable: true),
                    WeatherActivityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherRecords", x => x.DateTime);
                    table.ForeignKey(
                        name: "FK_WeatherRecords_WeatherActivities_WeatherActivityId",
                        column: x => x.WeatherActivityId,
                        principalTable: "WeatherActivities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WeatherRecordWindDirection",
                columns: table => new
                {
                    WeatherRecordDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WindDirectionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherRecordWindDirection", x => new { x.WeatherRecordDateTime, x.WindDirectionsId });
                    table.ForeignKey(
                        name: "FK_WeatherRecordWindDirection_WeatherRecords_WeatherRecordDate~",
                        column: x => x.WeatherRecordDateTime,
                        principalTable: "WeatherRecords",
                        principalColumn: "DateTime",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeatherRecordWindDirection_WindDirections_WindDirectionsId",
                        column: x => x.WindDirectionsId,
                        principalTable: "WindDirections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherRecords_WeatherActivityId",
                table: "WeatherRecords",
                column: "WeatherActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherRecordWindDirection_WindDirectionsId",
                table: "WeatherRecordWindDirection",
                column: "WindDirectionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherRecordWindDirection");

            migrationBuilder.DropTable(
                name: "WeatherRecords");

            migrationBuilder.DropTable(
                name: "WindDirections");

            migrationBuilder.DropTable(
                name: "WeatherActivities");
        }
    }
}
