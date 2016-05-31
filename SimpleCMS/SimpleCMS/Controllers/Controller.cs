using SimpleCMS;
using SimpleCMS.BLL;
using SimpleCMS.DAL;
using SimpleCMS.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SimpleCMS.ViewModels;
using SimpleCMS.Web;
using System.Drawing;
using SimpleCMS.Models.Blog;

namespace BootstrapControllers
{
    [Authorize]
    public abstract class Controller<T> : System.Web.Mvc.Controller where T : class, IHasId
    {
        protected BootstrapContext db = new BootstrapContext();
        protected IRepository<T> repository = new Repository<T>();

        /// <summary>
        /// Usage:
        ///     IQueryable<T> list = getSomeListData();
        ///     Expression<Func<T, Int32>> sortExpression = SortExpression("Name"); // Sort by Name
        ///     var sortedList = list.OrderBy(sortExpression);
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private Expression<Func<T, Int32>> SortExpression(string propertyName)
        {
            var param = Expression.Parameter(typeof(T));
            var exp = Expression.Property(param, propertyName);
            var sortExpression = Expression.Lambda<Func<T, Int32>>(Expression.Convert(exp, typeof(Int32)), param);
            return sortExpression;
        }

        protected virtual IOrderedQueryable<T> PagedListSort(IQueryable<T> list, string sortName = "", string sortType = "asc")
        {
            Expression<Func<T, Int32>> sortExpression = null;
            sortName = "Id";
            sortName = !string.IsNullOrEmpty(sortName) ? sortName : "Id";
            sortExpression = SortExpression(sortName);// System.Extensions.CreatePropSelectorExpression<T, string>(sortName);
            switch (sortType.ToLower())
            {
                case "asc":
                    return list.OrderBy(sortExpression);
                default:
                    return list.OrderByDescending(sortExpression);

            }
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            var cultureName = CookieHelper.GetCultureName(Request);
            var cultureInfo = new System.Globalization.CultureInfo(cultureName);
            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            return base.BeginExecuteCore(callback, state);
        }

        protected ActionResult GetRelatedDataForParent<TParent>(int? id, string viewName)
            where TParent : class
        {
            TParent obj = db.Set<TParent>().Find(id);
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return PartialView(viewName, obj);
        }

        protected ActionResult GetRelatedData(int? id, string viewName)
        {
            T obj = Find(id);
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return PartialView(viewName, obj);
        }

        protected async Task<ActionResult> GetRelatedDataAsync(int? id, string viewName)
        {
            T obj = await FindAsync(id);
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return PartialView(viewName, obj);
        }

        protected T Find(int? id)
        {
            return db.Set<T>().Find(id);
        }

        protected async Task<T> FindAsync(int? id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        protected virtual void Add(T obj)
        {
            db.Set<T>().Add(obj);
        }

        protected virtual void Remove(T obj)
        {
            db.Set<T>().Remove(obj);
        }

        protected virtual void AddViewBag(T obj)
        {

        }

        /// <summary>
        /// Implement functionality that's called before any other code in the Create (GET) method.
        /// </summary>
        protected virtual void CreateGetHook()
        {

        }

        protected virtual void EditGetHook()
        {

        }

        protected virtual void DeleteGetHook()
        {

        }

        protected virtual void DetailsGetHook(T obj)
        {

        }

        public virtual ActionResult Index()
        {
            return View();
        }

        // GET: /{Controller}/Details/{id}
        public async Task<ActionResult> Details(int? id, bool isPartial = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T obj = await FindAsync(id);

            if (obj == null)
            {
                return HttpNotFound();
            }
            DetailsGetHook(obj);
            if (Request.IsAjaxRequest())
                return PartialView(obj);

            return View(obj);
        }

        // GET: /{Controller}/Create
        public ActionResult Create<TParent>(string viewName, IViewModelBase viewModel = null, TParent obj = null) where TParent : class
        {
            CreateGetHook();
            //AddViewBag<TParent>(obj);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, viewModel);

            return View(viewName, viewModel);
        }

        // GET: /{Controller}/Edit/{parentObjId}/{relObjId}
        public ActionResult Edit<TParent>(string viewName, IViewModelBase viewModel, TParent obj = null) where TParent : class
        {
            EditGetHook();
            //AddViewBag(obj);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, viewModel);

            return View(viewName, viewModel);
        }

        // GET: /{Controller}/Delete/{parentObjId}/{relObjId}
        public ActionResult Delete<TObj>(string viewName, TObj obj, T parent = null)
        {
            DeleteGetHook();
            if (Request.IsAjaxRequest())
                return PartialView(viewName, obj);

            return View(viewName, obj);
        }

        protected virtual jQueryDataTableResultModel<T> GetData(jQueryDataTableParamModel param, Func<IList<T>, IEnumerable<dynamic>> transform = null)
        {
            var allResults = db.Set<T>().FilterDeleted().ToList();
            return new jQueryDataTableResultModel<T>()
            {
                AllResults = allResults,
                FilteredResults = allResults,
                SelectedResults = transform != null ? transform(allResults) : allResults
            };
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult GetAjaxData(jQueryDataTableParamModel param)
        {
            var result = GetData(param);

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.AllResults.Count,
                iTotalDisplayRecords = result.FilteredResults.Count,
                aaData = result.SelectedResults
            }, JsonRequestBehavior.AllowGet);
        }

