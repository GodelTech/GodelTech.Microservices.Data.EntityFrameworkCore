﻿using System;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Configurations
{
    public class BankConfiguration : EntityTypeConfiguration<BankEntity, Guid>
    {
        public BankConfiguration(string schemaName)
            : base(schemaName)
        {

        }

        public override void Configure(EntityTypeBuilder<BankEntity> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            // Table
            builder.ToTable("Bank", SchemaName);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()");
            builder.Property(x => x.Name).HasColumnType("nvarchar(255)").IsRequired();
        }
    }
}
