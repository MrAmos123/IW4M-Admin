﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations.Sqlite
{
    public partial class AddAdditionalClientStatsAndZScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EFClientStatistics",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZScore",
                table: "EFClientStatistics",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "EFHitLocations",
                columns: table => new
                {
                    HitLocationId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Game = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFHitLocations", x => x.HitLocationId);
                });

            migrationBuilder.CreateTable(
                name: "EFMaps",
                columns: table => new
                {
                    MapId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Game = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFMaps", x => x.MapId);
                });

            migrationBuilder.CreateTable(
                name: "EFMeansOfDeath",
                columns: table => new
                {
                    MeansOfDeathId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Game = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFMeansOfDeath", x => x.MeansOfDeathId);
                });

            migrationBuilder.CreateTable(
                name: "EFWeaponAttachments",
                columns: table => new
                {
                    WeaponAttachmentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Game = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFWeaponAttachments", x => x.WeaponAttachmentId);
                });

            migrationBuilder.CreateTable(
                name: "EFWeapons",
                columns: table => new
                {
                    WeaponId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Game = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFWeapons", x => x.WeaponId);
                });

            migrationBuilder.CreateTable(
                name: "EFWeaponAttachmentCombos",
                columns: table => new
                {
                    WeaponAttachmentComboId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Game = table.Column<int>(nullable: false),
                    Attachment1Id = table.Column<int>(nullable: false),
                    Attachment2Id = table.Column<int>(nullable: true),
                    Attachment3Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFWeaponAttachmentCombos", x => x.WeaponAttachmentComboId);
                    table.ForeignKey(
                        name: "FK_EFWeaponAttachmentCombos_EFWeaponAttachments_Attachment1Id",
                        column: x => x.Attachment1Id,
                        principalTable: "EFWeaponAttachments",
                        principalColumn: "WeaponAttachmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EFWeaponAttachmentCombos_EFWeaponAttachments_Attachment2Id",
                        column: x => x.Attachment2Id,
                        principalTable: "EFWeaponAttachments",
                        principalColumn: "WeaponAttachmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EFWeaponAttachmentCombos_EFWeaponAttachments_Attachment3Id",
                        column: x => x.Attachment3Id,
                        principalTable: "EFWeaponAttachments",
                        principalColumn: "WeaponAttachmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EFClientHitStatistics",
                columns: table => new
                {
                    ClientHitStatisticId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    ServerId = table.Column<long>(nullable: true),
                    HitLocationId = table.Column<int>(nullable: true),
                    MeansOfDeathId = table.Column<int>(nullable: true),
                    WeaponId = table.Column<int>(nullable: true),
                    WeaponAttachmentComboId = table.Column<int>(nullable: true),
                    HitCount = table.Column<int>(nullable: false),
                    KillCount = table.Column<int>(nullable: false),
                    DamageInflicted = table.Column<int>(nullable: false),
                    ReceivedHitCount = table.Column<int>(nullable: false),
                    DeathCount = table.Column<int>(nullable: false),
                    DamageReceived = table.Column<int>(nullable: false),
                    SuicideCount = table.Column<int>(nullable: false),
                    UsageSeconds = table.Column<int>(nullable: true),
                    Score = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFClientHitStatistics", x => x.ClientHitStatisticId);
                    table.ForeignKey(
                        name: "FK_EFClientHitStatistics_EFClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "EFClients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EFClientHitStatistics_EFHitLocations_HitLocationId",
                        column: x => x.HitLocationId,
                        principalTable: "EFHitLocations",
                        principalColumn: "HitLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EFClientHitStatistics_EFMeansOfDeath_MeansOfDeathId",
                        column: x => x.MeansOfDeathId,
                        principalTable: "EFMeansOfDeath",
                        principalColumn: "MeansOfDeathId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EFClientHitStatistics_EFServers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "EFServers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EFClientHitStatistics_EFWeaponAttachmentCombos_WeaponAttachmentComboId",
                        column: x => x.WeaponAttachmentComboId,
                        principalTable: "EFWeaponAttachmentCombos",
                        principalColumn: "WeaponAttachmentComboId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EFClientHitStatistics_EFWeapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "EFWeapons",
                        principalColumn: "WeaponId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EFClientStatistics_ZScore",
                table: "EFClientStatistics",
                column: "ZScore");

            migrationBuilder.CreateIndex(
                name: "IX_EFClientStatistics_ClientId_TimePlayed_ZScore",
                table: "EFClientStatistics",
                columns: new[] { "ClientId", "TimePlayed", "ZScore" });

            migrationBuilder.CreateIndex(
                name: "IX_EFClientHitStatistics_ClientId",
                table: "EFClientHitStatistics",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EFClientHitStatistics_HitLocationId",
                table: "EFClientHitStatistics",
                column: "HitLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EFClientHitStatistics_MeansOfDeathId",
                table: "EFClientHitStatistics",
                column: "MeansOfDeathId");

            migrationBuilder.CreateIndex(
                name: "IX_EFClientHitStatistics_ServerId",
                table: "EFClientHitStatistics",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_EFClientHitStatistics_WeaponAttachmentComboId",
                table: "EFClientHitStatistics",
                column: "WeaponAttachmentComboId");

            migrationBuilder.CreateIndex(
                name: "IX_EFClientHitStatistics_WeaponId",
                table: "EFClientHitStatistics",
                column: "WeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_EFHitLocations_Name",
                table: "EFHitLocations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_EFWeaponAttachmentCombos_Attachment1Id",
                table: "EFWeaponAttachmentCombos",
                column: "Attachment1Id");

            migrationBuilder.CreateIndex(
                name: "IX_EFWeaponAttachmentCombos_Attachment2Id",
                table: "EFWeaponAttachmentCombos",
                column: "Attachment2Id");

            migrationBuilder.CreateIndex(
                name: "IX_EFWeaponAttachmentCombos_Attachment3Id",
                table: "EFWeaponAttachmentCombos",
                column: "Attachment3Id");

            migrationBuilder.CreateIndex(
                name: "IX_EFWeapons_Name",
                table: "EFWeapons",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
