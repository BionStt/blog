using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.EntityFramework.Migrations
{
    public partial class RenameSlugToAlias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "BlogStories",
                newName: "Alias");

            migrationBuilder.RenameIndex(
                name: "SlugIndex",
                table: "BlogStories",
                newName: "AliasIndex");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Alias",
                table: "BlogStories",
                newName: "Slug");

            migrationBuilder.RenameIndex(
                name: "AliasIndex",
                table: "BlogStories",
                newName: "SlugIndex");
        }
    }
}
