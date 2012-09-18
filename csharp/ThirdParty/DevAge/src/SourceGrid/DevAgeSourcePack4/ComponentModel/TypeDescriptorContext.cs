using System;
using System.Collections.Generic;
using System.Text;

namespace DevAge.ComponentModel
{
    /// <summary>
    /// Class used to implement an empty ITypeDescriptorContext.
    /// This class seems to be required by the mono framework, ms framework accept null as ITypeDescriptorContext
    /// </summary>
    public class EmptyTypeDescriptorContext : System.ComponentModel.ITypeDescriptorContext
    {
        /// <summary>
        /// Empty ITypeDescriptorContext instance. For now I use null because mono seems to don't like this class (and throw anyway an exception)
        /// </summary>
        public static readonly EmptyTypeDescriptorContext Empty = null; //new EmptyTypeDescriptorContext();

        private EmptyContainer container = new EmptyContainer();

        #region ITypeDescriptorContext Members

        public System.ComponentModel.IContainer Container
        {
            get { return container; }
        }

        public object Instance
        {
            get { return null; }
        }

        public void OnComponentChanged()
        {

        }

        public bool OnComponentChanging()
        {
            return true;
        }

        public System.ComponentModel.PropertyDescriptor PropertyDescriptor
        {
            get { return null; }
        }

        #endregion

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return null;
        }

        #endregion
    }

    public class EmptyContainer : System.ComponentModel.IContainer
    {
        #region IContainer Members
        public void Add(System.ComponentModel.IComponent component, string name)
        {
            throw new NotImplementedException();
        }

        public void Add(System.ComponentModel.IComponent component)
        {
            throw new NotImplementedException();
        }

        public System.ComponentModel.ComponentCollection Components
        {
            get { return new System.ComponentModel.ComponentCollection(null); }
        }

        public void Remove(System.ComponentModel.IComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
        }
        #endregion
    }

}
