using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.EntityFramework.Migrations
{
    public partial class Raname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogStories_Users_AutorId",
                table: "BlogStories");

            migrationBuilder.RenameColumn(
                name: "AutorId",
                table: "BlogStories",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogStories_AutorId",
                table: "BlogStories",
                newName: "IX_BlogStories_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogStories_Users_AuthorId",
                table: "BlogStories",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogStories_Users_AuthorId",
                table: "BlogStories");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "BlogStories",
                newName: "AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogStories_AuthorId",
                table: "BlogStories",
                newName: "IX_BlogStories_AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogStories_Users_AutorId",
                table: "BlogStories",
                column: "AutorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
