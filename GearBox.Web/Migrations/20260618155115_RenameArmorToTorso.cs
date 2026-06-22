using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameArmorToTorso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "equipped_armor_name",
                table: "gb_player_character",
                newName: "equipped_torso_name");

            migrationBuilder.RenameColumn(
                name: "equipped_armor_level",
                table: "gb_player_character",
                newName: "equipped_torso_level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "equipped_torso_name",
                table: "gb_player_character",
                newName: "equipped_armor_name");

            migrationBuilder.RenameColumn(
                name: "equipped_torso_level",
                table: "gb_player_character",
                newName: "equipped_armor_level");
        }
    }
}
