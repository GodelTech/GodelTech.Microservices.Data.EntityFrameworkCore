using System;
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
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            // Table
            builder.ToTable("Bank", SchemaName);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
            builder.Property(x => x.Name).HasColumnType("nvarchar(256)").IsRequired();
        }
    }
}
