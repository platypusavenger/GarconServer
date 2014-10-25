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
    /// Table Controller - Basic CRUD for Table
    /// </summary>
    public class TableController : ApiController
    {
    	private GarconEntities _db = new GarconEntities();

        // GET api/Table/
        /// <summary>
        /// An iQueryable Table lookup
        /// </summary>
        /// <returns>
        /// 200 - Success + A list of Table
        /// 401 - Not Authorized 
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IQueryable<TableModel> Get()
        {
            return _db.Tables.Select(o => new TableModel
            {
                id = o.id, 
                merchantId = o.merchantId,
                beaconId = o.beaconId, 
                description = o.description, 
                available = o.available
            });
        }

        // GET api/Table/5
        /// <summary>
        /// Retrieve a single Table from the database.
        /// </summary>
        /// <param name="id">The TableId of the Table to return.</param>
        /// <returns>
        /// 200 - Success + The requested Table.
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// </returns>
		[ResponseType(typeof(TableModel))]
		public IHttpActionResult Get(int id)
        {
            TableModel table = Get().FirstOrDefault<TableModel>(o => o.id == id);
            if (table == null)
            {
                return this.NotFound("Table not found.");
            }

            return Ok(table);
        }

        // PUT api/Table/5
        /// <summary>
        /// Save changes to a single Table to the database.
        /// </summary>
        /// <param name="id">The TableId of the Table to save.</param>
        /// <param name="tableModel">The model of the edited Table</param>
        /// <returns>
        /// 204 - No Content
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
        public IHttpActionResult Put(int id, TableModel tableModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tableModel.id)
            {
                return BadRequest();
            }

            try
            {
                Table table = _db.Tables.FirstOrDefault(o => o.id == id);
				if (table == null)
                    throw new APIException("Table not found.", 404);

                table.merchantId = tableModel.merchantId;
                table.available = tableModel.available;
                table.beaconId = tableModel.beaconId;
                table.description = tableModel.description;

                _db.Entry(table).State = EntityState.Modified;
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

        // POST api/Table/
        /// <summary>
        /// A new Table to be added.
        /// </summary>
        /// <param name="tableModel">The new Table</param>
        /// <returns>
        /// 201 - Created + The new Table
        /// 400 - Bad Request + (Invalid Model State)
        /// 401 - Not Authorized 
        /// 404 - Not Found + Reason
        /// 500 - Internal Server Error + Exception
        /// </returns>
		[ResponseType(typeof(TableModel))]
		public IHttpActionResult Post(TableModel tableModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Table table = new Table();

                table.merchantId = tableModel.merchantId;
                table.beaconId = tableModel.beaconId;
                table.description = tableModel.description;
                table.available = tableModel.available;

                _db.Tables.Add(table);
                _db.SaveChanges();

                tableModel.id = table.id;

                return CreatedAtRoute("DefaultApi", new { id = table.id }, tableModel);
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

        // DELETE api/Table/5
        /// <summary>
        /// Delete a Table from the database.
        /// </summary>
        /// <param name="id">The TableId of the Table to delete.</param>
        /// <returns>
		/// 200 - Success + The deleted Table 
		/// 401 - Not Authorized 
		/// 500 - Internal Server Error + the Exception
        /// </returns>
		[ResponseType(typeof(TableModel))]
		public IHttpActionResult Delete(int id)
        {
            try {
				Table table = _db.Tables.Find(id);
				if (table == null)
				{
					return this.NotFound("Table not found.");
				}

                TableModel returnModel = Get().FirstOrDefault<TableModel>(o => o.id == id);
                _db.Tables.Remove(table);
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
