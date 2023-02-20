namespace CardCostApi.Infrastructure.Entities
{
    public class CardCostEntity
    {
        public int Id { get; set; }
        public string Country { get; set; }

        public decimal Cost { get; set; }
    }
}