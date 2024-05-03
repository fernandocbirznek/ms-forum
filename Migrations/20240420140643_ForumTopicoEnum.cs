using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms_forum.Migrations
{
    /// <inheritdoc />
    public partial class ForumTopicoEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForumTopicoEnum",
                table: "ForumTopico",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForumTopicoEnum",
                table: "ForumTopico");
        }
    }
}
