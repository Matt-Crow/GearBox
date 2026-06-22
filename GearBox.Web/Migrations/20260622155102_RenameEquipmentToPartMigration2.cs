using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameEquipmentToPartMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gb_player_character_equipment_slot");

            migrationBuilder.CreateTable(
                name: "gb_player_character_part_slot",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_character_id = table.Column<Guid>(type: "uuid", nullable: false),
                    slot_type = table.Column<string>(type: "text", nullable: false),
                    part_name = table.Column<string>(type: "text", nullable: true),
                    part_level = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gb_player_character_part_slot", x => x.id);
                    table.ForeignKey(
                        name: "FK_gb_player_character_part_slot_gb_player_character_player_ch~",
                        column: x => x.player_character_id,
                        principalTable: "gb_player_character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gb_player_character_part_slot_player_character_id_slot_type",
                table: "gb_player_character_part_slot",
                columns: new[] { "player_character_id", "slot_type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gb_player_character_part_slot");

            migrationBuilder.CreateTable(
                name: "gb_player_character_equipment_slot",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_character_id = table.Column<Guid>(type: "uuid", nullable: false),
                    part_level = table.Column<int>(type: "integer", nullable: true),
                    part_name = table.Column<string>(type: "text", nullable: true),
                    slot_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gb_player_character_equipment_slot", x => x.id);
                    table.ForeignKey(
                        name: "FK_gb_player_character_equipment_slot_gb_player_character_play~",
                        column: x => x.player_character_id,
                        principalTable: "gb_player_character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gb_player_character_equipment_slot_player_character_id_slot~",
                table: "gb_player_character_equipment_slot",
                columns: new[] { "player_character_id", "slot_type" },
                unique: true);
        }
    }
}
