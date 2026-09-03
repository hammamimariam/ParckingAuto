using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParckingAuto.Migrations
{
    /// <inheritdoc />
    public partial class AddTunisianCarteGriseFullFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PuissanceFiscale",
                table: "Vehicules",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Carrosserie",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "ChargeUtile",
                table: "Vehicules",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Cylindree",
                table: "Vehicules",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEtablissementCarteGrise",
                table: "Vehicules",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImmatriculationPrecedente",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LieuEtablissementCarteGrise",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "NombreEssieux",
                table: "Vehicules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NombrePlacesDebout",
                table: "Vehicules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NumeroSerieType",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "PTAC",
                table: "Vehicules",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Restrictions",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TypeCommercial",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TypeConstructeur",
                table: "Vehicules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carrosserie",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "ChargeUtile",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "Cylindree",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "DateEtablissementCarteGrise",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "ImmatriculationPrecedente",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "LieuEtablissementCarteGrise",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "NombreEssieux",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "NombrePlacesDebout",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "NumeroSerieType",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "PTAC",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "Restrictions",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "TypeCommercial",
                table: "Vehicules");

            migrationBuilder.DropColumn(
                name: "TypeConstructeur",
                table: "Vehicules");

            migrationBuilder.AlterColumn<int>(
                name: "PuissanceFiscale",
                table: "Vehicules",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }
    }
}
