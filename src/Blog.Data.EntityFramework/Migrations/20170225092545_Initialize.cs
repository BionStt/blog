using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Blog.Data.EntityFramework.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Int32>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<String>(maxLength: 256, nullable: false),
                    Slug = table.Column<String>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Int32>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<String>(maxLength: 256, nullable: false),
                    Slug = table.Column<String>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Int32>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccessFailedCount = table.Column<Int32>(nullable: false),
                    ConcurrencyStamp = table.Column<String>(nullable: true),
                    Email = table.Column<String>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<Boolean>(nullable: false),
                    LockoutEnabled = table.Column<Boolean>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<String>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<String>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<String>(nullable: true),
                    PhoneNumber = table.Column<String>(nullable: true),
                    PhoneNumberConfirmed = table.Column<Boolean>(nullable: false),
                    SecurityStamp = table.Column<String>(nullable: true),
                    TwoFactorEnabled = table.Column<Boolean>(nullable: false),
                    UserName = table.Column<String>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Int32>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<String>(nullable: true),
                    Name = table.Column<String>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<String>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogStories",
                columns: table => new
                {
                    Id = table.Column<Int32>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AutorId = table.Column<Int32>(nullable: false),
                    CommentsCount = table.Column<Int32>(nullable: false),
                    Content = table.Column<String>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsPublished = table.Column<Boolean>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Slug = table.Column<String>(maxLength: 256, nullable: false),
                    StoryImageUrl = table.Column<String>(nullable: true),
                    StorySrcUrl = table.Column<String>(nullable: true),
                    StoryThumbUrl = table.Column<String>(nullable: true),
                    Title = table.Column<String>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogStories_Users_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<Int32>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<String>(nullable: true),
                    ClaimValue = table.Column<String>(nullable: true),
                    UserId = table.Column<Int32>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<String>(nullable: false),
                    ProviderKey = table.Column<String>(nullable: false),
                    ProviderDisplayName = table.Column<String>(nullable: true),
                    UserId = table.Column<Int32>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Int32>(nullable: false),
                    LoginProvider = table.Column<String>(nullable: false),
                    Name = table.Column<String>(nullable: false),
                    Value = table.Column<String>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<Int32>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<String>(nullable: true),
                    ClaimValue = table.Column<String>(nullable: true),
                    RoleId = table.Column<Int32>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Int32>(nullable: false),
                    RoleId = table.Column<Int32>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogStoryCategory",
                columns: table => new
                {
                    BlogStoryId = table.Column<Int32>(nullable: false),
                    CategoryId = table.Column<Int32>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "BlogStoryTag",
                columns: table => new
                {
                    BlogStoryId = table.Column<Int32>(nullable: false),
                    TagId = table.Column<Int32>(nullable: false)
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
                name: "IX_BlogStories_AutorId",
                table: "BlogStories",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "SlugIndex",
                table: "BlogStories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogStoryCategory_CategoryId",
                table: "BlogStoryCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogStoryTag_TagId",
                table: "BlogStoryTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "CategorySlug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "TagSlug",
                table: "Tags",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogStoryCategory");

            migrationBuilder.DropTable(
                name: "BlogStoryTag");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "BlogStories");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
