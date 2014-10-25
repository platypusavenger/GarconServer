using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garcon.Models
{
    public class OrderModel
    {
        public int id { get; set; }
        public int tableId { get; set; }
        public DateTime openDateTime { get; set; }
        public Nullable<DateTime> closeDateTime { get; set; }
        public decimal amount { get; set; }
        public decimal taxAmount { get; set; }
        public decimal totalAmount { get; set; }
    }

    public class OrderDetailModel
    {
        public int id { get; set; }
        public int tableId { get; set; }
        public DateTime openDateTime { get; set; }
        public Nullable<DateTime> closeDateTime { get; set; }
        public decimal amount { get; set; }
        public decimal taxAmount { get; set; }
        public decimal totalAmount { get; set; }

        public List<OrderItemModel> Items { get; set; }
        public List<PaymentModel> Payments { get; set; }

        public OrderDetailModel(Data.Order order, int userId)
        {
            id = order.id;
            tableId = order.tableId;
            openDateTime = order.openDateTime;
            closeDateTime = order.closeDateTime;
            amount = order.amount;
            taxAmount = order.taxAmount;
            totalAmount = order.totalAmount;
            Items = order.OrderItems.Select(o => new OrderItemModel
            {
                id = o.id,
                orderId = o.orderId,
                description = o.description,
                price = o.price
            }).ToList<OrderItemModel>();

            Payments = order.Payments.Where(p => p.id == userId).Select(o => new PaymentModel
            {
                id = o.id,
                orderId = o.orderId,
                userCardId = o.userCardId,
                tipAmount = o.tipAmount,
                amount = o.amount
            }).ToList();

        }

    }
}