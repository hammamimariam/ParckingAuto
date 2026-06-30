using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParckingAuto.Migrations
{
    /// <inheritdoc />
    public partial class TunisianDocumentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssuranceDateDebut",
                table: "Vehicules",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompagnieAssurance",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Couleur",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePremiereMiseEnCirculation",
                table: "Vehicules",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenreVehicule",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "NombrePlaces",
                table: "Vehicules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NumeroCarteGrise",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PuissanceFiscale",
                table: "Vehicules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsageVehicule",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssuranceDateDebut",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "CompagnieAssurance",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "Couleur",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "DatePremiereMiseEnCirculation",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "GenreVehicule",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "NombrePlaces",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "NumeroCarteGrise",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "PuissanceFiscale",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "UsageVehicule",
                table: "Vehicules");
        }
    }
}
