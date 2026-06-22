using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameEquipmentToPartMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "equipment_name",
                table: "gb_player_character_equipment_slot",
                newName: "part_name");

            migrationBuilder.RenameColumn(
                name: "equipment_level",
                table: "gb_player_character_equipment_slot",
                newName: "part_level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "part_name",
                table: "gb_player_character_equipment_slot",
                newName: "equipment_name");

            migrationBuilder.RenameColumn(
                name: "part_level",
                table: "gb_player_character_equipment_slot",
                newName: "equipment_level");
        }
    }
}
