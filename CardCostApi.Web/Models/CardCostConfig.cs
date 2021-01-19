using System.ComponentModel.DataAnnotations;

namespace CardCostApi.Web.Models
{
    public class CardCostConfig
    {
        public class Request
        {
            private string _country;

            [Required]
            [MaxLength(2)]
            [MinLength(2)]
            public string Country
            {
                get => _country;
                set => _country = value.ToUpper();
            }

            [Required] [Range(0.00, 999999999)] public decimal? Cost { get; set; } = null!;
        }

        public class Response
        {
            public string Country { get; set; }
            public decimal Cost { get; set; }
        }
    }
}