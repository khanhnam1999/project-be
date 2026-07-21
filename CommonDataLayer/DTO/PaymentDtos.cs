using CommonDataLayer.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CommonDataLayer.DTO
{
    public class PaymentInvoiceDto
    {
        public Guid PaymentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? RoomNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Deadline { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentType Type { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus Status { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsLatePayment => PaymentDate.HasValue && PaymentDate.Value > Deadline;
    }

    public class CheckoutRequestDto
    {
        [Required, MinLength(1)]
        public List<Guid> PaymentIds { get; set; } = new();

        [EnumDataType(typeof(PaymentMethodEnum))]
        public PaymentMethodEnum PaymentMethod { get; set; }
    }

    public class CheckoutResponseDto
    {
        public Guid TransactionId { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus Status { get; set; }
        public BankTransferInfoDto? BankTransfer { get; set; }
    }

    public class BankTransferInfoDto
    {
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string TransferContent { get; set; } = string.Empty;
        public string? QrCodeUrl { get; set; }
    }
}
