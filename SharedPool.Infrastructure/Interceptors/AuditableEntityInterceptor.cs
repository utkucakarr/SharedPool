using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedPool.Domain.Entities;

namespace SharedPool.Infrastructure.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditableEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateAuditableEntities(DbContext? context)
        {
            if (context == null) return;

            var entries = context.ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                // Eğer kayıt yeni ekleniyorsa veya güncelleniyorsa
                if (entry.State == EntityState.Added)
                {
                    // CreatedDate zaten BaseEntity constructor'ında atanıyor ama garantilemek istersen buraya da yazabilirsin.
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Sadece güncellenen kayıtların UpdatedDate'ini değiştir.
                    entry.Property(x => x.UpdatedDate).CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // İleride Soft Delete yaparsak diye bir hazırlık:
                    // Veriyi gerçekten silmek yerine IsDeleted = true yapıp DeletedDate atayabiliriz.
                    // Şimdilik standart bırakıyoruz.
                }
            }
        }
    }
}
