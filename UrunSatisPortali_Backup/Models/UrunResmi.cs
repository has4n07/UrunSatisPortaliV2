namespace UrunSatisPortali.Models
{
    public class UrunResmi
    {
        public int Id { get; set; } // Resmin kendi ID'si

        // "/uploads/laptop-ust.jpg" gibi dosya yolunu tutacak
        public string ResimYolu { get; set; } = "";

        // --- İlişki (Relationship) ---
        // Bu resim hangi ürüne ait?
        public int UrunId { get; set; } // Foreign Key (Yabancı Anahtar)
        public Urun? Urun { get; set; } // Navigation Property (Urun'e bağlantı)
    }
}