        protected bool Update(T obj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch { return false; }
            }
            return false;
        }

        // POST: {Controller}/Delete/{id}/[parentId]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public virtual async Task<ActionResult> Delete(int id)
        public virtual ActionResult DeleteObject<TObject, TParent>(int id, int? parentId = null, ActionResult onError = null)
            where TObject : class
            where TParent : class
        {
            TObject dbObj = null;
            TParent parent = db.Set<TParent>().Find(parentId);
            try
            {
                if (ModelState.IsValid)
                {
                    dbObj = db.Set<TObject>().Find(id);
                    if (dbObj != null)
                    {
                        // Create a changeEvent object
                        DeleteChangeEvent(dbObj, parent);
                        // Attach an entity
                        AddEntityToRelatedObject(dbObj, parent);
                        // Set object state to modified
                        db.Entry(dbObj).State = System.Data.Entity.EntityState.Modified;
                        // Save changes to the DB
                        db.SaveChanges();
                        // Return JSON object if the request is AJAX
                        if (Request.IsAjaxRequest())
                        {
                            return Json(true);
                        }
                        // Otherwise redirect
                        return RedirectToAction("Index");
                    }
                    if (onError == null)
                    {
                        return Json(HttpStatusCode.NotFound);
                    }
                    return onError;
                }
            }
            catch (RetryLimitExceededException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            //AddViewBag(parent);
            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView(dbObj);
            }
            return View(dbObj);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected ChangeEvent CreateChangeEvent(string userIdCode = null)
        {
            userIdCode = string.IsNullOrEmpty(userIdCode) ? GetUserIdCode() : userIdCode;
            return new ChangeEventHandler(userIdCode).Create(db);
        }

        protected ChangeEvent UpdateChangeEvent(int? changeEventId)
        {
            return new ChangeEventHandler(GetUserIdCode()).Update(db, changeEventId);
        }

        protected ChangeEvent DeleteChangeEvent(int? changeEventId)
        {
            return new ChangeEventHandler(GetUserIdCode()).Delete(db, changeEventId);
        }

        protected string GetUserIdCode()
        {
            string userIdCode = null;
            if (AccountHandler.Instance.User == null)
            {
                userIdCode = GetUser().UserIdCode;
            }
            else
            {
                userIdCode = AccountHandler.Instance.User.UserIdCode;
            }
            System.Diagnostics.Debug.Assert(userIdCode != null);
            return userIdCode;
        }

        protected ApplicationUser GetUser()
        {
            return AccountHandler.Instance.Initialize(User.Identity.Name);
        }


        // POST: /{Controller}/CreateUsingViewModel
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        protected ActionResult CreateUsingViewModel(IViewModelBase vmObj, Func<IViewModelBase, T> convertAction, ActionResult onError = null, string viewName = null)
        {
            try
            {
                // Convert ViewModel to Model (handled in the child class)
                if (ModelState.IsValid)
                {
                    T dbObj = convertAction(vmObj);
                    if (dbObj != null)
                    {
                        // Create a changeEvent object
                        AddChangeEvent(dbObj, dbObj);
                        // Create an entity?
                        //AddEntityToRelatedObject(dbObj, parent);
                        // Add the new DB Object to the List
                        db.Set<T>().Add(dbObj);
                        // Save changes to the DB
                        db.SaveChanges();
                        // Return JSON object if the request is AJAX
                        if (Request.IsAjaxRequest())
                        {
                            return Json(vmObj);
                        }
                        // Otherwise redirect
                        return RedirectToAction("Index");
                    }
                    if (onError == null)
                    {
                        return Json(HttpStatusCode.NotFound);
                    }
                    return onError;
                }
            }
            catch (RetryLimitExceededException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            AddViewBag(null);
            if (Request != null && Request.IsAjaxRequest())
            {
                if (!string.IsNullOrEmpty(viewName))
                {
                    return PartialView(viewName, vmObj);
                }
                return PartialView(vmObj);
            }
            if (!string.IsNullOrEmpty(viewName))
            {
                return View(viewName, vmObj);
            }
            return View(vmObj);
        }

        protected ActionResult CreateRelatedObjectUsingViewModel<TParent, TResult>(int? parentId, IViewModelBase vmObj, Func<TParent, TResult> convertAction, ActionResult onError = null)
            where TParent : class
            where TResult : class
        {
            TParent parent = null;
            try
            {
                if (ModelState.IsValid)
                {
                    parent = db.Set<TParent>().Find(parentId);
                    if (parent != null)
                    {
                        // Convert ViewModel to Model (handled in the child class)
                        TResult dbObj = convertAction(parent);
                        if (dbObj != null)
                        {
                            // Create a changeEvent object
                            AddChangeEvent(dbObj, parent);
                            // Attach an entity
                            AddEntityToRelatedObject(dbObj, parent);
                            // Add the new DB Object to the List
                            db.Set<TResult>().Add(dbObj);
                            // Save changes to the DB
                            db.SaveChanges();
                            // Return JSON object if the request is AJAX
                            if (Request.IsAjaxRequest())
                            {
                                return Json(vmObj);
                            }
                            // Otherwise redirect
                            return RedirectToAction("Index");
                        }
                        if (onError == null)
                        {
                            return Json(HttpStatusCode.NotFound);
                        }
                        return onError;
                    }
                }
            }
            catch (RetryLimitExceededException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            //AddViewBag(parent);
            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView(vmObj);
            }
            return View(vmObj);
        }

        protected ActionResult UpdateUsingViewModel(IViewModelBase vmObj, Func<IViewModelBase, T> convertAction, ActionResult onError = null, string viewName = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Convert ViewModel to Model (handled in the child class)
                    T dbObj = convertAction(vmObj);
                    if (dbObj != null)
                    {
                        // Update the changeEvent object
                        var hasChangeEvent = UpdateChangeEvent(dbObj, dbObj);
                        // or if no ChangeEvent is assigned to the object for some reason, create one
                        if (!hasChangeEvent)
                        {
                            AddChangeEvent(dbObj, dbObj);
                        }
                        // Attach the entity
                        db.Set<T>().Attach(dbObj);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        db.Entry(dbObj).State = System.Data.Entity.EntityState.Modified;
                        // Save changes to the DB
                        db.SaveChanges();
                        // Return JSON object if the request is AJAX
                        if (Request.IsAjaxRequest())
                        {
                            return Json(vmObj);
                        }
                        // Otherwise redirect
                        return RedirectToAction("Index");
                    }
                    if (onError == null)
                    {
                        return Json(HttpStatusCode.NotFound);
                    }
                    return onError;
                }
            }
            catch (RetryLimitExceededException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            AddViewBag(null);
            if (Request != null && Request.IsAjaxRequest())
            {
                if (!string.IsNullOrEmpty(viewName))
                {
                    return PartialView(viewName, vmObj);
                }
                return PartialView(vmObj);
            }
            if (!string.IsNullOrEmpty(viewName))
            {
                return View(viewName, vmObj);
            }
            return View(vmObj);
        }

        protected ActionResult UpdateRelatedObjectUsingViewModel<TParent, TResult>(int? parentId, IViewModelBase vmObj, Func<TParent, TResult> convertAction, ActionResult onError = null)
            where TParent : class
            where TResult : class
        {
            TParent parent = null;
            try
            {
                // Todo: BankAccount_Update works, but MemberComment_Updated doesn't (somehow). Debug this!!! ( this check is important)
                if (ModelState.IsValid)
                {
                    parent = db.Set<TParent>().Find(parentId);
                    if (parent != null)
                    {
                        // Convert ViewModel to Model (handled in the child class)
                        TResult dbObj = convertAction(parent);
                        if (dbObj != null)
                        {
                            // Update the changeEvent object
                            var hasChangeEvent = UpdateChangeEvent<TResult, TParent>(dbObj, parent);
                            // or if no ChangeEvent is assigned to the object for some reason, create one
                            if (!hasChangeEvent)
                            {
                                AddChangeEvent(dbObj, parent);
                            }
                            // Attach an entity
                            AddEntityToRelatedObject(dbObj, parent);
                            // Set object state to modified
                            db.Entry(dbObj).State = System.Data.Entity.EntityState.Modified;
                            // Save changes to the DB
                            db.SaveChanges();
                            // Return JSON object if the request is AJAX
                            if (Request.IsAjaxRequest())
                            {
                                return Json(vmObj);
                            }
                            // Otherwise redirect
                            return RedirectToAction("Index");
                        }
                        if (onError == null)
                        {
                            return Json(HttpStatusCode.NotFound);
                        }
                        return onError;
                    }
                }
            }
            catch (RetryLimitExceededException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            //AddViewBag(parent);
            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView(vmObj);
            }
            return View(vmObj);
        }

        private void AddEntityToRelatedObject<TObj, TParent>(TObj dbObj, TParent parent)
        {
            var dbObjWithEntity = dbObj as IHasEntity;
            var parentWithPersonEntity = parent as IHasEntity;
            if (parentWithPersonEntity == null)
            {
                var parentWithPerson = parent as IHasPerson;
                parentWithPersonEntity = parentWithPerson != null ? parentWithPerson.Person : null;
            }
            if (dbObjWithEntity != null && parentWithPersonEntity != null && parentWithPersonEntity != null)
            {
                dbObjWithEntity.Entity = parentWithPersonEntity.Entity;
            }
        }

        protected void AddChangeEvent<TObj, TParent>(TObj dbObj, TParent parent) where TObj : class
        {
            var dbObjWithChangeEvent = dbObj as IHasChangeEvent;
            if (dbObjWithChangeEvent != null)
            {
                dbObjWithChangeEvent.ChangeEvent = CreateChangeEvent();
                var changeEventItems = CreateChangeEventItems(dbObj, parent, ChangeEventType.CREATED);
                if (changeEventItems.Count > 0)
                {
                    dbObjWithChangeEvent.ChangeEvent.ChangeEventItems.AddRange(changeEventItems);
                }
            }
        }

        private List<ChangeEventItem> CreateChangeEventItems<TObj, TParent>(TObj dbObj, TParent parent, ChangeEventType type) where TObj : class
        {
            var objWithEntity = dbObj as IHasEntity;
            if (objWithEntity == null)
            {
                objWithEntity = parent as IHasEntity;
            }

            var changeEventItems = new List<ChangeEventItem>();
            Type entityType = dbObj.GetType();
            foreach (PropertyInfo entityProperty in entityType.GetProperties().ToList())
            {
                if (entityProperty.PropertyType.IsPrimitive || entityProperty.PropertyType.IsValueType || (entityProperty.PropertyType == typeof(string)))
                {
                    var property = db.Entry(dbObj).Property(entityProperty.Name);
                    if (property.IsModified)
                    {
                        var changeEventItem = new ChangeEventItem()
                        {
                            ChangedFrom = "" + property.OriginalValue,
                            ChangedTo = "" + property.CurrentValue,
                            Type = type,
                            Date = DateTime.UtcNow,
                            UserIdCode = GetUserIdCode(),
                            ChangeEvent = null,
                            ColumnName = property.Name,
                            TableName = typeof(TObj) != null ? typeof(TObj).Name : "",
                            Entity = objWithEntity != null ? objWithEntity.Entity : null
                        };
                        // only update if changed from isn't same as changed to (IsModified doesn't validate this)
                        if (objWithEntity != null && !changeEventItem.ChangedFrom.Equals(changeEventItem.ChangedTo))
                        {
                            changeEventItems.Add(changeEventItem);
                        }
                    }
                }
            }
            return changeEventItems;
        }

        protected bool UpdateChangeEvent<TObj, TParent>(TObj dbObj, TParent parent) where TObj : class
        {
            var dbObjWithChangeEvent = dbObj as IHasChangeEvent;
            if (dbObjWithChangeEvent != null)
            {
                dbObjWithChangeEvent.ChangeEvent = UpdateChangeEvent(dbObjWithChangeEvent.ChangeEventId);
                var changeEventItems = CreateChangeEventItems(dbObj, parent, SimpleCMS.ChangeEventType.UPDATED);
                if (changeEventItems != null)
                {
                    dbObjWithChangeEvent.ChangeEvent.ChangeEventItems.AddRange(changeEventItems);
                }
                return dbObjWithChangeEvent.ChangeEvent != null;
            }
            return true;
        }

        protected void DeleteChangeEvent<TObj, TParent>(TObj dbObj, TParent parent) where TObj : class
        {
            var dbObjWithChangeEvent = dbObj as IHasChangeEvent;
            if (dbObjWithChangeEvent != null)
            {
                dbObjWithChangeEvent.ChangeEvent = DeleteChangeEvent(dbObjWithChangeEvent.ChangeEventId);
                if (dbObjWithChangeEvent.ChangeEvent != null)
                {
                    var changeEventItems = CreateChangeEventItems(dbObj, parent, SimpleCMS.ChangeEventType.DELETED);
                    if (changeEventItems != null)
                    {
                        dbObjWithChangeEvent.ChangeEvent.ChangeEventItems.AddRange(changeEventItems);
                    }
                }
            }
        }

        protected List<SimpleCMS.Models.File> AddFiles(IEnumerable<FileUploadInfo> fileUploadInfo)
        {
            var files = new List<SimpleCMS.Models.File>();

            if (fileUploadInfo == null)
                return files;

            byte[] data = null;
            FileInfo fileInfo = null;
            foreach (var item in fileUploadInfo)
            {
                ReadFileData(item, out data, out fileInfo);
                if (data != null && fileInfo != null)
                {
                    var file = new SimpleCMS.Models.File()
                    {
                        Name = fileInfo.Name,
                        ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName),
                        Length = fileInfo.Length,
                        Data = data,
                        ChangeEvent = CreateChangeEvent()
                    };
                    files.Add(file);
                }
            }
            return files;
        }

