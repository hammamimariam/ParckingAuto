using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParckingAuto.Migrations
{
    /// <inheritdoc />
    public partial class ChauffeurRelationsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chauffeurs_Utilisateurs_UtilisateurId",
                table: "Chauffeurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Mouvements_Chauffeurs_ChauffeurId",
                table: "Mouvements");

            migrationBuilder.DropForeignKey(
                name: "FK_Mouvements_Vehicules_VehiculeId",
                table: "Mouvements");

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                table: "Chauffeurs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Chauffeurs_Utilisateurs_UtilisateurId",
                table: "Chauffeurs",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Mouvements_Chauffeurs_ChauffeurId",
                table: "Mouvements",
                column: "ChauffeurId",
                principalTable: "Chauffeurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mouvements_Vehicules_VehiculeId",
                table: "Mouvements",
                column: "VehiculeId",
                principalTable: "Vehicules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chauffeurs_Utilisateurs_UtilisateurId",
                table: "Chauffeurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Mouvements_Chauffeurs_ChauffeurId",
                table: "Mouvements");

            migrationBuilder.DropForeignKey(
                name: "FK_Mouvements_Vehicules_VehiculeId",
                table: "Mouvements");

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                table: "Chauffeurs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chauffeurs_Utilisateurs_UtilisateurId",
                table: "Chauffeurs",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mouvements_Chauffeurs_ChauffeurId",
                table: "Mouvements",
                column: "ChauffeurId",
                principalTable: "Chauffeurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mouvements_Vehicules_VehiculeId",
                table: "Mouvements",
                column: "VehiculeId",
                principalTable: "Vehicules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
