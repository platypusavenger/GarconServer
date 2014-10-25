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
    /// Order Controller - Basic CRUD for Order
    /// </summary>
    public class OrderController : ApiController
    {
	
		private GarconEntities _db = new GarconEntities();

        // GET api/Order/
        /// <summary>
        /// An iQueryable Order lookup
        /// </summary>
        /// <returns>
        /// 200 - Success + A list of Order
        /// 401 - Not Authorized 
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IQueryable<OrderModel> Get()
        {
            return _db.Orders.Select(o => new OrderModel
            {
                id = o.id, 
                tableId = o.tableId, 
                openDateTime = o.openDateTime,
                closeDateTime = o.closeDateTime, 
                taxAmount = o.taxAmount, 
                totalAmount = o.totalAmount
            });
        }

        // GET api/Order/5
        /// <summary>
        /// Retrieve a single Order from the database.
        /// </summary>
        /// <param name="id">The OrderId of the Order to return.</param>
        /// <returns>
        /// 200 - Success + The requested Order.
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// </returns>
		[ResponseType(typeof(OrderModel))]
		public IHttpActionResult Get(int id)
        {
            OrderModel order = Get().FirstOrDefault<OrderModel>(o => o.id == id);
            if (order == null)
            {
                return this.NotFound("Order not found.");
            }

            return Ok(order);
        }

        // PUT api/Order/5
        /// <summary>
        /// Save changes to a single Order to the database.
        /// </summary>
        /// <param name="id">The OrderId of the Order to save.</param>
        /// <param name="orderModel">The model of the edited Order</param>
        /// <returns>
        /// 204 - No Content
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IHttpActionResult Put(int id, OrderModel orderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderModel.id)
            {
                return BadRequest();
            }

            try
            {
                Order order = _db.Orders.FirstOrDefault(o => o.id == id);
				if (order == null)
                    throw new APIException("Order not found.", 404);

                order.tableId = orderModel.tableId;
                order.openDateTime = orderModel.openDateTime;
                order.closeDateTime = orderModel.closeDateTime;
                order.taxAmount = orderModel.taxAmount;
                order.totalAmount = orderModel.totalAmount;

                _db.Entry(order).State = EntityState.Modified;
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

        // POST api/Order/
        /// <summary>
        /// A new Order to be added.
        /// </summary>
        /// <param name="orderModel">The new Order</param>
        /// <returns>
        /// 201 - Created + The new Order
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
		[ResponseType(typeof(OrderModel))]
		public IHttpActionResult Post(OrderModel orderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Order order = new Order();

                order.tableId = orderModel.tableId;
                order.openDateTime = orderModel.openDateTime;
                order.closeDateTime = orderModel.closeDateTime;
                order.taxAmount = orderModel.taxAmount;
                order.totalAmount = orderModel.totalAmount;
                
                _db.Orders.Add(order);
                _db.SaveChanges();

                orderModel.id = order.id;

                return CreatedAtRoute("DefaultApi", new { id = order.id }, orderModel);
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

        // DELETE api/Order/5
        /// <summary>
        /// Delete a Order from the database.
        /// </summary>
        /// <param name="id">The OrderId of the Order to delete.</param>
        /// <returns>
		/// 200 - Success + The deleted Order 
		/// 401 - Not Authorized 
		/// 500 - Internal Server Error + the Exception
        /// </returns>
		[ResponseType(typeof(OrderModel))]
		public IHttpActionResult Delete(int id)
        {
            try {
				Order order = _db.Orders.Find(id);
				if (order == null)
				{
					return this.NotFound("Order not found.");
				}

                OrderModel returnModel = Get().FirstOrDefault<OrderModel>(o => o.id == id);
                _db.Orders.Remove(order);
                _db.SaveChanges();

                return Ok(returnModel);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
