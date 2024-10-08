﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sst.Database;

#nullable disable

namespace Sst.Database.Migrations
{
    [DbContext(typeof(SstDbContext))]
    partial class SstDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.1.24451.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Sst.Database.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("AvailableBalance")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)");

                    b.Property<decimal?>("CurrentBalance")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlaidId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("PlaidId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Sst.Database.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Sst.Database.Entities.CategoryMonthTotal", b =>
                {
                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<decimal>("Total")
                        .HasColumnType("numeric");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.ToTable((string)null);

                    b.ToSqlQuery("   select c.\"Name\" as \"Category\", extract(year from \"Timestamp\") as \"Year\", extract(month from \"Timestamp\") as \"Month\", sum(t.\"Amount\") as \"Total\"\n   from \"Transactions\" t\n            inner join \"Categories\" as c on c.\"Id\" = t.\"CategoryId\"\n   where t.\"CategoryId\" is not null\n   group by c.\"Name\", extract(year from \"Timestamp\"), extract(month from \"Timestamp\")");
                });

            modelBuilder.Entity("Sst.Database.Entities.CategoryTreeEntry", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int[]>("Path")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int?>("SuperCategoryId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.ToSqlQuery("   select * from (with recursive categories as (\n       select \"Id\", \"Name\", 0 as \"Level\", \"Position\", \"SuperCategoryId\", array[\"Position\"] as \"Path\"\n       from \"Categories\"\n       where \"SuperCategoryId\" is null\n   \n       union all\n   \n       select c.\"Id\", c.\"Name\", \"Level\" + 1, c.\"Position\", c.\"SuperCategoryId\", array_append(\"Path\", c.\"Position\") as Path\n       from \"Categories\" c\n                inner join categories cats on cats.\"Id\" = c.\"SuperCategoryId\"\n   )\n   select * from categories order by \"Path\", \"Position\") as CategoryTreeEntries;");
                });

            modelBuilder.Entity("Sst.Database.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NextCursor")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Sst.Database.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountMask")
                        .HasColumnType("text");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Amount")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlaidId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PlaidId")
                        .IsUnique();

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Sst.Database.Entities.Account", b =>
                {
                    b.HasOne("Sst.Database.Entities.Item", "Item")
                        .WithMany("Accounts")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Sst.Database.Entities.Category", b =>
                {
                    b.HasOne("Sst.Database.Entities.Category", "ParentCategory")
                        .WithMany("Subcategories")
                        .HasForeignKey("ParentId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("Sst.Database.Entities.Transaction", b =>
                {
                    b.HasOne("Sst.Database.Entities.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Sst.Database.Entities.Category", b =>
                {
                    b.Navigation("Subcategories");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Sst.Database.Entities.Item", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
