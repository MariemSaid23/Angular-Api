namespace Talabat.core.Entities.Order_Aggregate
{
    public interface IOrderItem
    {
        decimal Price { get; set; }
        ProductItemOrdered Product { get; set; }
        int Quantity { get; set; }
    }
}