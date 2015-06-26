namespace Domain.Entities
{
	public class CurryOrder
    {
        public string Curry { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }
    }
}
