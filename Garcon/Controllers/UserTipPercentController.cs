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
    /// UserTipPercent Controller - Basic CRUD for UserTipPercent
    /// </summary>
    public class UserTipPercentController : ApiController
    {
	
		private GarconEntities _db = new GarconEntities();

        // GET api/UserTipPercent/
        /// <summary>
        /// An iQueryable UserTipPercent lookup
        /// </summary>
        /// <returns>
        /// 200 - Success + A list of UserTipPercent
        /// 401 - Not Authorized 
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IQueryable<UserTipPercentModel> Get()
        {
            return _db.UserTipPercents.Select(o => new UserTipPercentModel
            {
                id = o.id, 
                userId = o.userId, 
                tipPercent = o.tipPercent
            });
        }

        // GET api/UserTipPercent/5
        /// <summary>
        /// Retrieve a single UserTipPercent from the database.
        /// </summary>
        /// <param name="id">The UserTipPercentId of the UserTipPercent to return.</param>
        /// <returns>
        /// 200 - Success + The requested UserTipPercent.
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// </returns>
		[ResponseType(typeof(UserTipPercentModel))]
		public IHttpActionResult Get(int id)
        {
            UserTipPercentModel usertippercent = Get().FirstOrDefault<UserTipPercentModel>(o => o.id == id);
            if (usertippercent == null)
            {
                return this.NotFound("UserTipPercent not found.");
            }

            return Ok(usertippercent);
        }

        // PUT api/UserTipPercent/5
        /// <summary>
        /// Save changes to a single UserTipPercent to the database.
        /// </summary>
        /// <param name="id">The UserTipPercentId of the UserTipPercent to save.</param>
        /// <param name="usertippercentModel">The model of the edited UserTipPercent</param>
        /// <returns>
        /// 204 - No Content
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IHttpActionResult Put(int id, UserTipPercentModel usertippercentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usertippercentModel.id)
            {
                return BadRequest();
            }

            try
            {
                UserTipPercent usertippercent = _db.UserTipPercents.FirstOrDefault(o => o.id == id);
				if (usertippercent == null)
                    throw new APIException("UserTipPercent not found.", 404);

                usertippercent.tipPercent = usertippercentModel.tipPercent;

                _db.Entry(usertippercent).State = EntityState.Modified;
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

        // POST api/UserTipPercent/
        /// <summary>
        /// A new UserTipPercent to be added.
        /// </summary>
        /// <param name="usertippercentModel">The new UserTipPercent</param>
        /// <returns>
        /// 201 - Created + The new UserTipPercent
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
		[ResponseType(typeof(UserTipPercentModel))]
		public IHttpActionResult Post(UserTipPercentModel usertippercentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                UserTipPercent usertippercent = new UserTipPercent();

                usertippercent.userId = usertippercentModel.userId;
                usertippercent.tipPercent = usertippercentModel.tipPercent;

                _db.UserTipPercents.Add(usertippercent);
                _db.SaveChanges();

                usertippercentModel.id = usertippercent.id;

                return CreatedAtRoute("DefaultApi", new { id = usertippercent.id }, usertippercentModel);
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

        // DELETE api/UserTipPercent/5
        /// <summary>
        /// Delete a UserTipPercent from the database.
        /// </summary>
        /// <param name="id">The UserTipPercentId of the UserTipPercent to delete.</param>
        /// <returns>
		/// 200 - Success + The deleted UserTipPercent 
		/// 401 - Not Authorized 
		/// 500 - Internal Server Error + the Exception
        /// </returns>
		[ResponseType(typeof(UserTipPercentModel))]
		public IHttpActionResult Delete(int id)
        {
            try {
				UserTipPercent usertippercent = _db.UserTipPercents.Find(id);
				if (usertippercent == null)
				{
					return this.NotFound("UserTipPercent not found.");
				}

                UserTipPercentModel returnModel = Get().FirstOrDefault<UserTipPercentModel>(o => o.id == id);
                _db.UserTipPercents.Remove(usertippercent);
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
