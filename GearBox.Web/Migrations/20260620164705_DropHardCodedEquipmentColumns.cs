using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations
{
    /// <inheritdoc />
    public partial class DropHardCodedEquipmentColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "equipped_manipulator_level",
                table: "gb_player_character");

            migrationBuilder.DropColumn(
                name: "equipped_manipulator_name",
                table: "gb_player_character");

            migrationBuilder.DropColumn(
                name: "equipped_torso_level",
                table: "gb_player_character");

            migrationBuilder.DropColumn(
                name: "equipped_torso_name",
                table: "gb_player_character");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "equipped_manipulator_level",
                table: "gb_player_character",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "equipped_manipulator_name",
                table: "gb_player_character",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "equipped_torso_level",
                table: "gb_player_character",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "equipped_torso_name",
                table: "gb_player_character",
                type: "text",
                nullable: true);
        }
    }
}
