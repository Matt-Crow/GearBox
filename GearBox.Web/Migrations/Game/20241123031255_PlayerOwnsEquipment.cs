using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations.Game
{
    /// <inheritdoc />
    public partial class PlayerOwnsEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "equipped_armor_id",
                table: "gb_player_character");

            migrationBuilder.DropColumn(
                name: "equipped_weapon_id",
                table: "gb_player_character");

            migrationBuilder.AddColumn<int>(
                name: "equipped_armor_level",
                table: "gb_player_character",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "equipped_armor_name",
                table: "gb_player_character",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "equipped_weapon_level",
                table: "gb_player_character",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "equipped_weapon_name",
                table: "gb_player_character",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "equipped_armor_level",
                table: "gb_player_character");

            migrationBuilder.DropColumn(
                name: "equipped_armor_name",
                table: "gb_player_character");

            migrationBuilder.DropColumn(
                name: "equipped_weapon_level",
                table: "gb_player_character");

            migrationBuilder.DropColumn(
                name: "equipped_weapon_name",
                table: "gb_player_character");

            migrationBuilder.AddColumn<Guid>(
                name: "equipped_armor_id",
                table: "gb_player_character",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipped_weapon_id",
                table: "gb_player_character",
                type: "uuid",
                nullable: true);
        }
    }
}
