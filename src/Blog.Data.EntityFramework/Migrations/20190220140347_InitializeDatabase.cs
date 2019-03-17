using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.Data.EntityFramework.Migrations
{
    public partial class InitializeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogStories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccessToken = table.Column<string>(maxLength: 6, nullable: true),
                    Alias = table.Column<string>(maxLength: 256, nullable: false),
                    Content = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Language = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    PublishedDate = table.Column<DateTime>(nullable: true),
                    SeoDescription = table.Column<string>(nullable: true),
                    SeoKeywords = table.Column<string>(nullable: true),
                    StoryImageUrl = table.Column<string>(nullable: true),
                    StoryThumbUrl = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogStories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Alias = table.Column<string>(maxLength: 256, nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Score = table.Column<int>(nullable: false),
                    SeoDescription = table.Column<string>(nullable: true),
                    SeoKeywords = table.Column<string>(nullable: true),
                    SeoTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogStoryTag",
                columns: table => new
                {
                    BlogStoryId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogStoryTag", x => new { x.BlogStoryId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BlogStoryTag_BlogStories_BlogStoryId",
                        column: x => x.BlogStoryId,
                        principalTable: "BlogStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogStoryTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "AliasIndex",
                table: "BlogStories",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogStoryTag_TagId",
                table: "BlogStoryTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "TagSlug",
                table: "Tags",
                column: "Alias",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogStoryTag");

            migrationBuilder.DropTable(
                name: "BlogStories");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
