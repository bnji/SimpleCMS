using SimpleCMS.DAL;
using SimpleCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS.BLL
{
    public class ChangeEventHandler
    {
        private string _userIdCode;

        public ChangeEventHandler(string userIdCode)
        {
            this._userIdCode = userIdCode;
        }

        private ChangeEvent Get(BootstrapContext db, int? changeEventId)
        {
            return db.ChangeEvents.Find(changeEventId);
        }

        public ChangeEvent Update(BootstrapContext db, int? changeEventId)
        {
            ChangeEvent changeEvent = Get(db, changeEventId);
            if (changeEvent == null)
            {
                changeEvent = Create(db);
            }
            else
            {
                changeEvent.UpdatedOn = DateTime.UtcNow;
                changeEvent.UpdatedByUserIdCode = this._userIdCode;
                db.Entry(changeEvent).State = System.Data.Entity.EntityState.Modified;
            }
            return changeEvent;
        }

        public ChangeEvent Create(BootstrapContext db, DateTime? _createdOn, DateTime? _updatedOn, DateTime? _deletedOn = null,
                                    string createdByUserIdCode = null, string updatedByUserIdCode = null, string deletedByUserIdCode = null)
        {
            var changeEvent = new ChangeEvent()
            {
                CreatedOn = _createdOn.HasValue ? _createdOn.Value : DateTime.Now,
                UpdatedOn = _updatedOn.HasValue ? _updatedOn.Value : DateTime.Now,
                DeletedOn = _deletedOn,
                CreatedByUserIdCode = !String.IsNullOrEmpty(createdByUserIdCode) ? createdByUserIdCode : this._userIdCode,
                DeletedByUserIdCode = deletedByUserIdCode,
                UpdatedByUserIdCode = !String.IsNullOrEmpty(updatedByUserIdCode) ? updatedByUserIdCode : this._userIdCode,
            };
            return changeEvent;
        }

        public ChangeEvent Create(BootstrapContext db, string createdByUserIdCode = null)
        {
            var updatedByUserIdCode = createdByUserIdCode;
            var deletedByUserIdCode = createdByUserIdCode;
            return Create(db, DateTime.UtcNow, DateTime.UtcNow, null, createdByUserIdCode, updatedByUserIdCode, deletedByUserIdCode);
        }

        public ChangeEvent Delete(BootstrapContext db, int? id)
        {
            ChangeEvent changeEvent = Get(db, id);
            if (changeEvent != null)
            {
                changeEvent.DeletedOn = DateTime.UtcNow;
                changeEvent.DeletedByUserIdCode = this._userIdCode;
                db.Entry(changeEvent).State = System.Data.Entity.EntityState.Modified;
            }
            return changeEvent;
        }
    }
}