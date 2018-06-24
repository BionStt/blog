using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.EntityFramework.Migrations
{
    public partial class RenameSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Tags",
                newName: "Alias");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Categories",
                newName: "Alias");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Alias",
                table: "Tags",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "Alias",
                table: "Categories",
                newName: "Slug");
        }
    }
}
