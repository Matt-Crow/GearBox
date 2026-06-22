using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameWeaponToManipulator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "equipped_weapon_name",
                table: "gb_player_character",
                newName: "equipped_manipulator_name");

            migrationBuilder.RenameColumn(
                name: "equipped_weapon_level",
                table: "gb_player_character",
                newName: "equipped_manipulator_level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "equipped_manipulator_name",
                table: "gb_player_character",
                newName: "equipped_weapon_name");

            migrationBuilder.RenameColumn(
                name: "equipped_manipulator_level",
                table: "gb_player_character",
                newName: "equipped_weapon_level");
        }
    }
}