        protected void ReadFileData(FileUploadInfo o, out byte[] file, out FileInfo fileInfo)
        {
            var fh = CreateFileUploadHandler();
            file = null;
            fileInfo = null;
            if (o != null && o.FileName != null)
            {
                var filePath = fh.GetFilePhysicalPath(o.FileName);
                fileInfo = new FileInfo(filePath);
                file = System.IO.File.ReadAllBytes(filePath);
            }
        }

        protected virtual FileUploadHandler CreateFileUploadHandler()
        {
            throw new Exception("FileUploadHandler is not implemented!");
        }

        protected virtual string GetImagesPath()
        {
            return "~/SimpleCMS/Data/Images";
        }

        protected virtual string GetFilePath()
        {
            return "~/SimpleCMS/Data";
        }

        //[HttpPost]
        //public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> Files)
        //{
        //    var fh = new FileUploadHandler(Server, Session, false, GetFilePath());
        //    if (fh.Save(Files))
        //    {
        //        return Content("");
        //    }
        //    return Content("Error");
        //}

        //public ActionResult RemoveFile(string[] fileNames)
        //{
        //    var fh = new FileUploadHandler(Server, Session, false, GetFilePath());
        //    if (fh.Remove(fileNames))
        //    {
        //        return Content("");
        //    }
        //    return Content("Error");
        //}

