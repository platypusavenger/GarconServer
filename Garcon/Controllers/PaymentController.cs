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
    /// Payment Controller - Basic CRUD for Payment
    /// </summary>
    public class PaymentController : ApiController
    {
	
		private GarconEntities _db = new GarconEntities();

        // GET api/Payment/
        /// <summary>
        /// An iQueryable Payment lookup
        /// </summary>
        /// <returns>
        /// 200 - Success + A list of Payment
        /// 401 - Not Authorized 
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IQueryable<PaymentModel> Get()
        {
            return _db.Payments.Select(o => new PaymentModel
            {
                id = o.id, 
                orderId = o.orderId, 
                userCardId = o.userCardId, 
                tipAmount = o.tipAmount, 
                amount = o.amount
            });
        }

        // GET api/Payment/5
        /// <summary>
        /// Retrieve a single Payment from the database.
        /// </summary>
        /// <param name="id">The PaymentId of the Payment to return.</param>
        /// <returns>
        /// 200 - Success + The requested Payment.
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// </returns>
		[ResponseType(typeof(PaymentModel))]
		public IHttpActionResult Get(int id)
        {
            PaymentModel payment = Get().FirstOrDefault<PaymentModel>(o => o.id == id);
            if (payment == null)
            {
                return this.NotFound("Payment not found.");
            }

            return Ok(payment);
        }

        // PUT api/Payment/5
        /// <summary>
        /// Save changes to a single Payment to the database.
        /// </summary>
        /// <param name="id">The PaymentId of the Payment to save.</param>
        /// <param name="paymentModel">The model of the edited Payment</param>
        /// <returns>
        /// 204 - No Content
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IHttpActionResult Put(int id, PaymentModel paymentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paymentModel.id)
            {
                return BadRequest();
            }

            try
            {
				Payment payment = _db.Payments.FirstOrDefault(o => o.id == id);
				if (payment == null)
                    throw new APIException("Payment not found.", 404);

                payment.orderId = paymentModel.orderId;
                payment.userCardId = paymentModel.userCardId;
                payment.tipAmount = paymentModel.tipAmount;
                payment.amount = paymentModel.amount;

                _db.Entry(payment).State = EntityState.Modified;
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

        // POST api/Payment/
        /// <summary>
        /// A new Payment to be added.
        /// </summary>
        /// <param name="paymentModel">The new Payment</param>
        /// <returns>
        /// 201 - Created + The new Payment
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
		[ResponseType(typeof(PaymentModel))]
		public IHttpActionResult Post(PaymentModel paymentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Payment payment = new Payment();

                payment.orderId = paymentModel.orderId;
                payment.userCardId = paymentModel.userCardId;
                payment.tipAmount = paymentModel.tipAmount;
                payment.amount = paymentModel.amount;
                
                _db.Payments.Add(payment);
                _db.SaveChanges();

                paymentModel.id = payment.id;

                return CreatedAtRoute("DefaultApi", new { id = payment.id }, paymentModel);
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

        // DELETE api/Payment/5
        /// <summary>
        /// Delete a Payment from the database.
        /// </summary>
        /// <param name="id">The PaymentId of the Payment to delete.</param>
        /// <returns>
		/// 200 - Success + The deleted Payment 
		/// 401 - Not Authorized 
        /// 405 - Method Not Allowed
		/// 500 - Internal Server Error + the Exception
        /// </returns>
		[ResponseType(typeof(PaymentModel))]
		public IHttpActionResult Delete(int id)
        {
            try {
				Payment payment = _db.Payments.Find(id);
				if (payment == null)
				{
					return this.NotFound("Payment not found.");
				}

                PaymentModel returnModel = Get().FirstOrDefault<PaymentModel>(o => o.id == id);
                _db.Payments.Remove(payment);
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
