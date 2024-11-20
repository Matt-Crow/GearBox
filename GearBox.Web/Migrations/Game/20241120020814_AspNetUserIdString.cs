using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearBox.Web.Migrations.Game
{
    /// <inheritdoc />
    public partial class AspNetUserIdString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "asp_net_user_id",
                table: "gb_player_character",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "asp_net_user_id",
                table: "gb_player_character",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
