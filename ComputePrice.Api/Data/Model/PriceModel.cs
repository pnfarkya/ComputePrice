using ComputePrice.UI.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputePrice.Data.Model
{
    [Table("VC_Prices")]
    public class PriceModel
    {
        [Key]
        public int Surrogate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime PriceDate { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public bool Status { get; set; }

        public string State
        {
            get => Status ? PriceStatus.Active.ToString() : PriceStatus.Inactive.ToString();
        }
    }
}
