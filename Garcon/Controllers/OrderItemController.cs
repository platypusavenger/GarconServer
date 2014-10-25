using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Garcon.Data;
using Garcon.Models;
using Garcon.Classes;

namespace Garcon.Controllers
{
    /// <summary>
    /// OrderItem Controller - Basic CRUD for OrderItem
    /// </summary>
    public class OrderItemController : ApiController
    {
	
		private GarconEntities _db = new GarconEntities();

        // GET api/OrderItem/
        /// <summary>
        /// An iQueryable OrderItem lookup
        /// </summary>
        /// <returns>
        /// 200 - Success + A list of OrderItem
        /// 401 - Not Authorized 
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IQueryable<OrderItemModel> Get()
        {
            return _db.OrderItems.Select(o => new OrderItemModel
            {
                id = o.id, 
                orderId = o.orderId, 
                description = o.description, 
                price = o.price
            });
        }

        // GET api/OrderItem/5
        /// <summary>
        /// Retrieve a single OrderItem from the database.
        /// </summary>
        /// <param name="id">The OrderItemId of the OrderItem to return.</param>
        /// <returns>
        /// 200 - Success + The requested OrderItem.
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// </returns>
		[ResponseType(typeof(OrderItemModel))]
		public IHttpActionResult Get(int id)
        {
            OrderItemModel orderitem = Get().FirstOrDefault<OrderItemModel>(o => o.id == id);
            if (orderitem == null)
            {
                return this.NotFound("OrderItem not found.");
            }

            return Ok(orderitem);
        }

        // PUT api/OrderItem/5
        /// <summary>
        /// Save changes to a single OrderItem to the database.
        /// </summary>
        /// <param name="id">The OrderItemId of the OrderItem to save.</param>
        /// <param name="orderitemModel">The model of the edited OrderItem</param>
        /// <returns>
        /// 204 - No Content
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IHttpActionResult Put(int id, OrderItemModel orderitemModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderitemModel.id)
            {
                return BadRequest();
            }

            try
            {
				OrderItem orderitem = _db.OrderItems.FirstOrDefault(o => o.id == id);

				if (orderitem == null)
                    throw new APIException("OrderItem not found.", 404);

                if (orderitem.Order.closeDateTime != null)
                {
                    throw new APIException("This order has been closed.", 401);
                }
                orderitem.orderId = orderitemModel.orderId;
                orderitem.description = orderitemModel.description;
                orderitem.price = orderitemModel.price;

                UpdateOrderTotals(orderitem.Order);                
                _db.Entry(orderitem).State = EntityState.Modified;
                _db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (APIException ex)
            {
                if (ex.code == 404)
                    return this.NotFound(ex.message);
                else
                    return this.InternalServerError(ex);
            }
            catch (Exception e)
            {
                return this.InternalServerError(e);
            }
        }

        // POST api/OrderItem/
        /// <summary>
        /// A new OrderItem to be added.
        /// </summary>
        /// <param name="orderitemModel">The new OrderItem</param>
        /// <returns>
        /// 201 - Created + The new OrderItem
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
		[ResponseType(typeof(OrderItemModel))]
		public IHttpActionResult Post(OrderItemModel orderitemModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Order order = _db.Orders.FirstOrDefault(o => o.id == orderitemModel.orderId);
                if (order == null)
                {
                    throw new APIException("Order not found.", 404);
                }

                if (order.closeDateTime != null)
                {
                    throw new APIException("This order has been closed.", 401);
                }

				OrderItem orderitem = new OrderItem();

                orderitem.orderId = orderitemModel.orderId;
                orderitem.description = orderitemModel.description;
                orderitem.price = orderitemModel.price;

                _db.OrderItems.Add(orderitem);
                _db.SaveChanges();

                orderitemModel.id = orderitem.id;
                
                UpdateOrderTotals(orderitem.Order);
                _db.SaveChanges();
                
                return CreatedAtRoute("DefaultApi", new { id = orderitem.id }, orderitemModel);
            }
            catch (APIException ex)
            {
                if (ex.code == 404)
                    return this.NotFound(ex.message);
                else
                    return this.InternalServerError(ex);
            }
            catch (Exception e)
            {
                return this.InternalServerError(e);
            }
        }

        // DELETE api/OrderItem/5
        /// <summary>
        /// Delete a OrderItem from the database.
        /// </summary>
        /// <param name="id">The OrderItemId of the OrderItem to delete.</param>
        /// <returns>
		/// 200 - Success + The deleted OrderItem 
		/// 401 - Not Authorized 
		/// 500 - Internal Server Error + the Exception
        /// </returns>
		[ResponseType(typeof(OrderItemModel))]
		public IHttpActionResult Delete(int id)
        {
            try {
				OrderItem orderitem = _db.OrderItems.Find(id);
				if (orderitem == null)
				{
					return this.NotFound("OrderItem not found.");
				}

                if (orderitem.Order.closeDateTime != null)
                {
                    throw new APIException("This order has been closed.", 401);
                }


                OrderItemModel returnModel = Get().FirstOrDefault<OrderItemModel>(o => o.id == id);
                _db.OrderItems.Remove(orderitem);
                _db.SaveChanges();

                UpdateOrderTotals(orderitem.Order);
                _db.SaveChanges();

                return Ok(returnModel);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        private void UpdateOrderTotals(Data.Order order)
        {
            decimal amount = 0;
            decimal taxAmount = 0;
            decimal totalAmount = 0;

            foreach (OrderItem oneItem in order.OrderItems)
            {
                amount += oneItem.price;
            }

            // Sales tax in GA is 4% -- use a MAGIC NUMBER here!
            // In reality we would have multiple vendors/locations set up and a tax option in the config
            taxAmount = decimal.Round(amount * (decimal).04, 2);
            totalAmount = amount + taxAmount;

            order.amount = amount;
            order.taxAmount = taxAmount;
            order.totalAmount = totalAmount;

            _db.Entry(order).State = EntityState.Modified;
        }
    }
}
