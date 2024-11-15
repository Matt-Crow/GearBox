using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations.Game
{
    /// <inheritdoc />
    public partial class GameInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gb_player_character",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asp_net_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    xp = table.Column<int>(type: "integer", nullable: false),
                    gold = table.Column<int>(type: "integer", nullable: false),
                    equipped_weapon_id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipped_armor_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gb_player_character", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gb_player_character");
        }
    }
}
