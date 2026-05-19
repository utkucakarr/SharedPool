namespace SharedPool.Domain.Models
{
    // Equal (Eşit) için sadece UserId yeterli (Value önemsiz).
    // Percentage (Yüzde) için Value yüzdelik dilimi (örn: 40) ifade eder.
    // ExactAmount (Tam Tutar) için Value direkt tutarı (örn: 150 TL) ifade eder.
    public record SplitDetailModel(Guid UserId, decimal Value);
}
