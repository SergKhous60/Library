using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BBLibraryApp.Data.BBLibraryData.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BigBand");

            migrationBuilder.CreateTable(
                name: "Chart",
                schema: "BigBand",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChartName = table.Column<string>(maxLength: 150, nullable: false),
                    Minutes = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    Seconds = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    Composer = table.Column<string>(maxLength: 50, nullable: true),
                    Arranger = table.Column<string>(maxLength: 50, nullable: true),
                    RecordingUrl = table.Column<string>(maxLength: 150, nullable: true),
                    Note = table.Column<string>(maxLength: 150, nullable: true),
                    ShelfNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chart", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Instrument",
                schema: "BigBand",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScoreOrder = table.Column<int>(nullable: false),
                    IsInDefaultSet = table.Column<bool>(nullable: false),
                    InstrumentName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "BigBand",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Note = table.Column<string>(maxLength: 200, nullable: true),
                    IsFullTime = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Venue",
                schema: "BigBand",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VenueName = table.Column<string>(maxLength: 150, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    Contact = table.Column<string>(maxLength: 200, nullable: true),
                    Comments = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venue", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChartInstrument",
                schema: "BigBand",
                columns: table => new
                {
                    ChartID = table.Column<int>(nullable: false),
                    InstrumentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartInstrument", x => new { x.ChartID, x.InstrumentID });
                    table.ForeignKey(
                        name: "FK_ChartInstrument_Chart_ChartID",
                        column: x => x.ChartID,
                        principalSchema: "BigBand",
                        principalTable: "Chart",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChartInstrument_Instrument_InstrumentID",
                        column: x => x.InstrumentID,
                        principalSchema: "BigBand",
                        principalTable: "Instrument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonInstrument",
                schema: "BigBand",
                columns: table => new
                {
                    PersonID = table.Column<int>(nullable: false),
                    InstrumentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonInstrument", x => new { x.PersonID, x.InstrumentID });
                    table.ForeignKey(
                        name: "FK_PersonInstrument_Instrument_InstrumentID",
                        column: x => x.InstrumentID,
                        principalSchema: "BigBand",
                        principalTable: "Instrument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonInstrument_Person_PersonID",
                        column: x => x.PersonID,
                        principalSchema: "BigBand",
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Performance",
                schema: "BigBand",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Comments = table.Column<string>(maxLength: 200, nullable: true),
                    VenueID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Performance_Venue_VenueID",
                        column: x => x.VenueID,
                        principalSchema: "BigBand",
                        principalTable: "Venue",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceChart",
                schema: "BigBand",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PerformanceID = table.Column<int>(nullable: false),
                    ChartID = table.Column<int>(nullable: false),
                    ChartListOrder = table.Column<int>(nullable: true),
                    PersonID = table.Column<int>(nullable: true),
                    InstrumentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceChart", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PerformanceChart_Chart_ChartID",
                        column: x => x.ChartID,
                        principalSchema: "BigBand",
                        principalTable: "Chart",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerformanceChart_Instrument_InstrumentID",
                        column: x => x.InstrumentID,
                        principalSchema: "BigBand",
                        principalTable: "Instrument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceChart_Performance_PerformanceID",
                        column: x => x.PerformanceID,
                        principalSchema: "BigBand",
                        principalTable: "Performance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerformanceChart_Person_PersonID",
                        column: x => x.PersonID,
                        principalSchema: "BigBand",
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChartInstrument_InstrumentID",
                schema: "BigBand",
                table: "ChartInstrument",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Performance_VenueID",
                schema: "BigBand",
                table: "Performance",
                column: "VenueID");

            migrationBuilder.CreateIndex(
                name: "IX_Performance_Name_Date",
                schema: "BigBand",
                table: "Performance",
                columns: new[] { "Name", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceChart_ChartID",
                schema: "BigBand",
                table: "PerformanceChart",
                column: "ChartID");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceChart_InstrumentID",
                schema: "BigBand",
                table: "PerformanceChart",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceChart_PerformanceID",
                schema: "BigBand",
                table: "PerformanceChart",
                column: "PerformanceID");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceChart_PersonID",
                schema: "BigBand",
                table: "PerformanceChart",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInstrument_InstrumentID",
                schema: "BigBand",
                table: "PersonInstrument",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Venue_VenueName",
                schema: "BigBand",
                table: "Venue",
                column: "VenueName",
                unique: true,
                filter: "[VenueName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChartInstrument",
                schema: "BigBand");

            migrationBuilder.DropTable(
                name: "PerformanceChart",
                schema: "BigBand");

            migrationBuilder.DropTable(
                name: "PersonInstrument",
                schema: "BigBand");

            migrationBuilder.DropTable(
                name: "Chart",
                schema: "BigBand");

            migrationBuilder.DropTable(
                name: "Performance",
                schema: "BigBand");

            migrationBuilder.DropTable(
                name: "Instrument",
                schema: "BigBand");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "BigBand");

            migrationBuilder.DropTable(
                name: "Venue",
                schema: "BigBand");
        }
    }
}
