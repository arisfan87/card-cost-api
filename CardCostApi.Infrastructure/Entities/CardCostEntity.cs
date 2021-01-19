namespace CardCostApi.Infrastructure.Entities
{
    public class CardCostEntity : BaseEntity
    {
        public string Country { get; set; }

        public decimal Cost { get; set; }
    }
}