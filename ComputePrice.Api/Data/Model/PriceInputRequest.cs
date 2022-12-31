using System.ComponentModel.DataAnnotations;

namespace ComputePrice.Data.Model
{
    public class PriceInputRequest
    {
        [Required]
        public DateTime PriceDate { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public DateTime PriceDateBeg { get; set; }

        [Required]
        public DateTime PriceDateEnd { get; set; }

        [Required]
        public DateTime DeliveryDateBeg { get; set; }

        [Required]
        public DateTime DeliveryDateEnd { get; set; }
    }
}
