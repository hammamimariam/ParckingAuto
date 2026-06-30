using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParckingAuto.Migrations
{
    /// <inheritdoc />
    public partial class MaintenanceKilometrage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KilometrageIntervention",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KilometrageIntervention",
                table: "Maintenances");
        }
    }
}
