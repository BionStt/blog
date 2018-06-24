using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.EntityFramework.Migrations
{
    public partial class ChangeCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogStoryCategory");

            migrationBuilder.DropColumn(
                name: "StorySrcUrl",
                table: "BlogStories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "BlogStories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BlogStories_CategoryId",
                table: "BlogStories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogStories_Categories_CategoryId",
                table: "BlogStories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogStories_Categories_CategoryId",
                table: "BlogStories");

            migrationBuilder.DropIndex(
                name: "IX_BlogStories_CategoryId",
                table: "BlogStories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BlogStories");

            migrationBuilder.AddColumn<string>(
                name: "StorySrcUrl",
                table: "BlogStories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlogStoryCategory",
                columns: table => new
                {
                    BlogStoryId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogStoryCategory", x => new { x.BlogStoryId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_BlogStoryCategory_BlogStories_BlogStoryId",
                        column: x => x.BlogStoryId,
                        principalTable: "BlogStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogStoryCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogStoryCategory_CategoryId",
                table: "BlogStoryCategory",
                column: "CategoryId");
        }
    }
}
