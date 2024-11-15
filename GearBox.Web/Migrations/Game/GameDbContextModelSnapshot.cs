﻿// <auto-generated />
using System;
using GearBox.Web.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GearBox.Web.Migrations.Game
{
    [DbContext(typeof(GameDbContext))]
    partial class GameDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GearBox.Web.Database.DbPlayerCharacter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AspNetUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("asp_net_user_id");

                    b.Property<Guid?>("EquippedArmorId")
                        .HasColumnType("uuid")
                        .HasColumnName("equipped_armor_id");

                    b.Property<Guid?>("EquippedWeaponId")
                        .HasColumnType("uuid")
                        .HasColumnName("equipped_weapon_id");

                    b.Property<int>("Gold")
                        .HasColumnType("integer")
                        .HasColumnName("gold");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Xp")
                        .HasColumnType("integer")
                        .HasColumnName("xp");

                    b.HasKey("Id");

                    b.ToTable("gb_player_character");
                });
#pragma warning restore 612, 618
        }
    }
}