        internal List<ImageInfo> SaveImages(IEnumerable<FileUploadInfo> Files)
        {
            var fh = new FileUploadHandler(Server, Session, false, GetImagesPath());
            if (fh.Save(Files))
            {
                return SaveImages();
            }
            return null;
        }

        internal List<ImageInfo> SaveImages(IEnumerable<HttpPostedFileBase> Files)
        {
            var fh = new FileUploadHandler(Server, Session, false, GetImagesPath());
            if (fh.Save(Files))
            {
                return SaveImages();
            }
            return null;
        }

        internal List<ImageInfo> SaveImages()
        {
            var images = new List<ImageInfo>();
            //var files = AddFiles(vmObj.Files);
            foreach (var fileInfo in FileUploadHandler.UploadedFiles)
            {
                var file = new SimpleCMS.Models.File()
                {
                    Name = fileInfo.Name,
                    Length = fileInfo.Length,
                    ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName),
                    Data = System.IO.File.ReadAllBytes(Server.MapPath(Path.Combine(GetImagesPath(), fileInfo.Name))),
                    ChangeEvent = CreateChangeEvent()
                };
                var imageInfo = new ImageInfo()
                {
                    Width = -1,
                    Height = -1,
                    Description = string.Empty,
                    File = file,
                    ChangeEvent = CreateChangeEvent()
                };
                using (FileStream fs = new FileStream(Server.MapPath(Path.Combine(GetImagesPath(), file.Name)), FileMode.Open, FileAccess.Read))
                {
                    using (Image img = Image.FromStream(stream: fs,
                                                        useEmbeddedColorManagement: false,
                                                        validateImageData: false))
                    {
                        float width = img.PhysicalDimension.Width;
                        float height = img.PhysicalDimension.Height;
                        //float hresolution = img.HorizontalResolution;
                        //float vresolution = img.VerticalResolution;
                        imageInfo.Width = (int)width;
                        imageInfo.Height = (int)height;
                    }
                    fs.Close();
                }
                db.Files.Add(file);
                db.Images.Add(imageInfo);
                images.Add(imageInfo);
                //try
                //{
                //    db.SaveChanges();
                //    images.Add(imageInfo);
                //}
                //catch (Exception ex)
                //{
                //    // TODO: Handle exception
                //}
            }
            return images;
        }
    }
}