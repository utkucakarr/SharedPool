namespace SharedPool.Application.DTOs
{
    public record GroupBalanceDto(
            Guid OwedByUserId, // Borçlu
            Guid OwedToUserId, // Alacaklı
            decimal Amount     // Tutar
        );
}
