using System;
using System.ComponentModel;

namespace Mvp.Xml.Design.VisualStudio
{
	/// <summary>
	/// Basic <see cref="ISite"/> implementation. Passes all service 
	/// requests to the parent service provider. 
	/// </summary>
	public class Site : ISite
	{
		#region Field & Ctor

		IServiceProvider _provider;

		/// <summary>
		/// Constructs a site.
		/// </summary>
		/// <param name="provider">The object providing services to this site.</param>
		/// <param name="component">The component this site is being associated with.</param>
		/// <param name="name">A name for the site.</param>
		public Site(IServiceProvider provider, IComponent component, string name)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			if (component == null)
				throw new ArgumentNullException("component");

			_component = component;
			// Pull the container from the service provider (if any).
			_container = (IContainer) provider.GetService(typeof(IContainer));
			_provider = provider;
			_name = name;
		}

		#endregion Field & Ctor

		#region ISite Members

		/// <summary>
		/// See <see cref="ISite.Component"/>.
		/// </summary>
		public IComponent Component
		{
			get { return _component; }
		} IComponent _component;

		/// <summary>
		/// See <see cref="ISite.Container"/>.
		/// </summary>
		public IContainer Container
		{
			get { return _container; }
		} IContainer _container;

		/// <summary>
		/// Always returns <c>false</c>.
		/// </summary>
		public bool DesignMode
		{
			get { return false; }
		} 

		/// <summary>
		/// See <see cref="ISite.Name"/>.
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		} string _name;

		#endregion

		#region IServiceProvider Members

		/// <summary>
		/// See <see cref="IServiceProvider.GetService"/>.
		/// </summary>
		public virtual object GetService(Type service)
		{
			if (service != typeof(ISite))
				return _provider.GetService(service); 

			return this;
		}

		#endregion
	}
}
