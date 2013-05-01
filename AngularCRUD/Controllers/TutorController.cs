using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularCRUD.Models;

namespace AngularCRUD.Controllers
{
    public class TutorController : ApiController
    {
        private TutoringContext db = new TutoringContext();

        // GET api/Tutor
        public IEnumerable<Tutor> GetTutors(string q = null, string sort = null, bool desc = false,
                                                         int? limit = null, int offset = 0)
        {
            var list = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<Tutor>();

            IQueryable<Tutor> items = string.IsNullOrEmpty(sort) ? list.OrderBy(o => o.Age)
                : list.OrderBy(String.Format("it.{0} {1}", sort, desc ? "DESC" : "ASC"));

            if (!string.IsNullOrEmpty(q) && q != "undefined") items = items.Where(t => t.FullName.Contains(q));

            if (offset > 0) items = items.Skip(offset);
            if (limit.HasValue) items = items.Take(limit.Value);
            return items;
        }

        // GET api/Tutor/5
        public Tutor GetTutor(int id)
        {
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tutor;
        }

        // PUT api/Tutor/5
        public HttpResponseMessage PutTutor(int id, Tutor tutor)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != tutor.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(tutor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Tutor
        public HttpResponseMessage PostTutor(Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                db.Tutors.Add(tutor);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tutor);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tutor.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Tutor/5
        public HttpResponseMessage DeleteTutor(int id)
        {
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Tutors.Remove(tutor);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tutor);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}