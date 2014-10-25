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
    /// Merchant Controller - Basic CRUD for Merchant
    /// </summary>
    public class MerchantController : ApiController
    {
	
		private GarconEntities _db = new GarconEntities();

        // GET api/Merchant/
        /// <summary>
        /// An iQueryable Merchant lookup
        /// </summary>
        /// <returns>
        /// 200 - Success + A list of Merchant
        /// 401 - Not Authorized 
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IQueryable<MerchantModel> Get()
        {
            return _db.Merchants.Select(o => new MerchantModel
            {
                id = o.id, 
                description = o.description, 
                contactPhone = o.contactPhone,
                contactName = o.contactName
            });
        }

        // GET api/Merchant/5
        /// <summary>
        /// Retrieve a single Merchant from the database.
        /// </summary>
        /// <param name="id">The MerchantId of the Merchant to return.</param>
        /// <returns>
        /// 200 - Success + The requested Merchant.
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// </returns>
		[ResponseType(typeof(MerchantModel))]
		public IHttpActionResult Get(int id)
        {
            MerchantModel merchant = Get().FirstOrDefault<MerchantModel>(o => o.id == id);
            if (merchant == null)
            {
                return this.NotFound("Merchant not found.");
            }

            return Ok(merchant);
        }

        // PUT api/Merchant/5
        /// <summary>
        /// Save changes to a single Merchant to the database.
        /// </summary>
        /// <param name="id">The MerchantId of the Merchant to save.</param>
        /// <param name="merchantModel">The model of the edited Merchant</param>
        /// <returns>
        /// 204 - No Content
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IHttpActionResult Put(int id, MerchantModel merchantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != merchantModel.id)
            {
                return BadRequest();
            }

            try
            {
                Merchant merchant = _db.Merchants.FirstOrDefault(o => o.id == id);
				if (merchant == null)
                    throw new APIException("Merchant not found.", 404);

                merchant.description = merchantModel.description;
                merchant.contactPhone = merchantModel.contactPhone;
                merchant.contactName = merchant.contactName;

                _db.Entry(merchant).State = EntityState.Modified;
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

        // POST api/Merchant/
        /// <summary>
        /// A new Merchant to be added.
        /// </summary>
        /// <param name="merchantModel">The new Merchant</param>
        /// <returns>
        /// 201 - Created + The new Merchant
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
		[ResponseType(typeof(MerchantModel))]
		public IHttpActionResult Post(MerchantModel merchantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Merchant merchant = new Merchant();

                merchant.description = merchantModel.description;
                merchant.contactPhone = merchantModel.contactPhone;
                merchant.contactName = merchant.contactName;
                
                _db.Merchants.Add(merchant);
                _db.SaveChanges();

                merchantModel.id = merchant.id;

                return CreatedAtRoute("DefaultApi", new { id = merchant.id }, merchantModel);
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

        // DELETE api/Merchant/5
        /// <summary>
        /// Delete a Merchant from the database.
        /// </summary>
        /// <param name="id">The MerchantId of the Merchant to delete.</param>
        /// <returns>
        /// 405 - Method Not Allowed
        /// </returns>
		[ResponseType(typeof(MerchantModel))]
		public IHttpActionResult Delete(int id)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);  
        }
    }
}
