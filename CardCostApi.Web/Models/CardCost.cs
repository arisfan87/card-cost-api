using System.ComponentModel.DataAnnotations;

namespace CardCostApi.Web.Models
{
    public class CardCost
    {
        public class Request
        {
            [MaxLength(8)] [Required] public string Bin { get; set; }
        }

        public class Response
        {
            public string Country { get; set; }
            public decimal Cost { get; set; }
        }
    }
}