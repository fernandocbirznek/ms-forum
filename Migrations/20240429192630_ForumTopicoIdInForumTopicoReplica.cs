using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms_forum.Migrations
{
    /// <inheritdoc />
    public partial class ForumTopicoIdInForumTopicoReplica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ForumTopicoId",
                table: "ForumTopicoReplica",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForumTopicoId",
                table: "ForumTopicoReplica");
        }
    }
}
