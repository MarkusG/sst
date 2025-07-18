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

                    b.Property<int?>("ItemId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlaidId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("PlaidId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Sst.Database.Entities.CashFlowTreeEntry", b =>
                {
                    b.Property<decimal>("CategoryTotal")
                        .HasColumnType("numeric");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TreeTotal")
                        .HasColumnType("numeric");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.ToTable((string)null);

                    b.ToSqlQuery("   with recursive\n       categories as (select \"Id\", \"Name\", 0 as \"Level\", \"Position\", \"ParentId\", array [\"Position\"] as \"Path\"\n                      from \"Categories\"\n                      where \"ParentId\" is null\n   \n                      union all\n   \n                      select c.\"Id\",\n                             c.\"Name\",\n                             \"Level\" + 1,\n                             c.\"Position\",\n                             c.\"ParentId\",\n                             array_append(\"Path\", c.\"Position\") as Path\n                      from \"Categories\" c\n                               inner join categories cats on cats.\"Id\" = c.\"ParentId\"),\n       ancestry as (select \"Id\", \"Name\", \"ParentId\", \"Id\" as \"AncestorId\"\n                    from \"Categories\"\n   \n                    union all\n   \n                    select c.\"Id\", a.\"Name\", c.\"ParentId\", \"AncestorId\"\n                    from \"Categories\" c\n                             inner join ancestry a on a.\"Id\" = c.\"ParentId\"),\n       totals as (select C.\"Id\",\n                         C.\"Name\",\n                         C.\"ParentId\",\n                         extract(year from \"Timestamp\")  as \"Year\",\n                         extract(month from \"Timestamp\") as \"Month\",\n                         sum(CZ.\"Amount\")                 as \"Total\"\n                  from \"Categories\" C\n                           left join \"Categorizations\" CZ on C.\"Id\" = CZ.\"CategoryId\"\n                           left join \"Transactions\" T on CZ.\"TransactionId\" = T.\"Id\"\n                  group by C.\"Id\", C.\"Name\", extract(year from \"Timestamp\"), extract(month from \"Timestamp\")),\n       categoryTotals as (select a.\"AncestorId\" as \"CategoryId\", t.\"Year\", t.\"Month\", sum(t.\"Total\") as \"Total\"\n                          from ancestry as a\n                                   inner join totals t on t.\"Id\" = a.\"Id\"\n                          group by a.\"AncestorId\", t.\"Year\", t.\"Month\")\n   select ct.\"CategoryId\" as \"Id\", c.\"Name\", c.\"Level\", ct.\"Total\" as \"TreeTotal\", coalesce(t.\"Total\", 0) as \"CategoryTotal\", ct.\"Year\", ct.\"Month\"\n   from categoryTotals ct\n            inner join categories c on c.\"Id\" = ct.\"CategoryId\"\n            left join totals t on t.\"Id\" = ct.\"CategoryId\" and t.\"Year\" = ct.\"Year\" and t.\"Month\" = ct.\"Month\"\n   where ct.\"Year\" is not null\n     and ct.\"Month\" is not null\n   order by c.\"Path\", \"Year\", \"Month\"");
                });

            modelBuilder.Entity("Sst.Database.Entities.Categorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int>("TransactionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("TransactionId");

                    b.ToTable("Categorizations");
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

            modelBuilder.Entity("Sst.Database.Entities.CategoryTreeEntry", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.ToSqlQuery("   select * from (with recursive categories as (\n       select \"Id\", \"Name\", 0 as \"Level\", \"Position\", \"ParentId\", array[\"Position\"] as \"Path\"\n       from \"Categories\"\n       where \"ParentId\" is null\n   \n       union all\n   \n       select c.\"Id\", c.\"Name\", \"Level\" + 1, c.\"Position\", c.\"ParentId\", array_append(\"Path\", c.\"Position\") as Path\n       from \"Categories\" c\n                inner join categories cats on cats.\"Id\" = c.\"ParentId\"\n   )\n   select \"Id\", \"Name\", \"Level\", \"Position\", \"ParentId\" from categories order by \"Path\") as CategoryTreeEntries;");
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

                    b.Property<int?>("AccountId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlaidId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("PlaidId")
                        .IsUnique();

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Sst.Database.Entities.Account", b =>
                {
                    b.HasOne("Sst.Database.Entities.Item", "Item")
                        .WithMany("Accounts")
                        .HasForeignKey("ItemId");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Sst.Database.Entities.Categorization", b =>
                {
                    b.HasOne("Sst.Database.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sst.Database.Entities.Transaction", "Transaction")
                        .WithMany("Categorizations")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Transaction");
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
                    b.HasOne("Sst.Database.Entities.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Sst.Database.Entities.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Sst.Database.Entities.Category", b =>
                {
                    b.Navigation("Subcategories");
                });

            modelBuilder.Entity("Sst.Database.Entities.Item", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("Sst.Database.Entities.Transaction", b =>
                {
                    b.Navigation("Categorizations");
                });
#pragma warning restore 612, 618
        }
    }
}
