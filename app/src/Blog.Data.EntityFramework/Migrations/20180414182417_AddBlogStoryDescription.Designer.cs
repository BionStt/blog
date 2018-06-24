﻿// <auto-generated />
using Blog.Core.Enums;
using Blog.Data.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Blog.Data.EntityFramework.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20180414182417_AddBlogStoryDescription")]
    partial class AddBlogStoryDescription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Blog.Core.Entities.BlogStory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken")
                        .HasMaxLength(6);

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int?>("CategoryId");

                    b.Property<int>("CommentsCount");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Description");

                    b.Property<bool>("IsPublished");

                    b.Property<string>("Keywords");

                    b.Property<int>("Language");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<DateTime?>("PublishedDate");

                    b.Property<string>("StoryImageUrl");

                    b.Property<string>("StoryThumbUrl");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Alias")
                        .IsUnique()
                        .HasName("AliasIndex");

                    b.ToTable("BlogStories");
                });

            modelBuilder.Entity("Blog.Core.Entities.BlogStoryTag", b =>
                {
                    b.Property<int>("BlogStoryId");

                    b.Property<int>("TagId");

                    b.HasKey("BlogStoryId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("BlogStoryTag");
                });

            modelBuilder.Entity("Blog.Core.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<bool>("IsPublished");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("SeoDescription");

                    b.Property<string>("SeoTitle");

                    b.HasKey("Id");

                    b.HasIndex("Alias")
                        .IsUnique()
                        .HasName("TagSlug");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Blog.Core.Entities.BlogStoryTag", b =>
                {
                    b.HasOne("Blog.Core.Entities.BlogStory", "BlogStory")
                        .WithMany("BlogStoryTags")
                        .HasForeignKey("BlogStoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Blog.Core.Entities.Tag", "Tag")
                        .WithMany("BlogStoryTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
