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
    /// User Controller - Basic CRUD for User
    /// </summary>
    public class UserController : ApiController
    {
	
		private GarconEntities _db = new GarconEntities();

        // GET api/User/
        /// <summary>
        /// An iQueryable User lookup
        /// </summary>
        /// <returns>
        /// 200 - Success + A list of User
        /// 401 - Not Authorized 
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IQueryable<UserModel> Get()
        {
            return _db.Users.Select(o => new UserModel
            {
                id = o.id, 
                username = o.username,
                email = o.email
            });
        }

        // GET api/User/5
        /// <summary>
        /// Retrieve a single User from the database.
        /// </summary>
        /// <param name="id">The UserId of the User to return.</param>
        /// <returns>
        /// 200 - Success + The requested User.
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// </returns>
		[ResponseType(typeof(UserModel))]
		public IHttpActionResult Get(int id)
        {
            UserModel user = Get().FirstOrDefault<UserModel>(o => o.id == id);
            if (user == null)
            {
                return this.NotFound("User not found.");
            }

            return Ok(user);
        }

        // PUT api/User/5
        /// <summary>
        /// Save changes to a single User to the database.
        /// </summary>
        /// <param name="id">The UserId of the User to save.</param>
        /// <param name="userModel">The model of the edited User</param>
        /// <returns>
        /// 204 - No Content
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IHttpActionResult Put(int id, UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userModel.id)
            {
                return BadRequest();
            }

            try
            {
				User user = _db.Users.FirstOrDefault(o => o.id == id);
				if (user == null)
                    throw new APIException("User not found.", 404);

                user.username = userModel.username;
                user.email = userModel.email;

                _db.Entry(user).State = EntityState.Modified;
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

        // POST api/User/
        /// <summary>
        /// A new User to be added.
        /// </summary>
        /// <param name="userModel">The new User</param>
        /// <returns>
        /// 201 - Created + The new User
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
		[ResponseType(typeof(UserModel))]
		public IHttpActionResult Post(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                User user = new User();

                user.username = userModel.username;
                user.email = userModel.email;

                _db.Users.Add(user);
                _db.SaveChanges();

                userModel.id = user.id;

                return CreatedAtRoute("DefaultApi", new { id = user.id }, userModel);
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

        // DELETE api/User/5
        /// <summary>
        /// Delete a User from the database.
        /// </summary>
        /// <param name="id">The UserId of the User to delete.</param>
        /// <returns>
		/// 200 - Success + The deleted User 
		/// 401 - Not Authorized 
		/// 500 - Internal Server Error + the Exception
        /// </returns>
		[ResponseType(typeof(UserModel))]
		public IHttpActionResult Delete(int id)
        {
            try {
				User user = _db.Users.Find(id);
				if (user == null)
				{
					return this.NotFound("User not found.");
				}

                UserModel returnModel = Get().FirstOrDefault<UserModel>(o => o.id == id);
                _db.Users.Remove(user);
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
