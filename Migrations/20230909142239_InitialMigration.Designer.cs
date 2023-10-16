﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ms_forum;

#nullable disable

namespace ms_forum.Migrations
{
    [DbContext(typeof(ForumDbContext))]
    [Migration("20230909142239_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ms_forum.Domains.Forum", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Forum");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ForumTag");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopico", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("ForumId")
                        .HasColumnType("bigint");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("UsuarioId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ForumId");

                    b.ToTable("ForumTopico");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopicoReplica", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("ForumTopicoRespostaId")
                        .HasColumnType("bigint");

                    b.Property<long>("UsuarioId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ForumTopicoRespostaId");

                    b.ToTable("ForumTopicoReplica");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopicoResposta", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("ForumTopicoId")
                        .HasColumnType("bigint");

                    b.Property<long>("UsuarioId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ForumTopicoId");

                    b.ToTable("ForumTopicoResposta");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopicoTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("ForumTagId")
                        .HasColumnType("bigint");

                    b.Property<long>("ForumTopicoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ForumTagId");

                    b.HasIndex("ForumTopicoId");

                    b.ToTable("ForumTopicoTag");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopico", b =>
                {
                    b.HasOne("ms_forum.Domains.Forum", "Forum")
                        .WithMany("ForumTopicos")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forum");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopicoReplica", b =>
                {
                    b.HasOne("ms_forum.Domains.ForumTopicoResposta", "ForumTopicoResposta")
                        .WithMany("Replicas")
                        .HasForeignKey("ForumTopicoRespostaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ForumTopicoResposta");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopicoResposta", b =>
                {
                    b.HasOne("ms_forum.Domains.ForumTopico", "ForumTopico")
                        .WithMany("Respostas")
                        .HasForeignKey("ForumTopicoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ForumTopico");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopicoTag", b =>
                {
                    b.HasOne("ms_forum.Domains.ForumTag", "ForumTag")
                        .WithMany("ForumTopicoTags")
                        .HasForeignKey("ForumTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ms_forum.Domains.ForumTopico", "ForumTopico")
                        .WithMany("ForumTopicoTags")
                        .HasForeignKey("ForumTopicoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ForumTag");

                    b.Navigation("ForumTopico");
                });

            modelBuilder.Entity("ms_forum.Domains.Forum", b =>
                {
                    b.Navigation("ForumTopicos");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTag", b =>
                {
                    b.Navigation("ForumTopicoTags");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopico", b =>
                {
                    b.Navigation("ForumTopicoTags");

                    b.Navigation("Respostas");
                });

            modelBuilder.Entity("ms_forum.Domains.ForumTopicoResposta", b =>
                {
                    b.Navigation("Replicas");
                });
#pragma warning restore 612, 618
        }
    }
}
