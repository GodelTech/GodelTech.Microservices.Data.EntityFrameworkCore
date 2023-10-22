using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "currencyExchangeRate");

            migrationBuilder.CreateTable(
                name: "Bank",
                schema: "currencyExchangeRate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "currencyExchangeRate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AlphabeticCode = table.Column<string>(type: "nvarchar(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bank",
                schema: "currencyExchangeRate");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "currencyExchangeRate");
        }
    }
}
