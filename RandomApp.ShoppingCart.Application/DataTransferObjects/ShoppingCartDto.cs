namespace RandomApp.ShoppingCartManagement.Application.DataTransferObjects
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<ShoppingCartItemDto> Items { get; set; }
        public decimal TotalAmount
        {
            get
            {
                if (Items == null)
                {
                    return 0;
                }

                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Price * item.Quantity;
                }
                return total;
            }
        }
    }
}
