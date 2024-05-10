using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core;
using Talabat.core.Entities;
using Talabat.core.Entities.Order_Aggregate;
using Talabat.core.Repositories.Contract;
using Talabat.core.Services.Contract;

namespace Talabat.services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IunitOfWork _unitOfWork;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenericRepository<Order> _orderRepo;


        public OrderService(
            IBasketRepository basketRepo,
            IunitOfWork unitOfWork
            //IGenericRepository<Product>productRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            //IGenericRepository<Order>orderRepo
            )


            
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            ///_productRepo = productRepo;
            ///_deliveryMethodRepo = deliveryMethodRepo;
            ///_orderRepo = orderRepo;
            
        }


        public async Task<Order?> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);

            var orderItems = new List<OrderItem>();
            if (basket?.Items?.Count > 0)

               
            {
                foreach (var item in basket.Items )
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(ProductItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);

                }
            }
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
              var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);
            var order = new Order(
                buyerEmail: buyerEmail,
            shippingaddress: shippingAddress,
                deliveryMethodId: deliveryMethodId,
                items: orderItems,
                subtotal: subtotal
                );
            _unitOfWork.Repository<Order>().AddAsync(order);
         var result= await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;

            return order;

        }
        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
        public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int order)
        {
            throw new NotImplementedException();
        }
        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }




    }
}
