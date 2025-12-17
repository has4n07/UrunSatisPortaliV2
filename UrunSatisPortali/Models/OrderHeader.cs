using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrunSatisPortali.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public decimal OrderTotal { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }

        [Required(ErrorMessage = "Telefon numarası gereklidir.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Adres gereklidir.")]
        public string StreetAddress { get; set; }
        [Required(ErrorMessage = "Şehir gereklidir.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Bölge/Eyalet gereklidir.")]
        public string State { get; set; }
        [Required(ErrorMessage = "Posta Kodu gereklidir.")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Ad Soyad gereklidir.")]
        public string Name { get; set; }
    }
}
