﻿// <auto-generated />
using System;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Migrations
{
    [DbContext(typeof(CurrencyExchangeRateDbContext))]
    [Migration("20220728074549_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities.BankEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Bank", "currencyExchangeRate");
                });

            modelBuilder.Entity("GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities.CurrencyEntity", b =>
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
