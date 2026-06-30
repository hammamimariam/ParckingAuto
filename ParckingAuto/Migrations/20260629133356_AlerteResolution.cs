using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParckingAuto.Migrations
{
    /// <inheritdoc />
    public partial class AlerteResolution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DernierKmVidange",
                table: "Vehicules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateResolution",
                table: "Alertes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceDeclencheur",
                table: "Alertes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DernierKmVidange",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "DateResolution",
                table: "Alertes");

            migrationBuilder.DropColumn(
                name: "ReferenceDeclencheur",
                table: "Alertes");
        }
    }
}
