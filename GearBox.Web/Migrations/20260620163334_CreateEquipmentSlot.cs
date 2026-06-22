using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations
{
    /// <inheritdoc />
    public partial class CreateEquipmentSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gb_player_character_equipment_slot",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_character_id = table.Column<Guid>(type: "uuid", nullable: false),
                    slot_type = table.Column<string>(type: "text", nullable: false),
                    equipment_name = table.Column<string>(type: "text", nullable: true),
                    equipment_level = table.Column<int>(type: "integer", nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gb_player_character_equipment_slot");
        }
    }
}
