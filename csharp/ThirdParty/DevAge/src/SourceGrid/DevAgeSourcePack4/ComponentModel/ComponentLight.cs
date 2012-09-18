using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevAge.ComponentModel
{
    /// <summary>
    /// A IComponent implementation, used as a base class for component derived class. It is similar to the System Component class but doesn't derive from MarshalByRef class, for this reason it is faster and consume less resources.
    /// Can be serialized.
    /// </summary>
    [Serializable]
    public class ComponentLight : IComponent
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ComponentLight()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public ComponentLight(ComponentLight other)
        {
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IContainer Container
        {
            get
            {
                if (Site != null)
                {
                    return Site.Container;
                }
                return null;
            }
        }

        protected virtual object GetService(Type service)
        {
            if (Site != null)
            {
                return Site.GetService(service);
            }
            return null;
        }

        #region IComponent Members

        public event EventHandler Disposed;
        [NonSerialized()]
        private System.ComponentModel.ISite mSite = null;

        [DefaultValue(null), Browsable(false)]
        public System.ComponentModel.ISite Site
        {
            get { return mSite; }
            set { mSite = value; }
        }

        #endregion

        #region  IDisposable Support
        private bool disposed = false;        // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                    lock (this)
                    {
                        if (Site != null && Site.Container != null)
                        {
                            Site.Container.Remove(this);
                        }
                    }

                    if (Disposed != null)
                        Disposed(this, EventArgs.Empty);

                    // free unmanaged resources when explicitly called
                }

                // free shared unmanaged resources

                disposed = true;
            }
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ComponentLight()
        {
            // Do not change this code.  Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        #endregion

    }

}
