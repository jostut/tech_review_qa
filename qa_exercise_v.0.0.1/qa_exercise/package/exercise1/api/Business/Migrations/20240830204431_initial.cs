using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StargateAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AstronautDuty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rank = table.Column<string>(type: "TEXT", nullable: false),
                    DutyTitle = table.Column<string>(type: "TEXT", nullable: false),
                    DutyStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DutyEndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstronautDuty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AstronautDuty_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "John Doe" });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "Password", "PersonId", "Username" },
                values: new object[] { 1, "6ca13d52ca70c883e0f0bb101e425a89e8624de51db2d2392593af6a84118090", 1, "john.doe" });

            migrationBuilder.InsertData(
                table: "AstronautDuty",
                columns: new[] { "Id", "DutyEndDate", "DutyStartDate", "DutyTitle", "PersonId", "Rank" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 30, 16, 44, 30, 793, DateTimeKind.Local).AddTicks(8511), new DateTime(2024, 8, 30, 16, 44, 30, 793, DateTimeKind.Local).AddTicks(8427), "Commander", 1, "Capt" },
                    { 2, new DateTime(2024, 8, 29, 16, 44, 30, 793, DateTimeKind.Local).AddTicks(8521), new DateTime(2022, 8, 30, 16, 44, 30, 793, DateTimeKind.Local).AddTicks(8519), "Engineer", 1, "1LT" },
                    { 3, new DateTime(2022, 8, 29, 16, 44, 30, 793, DateTimeKind.Local).AddTicks(8526), new DateTime(2020, 8, 30, 16, 44, 30, 793, DateTimeKind.Local).AddTicks(8525), "Trainee", 1, "2LT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_PersonId",
                table: "Account",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AstronautDuty_PersonId",
                table: "AstronautDuty",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "AstronautDuty");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
