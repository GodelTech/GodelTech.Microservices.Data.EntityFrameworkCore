using System;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Configurations
{
    public class CurrencyConfiguration : EntityTypeConfiguration<CurrencyEntity, int>
    {
        public CurrencyConfiguration(string schemaName)
            : base(schemaName)
        {

        }

        public override void Configure(EntityTypeBuilder<CurrencyEntity> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            // Table
            builder.ToTable("Currency", SchemaName);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.AlphabeticCode).HasColumnType("nvarchar(3)").IsRequired();
        }
    }
}
