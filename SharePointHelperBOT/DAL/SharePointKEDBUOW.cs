using SharePointHelperBOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointHelperBOT.DAL
{
    public interface ISharePointKEDBUOW
    {
        // Save pending changes to the data store.
        void Commit();

        // Repositories
        IRepository<FAQ> FAQs { get; }
    }
    [Serializable]
    public class SharePointKEDBUOW : ISharePointKEDBUOW , IDisposable
    {
        private SharePointKEDBContext DbContext { get; set; }
        public SharePointKEDBUOW()
        {
            CreateDbContext();
        }
        private IRepository<FAQ> _faqs;

        public IRepository<FAQ> FAQs
        {
            get
            {
                if (_faqs == null)
                {
                    _faqs = new SharePointKEDBRepository<FAQ>(DbContext);

                }
                return _faqs;
            }
        }
        public void Commit()
        {
            DbContext.SaveChanges();
        }

        protected void CreateDbContext()
        {
            DbContext = new SharePointKEDBContext();

            // Do NOT enable proxied entities, else serialization fails.
            //if false it will not get the associated certification and skills when we
            //get the applicants
            DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

    }
}