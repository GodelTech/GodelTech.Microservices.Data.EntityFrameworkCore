﻿// <auto-generated />
using System;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Migrations
{
    [DbContext(typeof(CurrencyExchangeRateDbContext))]
    partial class CurrencyExchangeRateDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities.BankEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Bank", "currencyExchangeRate");
                });

            modelBuilder.Entity("GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities.CurrencyEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("AlphabeticCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)");

                    b.HasKey("Id");

                    b.ToTable("Currency", "currencyExchangeRate");
                });
#pragma warning restore 612, 618
        }
    }
}
