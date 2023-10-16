using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ms_forum.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forum",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumTag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopico",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    ForumId = table.Column<long>(type: "bigint", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumTopico_Forum_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Forum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopicoResposta",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    ForumTopicoId = table.Column<long>(type: "bigint", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopicoResposta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumTopicoResposta_ForumTopico_ForumTopicoId",
                        column: x => x.ForumTopicoId,
                        principalTable: "ForumTopico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopicoTag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ForumTagId = table.Column<long>(type: "bigint", nullable: false),
                    ForumTopicoId = table.Column<long>(type: "bigint", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopicoTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumTopicoTag_ForumTag_ForumTagId",
                        column: x => x.ForumTagId,
                        principalTable: "ForumTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumTopicoTag_ForumTopico_ForumTopicoId",
                        column: x => x.ForumTopicoId,
                        principalTable: "ForumTopico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopicoReplica",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    ForumTopicoRespostaId = table.Column<long>(type: "bigint", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopicoReplica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumTopicoReplica_ForumTopicoResposta_ForumTopicoRespostaId",
                        column: x => x.ForumTopicoRespostaId,
                        principalTable: "ForumTopicoResposta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopico_ForumId",
                table: "ForumTopico",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopicoReplica_ForumTopicoRespostaId",
                table: "ForumTopicoReplica",
                column: "ForumTopicoRespostaId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopicoResposta_ForumTopicoId",
                table: "ForumTopicoResposta",
                column: "ForumTopicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopicoTag_ForumTagId",
                table: "ForumTopicoTag",
                column: "ForumTagId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopicoTag_ForumTopicoId",
                table: "ForumTopicoTag",
                column: "ForumTopicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumTopicoReplica");

            migrationBuilder.DropTable(
                name: "ForumTopicoTag");

            migrationBuilder.DropTable(
                name: "ForumTopicoResposta");

            migrationBuilder.DropTable(
                name: "ForumTag");

            migrationBuilder.DropTable(
                name: "ForumTopico");

            migrationBuilder.DropTable(
                name: "Forum");
        }
    }
}
