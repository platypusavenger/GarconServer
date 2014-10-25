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
    /// UserCard Controller - Basic CRUD for UserCard
    /// </summary>
    public class UserCardController : ApiController
    {
	
		private GarconEntities _db = new GarconEntities();

        // GET api/UserCard/
        /// <summary>
        /// An iQueryable UserCard lookup
        /// </summary>
        /// <returns>
        /// 200 - Success + A list of UserCard
        /// 401 - Not Authorized 
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IQueryable<UserCardModel> Get()
        {
            return _db.UserCards.Select(o => new UserCardModel
            {
                id = o.id, 
                userId = o.userId, 
                cardType = o.cardType, 
                description = o.description, 
                fakeDigits = o.fakeDigits, 
                fakeCVV2 = o.fakeCVV2
            });
        }

        // GET api/UserCard/5
        /// <summary>
        /// Retrieve a single UserCard from the database.
        /// </summary>
        /// <param name="id">The UserCardId of the UserCard to return.</param>
        /// <returns>
        /// 200 - Success + The requested UserCard.
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// </returns>
		[ResponseType(typeof(UserCardModel))]
		public IHttpActionResult Get(int id)
        {
            UserCardModel usercard = Get().FirstOrDefault<UserCardModel>(o => o.id == id);
            if (usercard == null)
            {
                return this.NotFound("UserCard not found.");
            }

            return Ok(usercard);
        }

        // PUT api/UserCard/5
        /// <summary>
        /// Save changes to a single UserCard to the database.
        /// </summary>
        /// <param name="id">The UserCardId of the UserCard to save.</param>
        /// <param name="usercardModel">The model of the edited UserCard</param>
        /// <returns>
        /// 204 - No Content
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IHttpActionResult Put(int id, UserCardModel usercardModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usercardModel.id)
            {
                return BadRequest();
            }

            try
            {
				UserCard usercard = _db.UserCards.FirstOrDefault(o => o.id == id);
				if (usercard == null)
                    throw new APIException("UserCard not found.", 404);

                usercard.userId = usercardModel.userId;
                usercard.cardType = usercardModel.cardType;
                usercard.description = usercardModel.description;
                usercard.fakeDigits = usercardModel.fakeDigits;
                usercard.fakeCVV2 = usercardModel.fakeCVV2;

                _db.Entry(usercard).State = EntityState.Modified;
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

        // POST api/UserCard/
        /// <summary>
        /// A new UserCard to be added.
        /// </summary>
        /// <param name="usercardModel">The new UserCard</param>
        /// <returns>
        /// 201 - Created + The new UserCard
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
		[ResponseType(typeof(UserCardModel))]
		public IHttpActionResult Post(UserCardModel usercardModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                UserCard usercard = new UserCard();

                usercard.userId = usercardModel.userId;
                usercard.cardType = usercardModel.cardType;
                usercard.description = usercardModel.description;
                usercard.fakeDigits = usercardModel.fakeDigits;
                usercard.fakeCVV2 = usercardModel.fakeCVV2;

                _db.UserCards.Add(usercard);
                _db.SaveChanges();

                usercardModel.id = usercard.id;

                return CreatedAtRoute("DefaultApi", new { id = usercard.id }, usercardModel);
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

        // DELETE api/UserCard/5
        /// <summary>
        /// Delete a UserCard from the database.
        /// </summary>
        /// <param name="id">The UserCardId of the UserCard to delete.</param>
        /// <returns>
		/// 200 - Success + The deleted UserCard 
		/// 401 - Not Authorized 
		/// 500 - Internal Server Error + the Exception
        /// </returns>
		[ResponseType(typeof(UserCardModel))]
		public IHttpActionResult Delete(int id)
        {
            try {
				UserCard usercard = _db.UserCards.Find(id);
				if (usercard == null)
				{
					return this.NotFound("UserCard not found.");
				}

                UserCardModel returnModel = Get().FirstOrDefault<UserCardModel>(o => o.id == id);
                _db.UserCards.Remove(usercard);
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
