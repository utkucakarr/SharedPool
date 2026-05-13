using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedPool.Domain.Entities;

namespace SharedPool.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            // Email adresi benzersiz (unique) olmalı
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.PasswordHash)
                .IsRequired() // Şifresiz kullanıcı olamaz
                .HasMaxLength(500); // Hash algoritmalarına (BCrypt, Argon2) göre yeterli uzunluk

            builder.ToTable("Users");
        }
    }
}
