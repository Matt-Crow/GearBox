using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations
{
    /// <inheritdoc />
    public partial class PlayerToUserFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_gb_player_character_asp_net_user_id",
                table: "gb_player_character",
                column: "asp_net_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_gb_player_character_AspNetUsers_asp_net_user_id",
                table: "gb_player_character",
                column: "asp_net_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gb_player_character_AspNetUsers_asp_net_user_id",
                table: "gb_player_character");

            migrationBuilder.DropIndex(
                name: "IX_gb_player_character_asp_net_user_id",
                table: "gb_player_character");
        }
    }
}
