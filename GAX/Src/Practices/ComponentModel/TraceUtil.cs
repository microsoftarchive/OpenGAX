using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.Practices.ComponentModel
{
	/// <summary>
	/// utility class for using TraceSources that are specific and partitioned by vsix package.
	/// </summary>
	public static class TraceUtil
	{
		/// <summary>
		/// trace source for GAX package.
		/// </summary>
		public static string GaxTraceSourceName = Properties.Resources.OutputWindowService_WindowName;

		internal static Hashtable traceSources = new Hashtable(); // must come before GaxOutputWindowService's init.

		/// <summary>
		/// Gets the switch for the Recipe Fromework, set to the 
		/// level specified in the manifest file.
		/// </summary>
		public static SourceSwitch GaxSourceSwitch = new SourceSwitch("RecipeFramework.VisualStudio", SourceLevels.Error.ToString("d"));

		internal static OutputWindowService GaxOutputWindowService = new OutputWindowService(GaxTraceSourceName,
						GaxSourceSwitch);

		/// <summary>
		/// reserved from Recipe Fromework.
		/// </summary>
		public static TraceSource GaxTraceSource = GaxOutputWindowService.TraceSource;

		/// <summary>
		/// get the TraceSource for the GAX extension package. Each package has a separate TraceSource,
		/// including the gax and gat.
		/// </summary>
		/// <param name="packageName">use the caption in the configuration file.</param>
		public static TraceSource PackageTraceSource(string packageName)
		{
			return (TraceSource)traceSources[packageName];
		}

		/// <summary>
		/// get the TraceSource assigned to the package that this componnect is in.
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public static TraceSource SiteTraceSource(this IComponent component)
		{
			TraceSource s = ((IOutputWindowService)component.Site?.GetService(typeof(IOutputWindowService)))?.TraceSource;			
			if (s == null)
				s = GaxTraceSource;
			return s;
		}

		#region TraceInformation

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		/// <param name="package"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		[Conditional("TRACE")]
		public static void TraceInformation(string package, string format, params object[] args)
		{
			TraceSource s = (TraceSource)traceSources[package];
			if (s != null) s.TraceInformation(format, args);
			else
			{
				Trace.TraceWarning("Package of name: '" + package + "' has no TraceSource");
				Trace.TraceInformation(format, args);
				GaxTraceSource.TraceInformation("??package: '" + package + "'??\n" + format, args);					
			}
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceInformation(string package, string message)
		{
			TraceSource s = (TraceSource)traceSources[package];
			if (s != null) s.TraceInformation(message);
			else
			{
				Trace.TraceWarning("Package of name: '" + package + "' has no TraceSource");
				Trace.TraceInformation(message);
				GaxTraceSource.TraceInformation("??package: '" + package + "'??\n" + message);
			}
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceInformation(this ServiceContainer container, string format, params object[] args)
		{			
			TraceSource s = ((IOutputWindowService)((IServiceProvider)container).GetService(typeof(IOutputWindowService)))?.TraceSource;
			if (s != null) s.TraceInformation(format, args);
			else GaxTraceSource.TraceInformation(format, args);
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceInformation(this ServiceContainer container, string message)
		{
			TraceSource s = ((IOutputWindowService)((IServiceProvider)container).GetService(typeof(IOutputWindowService)))?.TraceSource;
			if (s != null) s.TraceInformation(message);
			else GaxTraceSource.TraceInformation(message);
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceInformation(this IComponent component, string format, params object[] args)
		{
			TraceSource s = ((IOutputWindowService)component.Site?.GetService(typeof(IOutputWindowService)))?.TraceSource;

			if (s != null) s.TraceInformation(format, args);
			else GaxTraceSource.TraceInformation(format, args);
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceInformation(this IComponent component, string message)
		{
			TraceSource s = ((IOutputWindowService)component.Site?.GetService(typeof(IOutputWindowService)))?.TraceSource;
			if (s != null) s.TraceInformation(message);
			else GaxTraceSource.TraceInformation(message);
		}
		#endregion

		#region TraceWarning
		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceWarning(string package, string format, params object[] args)
		{
			TraceSource s = (TraceSource)traceSources[package];
			if (s != null) s.TraceEvent(TraceEventType.Warning, 0, format, args);
			else
			{
				Trace.TraceWarning("Package of name: '" + package + "' has no TraceSource");
				Trace.TraceWarning(format, args);
				GaxTraceSource.TraceEvent(TraceEventType.Warning, 0, "??package: '" + package + "'??\n" + format, args);
			}
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceWarning(string package, string message)
		{
			TraceSource s = (TraceSource)traceSources[package];
			if (s != null) s.TraceEvent(TraceEventType.Warning, 0, message);
			else
			{
				Trace.TraceWarning("Package of name: '" + package + "' has no TraceSource");
				Trace.TraceWarning(message);
				GaxTraceSource.TraceEvent(TraceEventType.Warning, 0, "??package: '" + package + "'??\n" + message);
			}
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceWarning(this ServiceContainer container, string format, params object[] args)
		{
			TraceSource s = ((IOutputWindowService)((IServiceProvider)container).GetService(typeof(IOutputWindowService)))?.TraceSource;
			if (s != null) s.TraceEvent(TraceEventType.Warning, 0, format, args);
			else GaxTraceSource.TraceEvent(TraceEventType.Warning, 0, format, args);
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceWarning(this ServiceContainer container, string message)
		{
			TraceSource s = ((IOutputWindowService)((IServiceProvider)container).GetService(typeof(IOutputWindowService)))?.TraceSource;
			if (s != null) s.TraceEvent(TraceEventType.Warning, 0, message);
			else GaxTraceSource.TraceEvent(TraceEventType.Warning, 0, message);
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceWarning(this IComponent component, string format, params object[] args)
		{
			TraceSource s = ((IOutputWindowService)component.Site?.GetService(typeof(IOutputWindowService)))?.TraceSource;

			if (s != null) s.TraceEvent(TraceEventType.Warning, 0, format, args);
			else GaxTraceSource.TraceEvent(TraceEventType.Warning, 0, format, args);
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceWarning(this IComponent component, string message)
		{
			TraceSource s = ((IOutputWindowService)component.Site?.GetService(typeof(IOutputWindowService)))?.TraceSource;
			if (s != null) s.TraceEvent(TraceEventType.Warning, 0, message);
			else GaxTraceSource.TraceEvent(TraceEventType.Warning, 0, message);
		}
		#endregion

		#region TraceError
		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceError(string package, string format, params object[] args)
		{
			TraceSource s = (TraceSource)traceSources[package];
			if (s != null) s.TraceEvent(TraceEventType.Error, 0, format, args);
			else
			{
				Trace.TraceWarning("Package of name: '" + package + "' has no TraceSource");
				Trace.TraceError(format, args);
				GaxTraceSource.TraceEvent(TraceEventType.Error, 0, "??package: '" + package + "'??\n" + format, args);
			}
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceError(string package, string message)
		{
			TraceSource s = (TraceSource)traceSources[package];
			if (s != null) s.TraceEvent(TraceEventType.Error, 0, message);
			else
			{
				Trace.TraceWarning("Package of name: '" + package + "' has no TraceSource");
				Trace.TraceError(message);
				GaxTraceSource.TraceEvent(TraceEventType.Error, 0, "??package: '" + package + "'??\n" + message);
			}
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceError(this ServiceContainer container, string format, params object[] args)
		{
			TraceSource s = ((IOutputWindowService)((IServiceProvider)container).GetService(typeof(IOutputWindowService)))?.TraceSource;
			if (s != null) s.TraceEvent(TraceEventType.Error, 0, format, args);
			else GaxTraceSource.TraceEvent(TraceEventType.Error, 0, format, args);
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceError(this ServiceContainer container, string message)
		{
			TraceSource s = ((IOutputWindowService)((IServiceProvider)container).GetService(typeof(IOutputWindowService)))?.TraceSource;
			if (s != null) s.TraceEvent(TraceEventType.Error, 0, message);
			else GaxTraceSource.TraceEvent(TraceEventType.Error, 0, message);
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceError(this IComponent component, string format, params object[] args)
		{
			TraceSource s = ((IOutputWindowService)component.Site?.GetService(typeof(IOutputWindowService)))?.TraceSource;

			if (s != null) s.TraceEvent(TraceEventType.Error, 0, format, args);
			else GaxTraceSource.TraceEvent(TraceEventType.Error, 0, format, args);
		}

		/// <summary>
		/// write message to tracesource for package.
		/// </summary>
		[Conditional("TRACE")]
		public static void TraceError(this IComponent component, string message)
		{
			TraceSource s = ((IOutputWindowService)component.Site?.GetService(typeof(IOutputWindowService)))?.TraceSource;
			if (s != null) s.TraceEvent(TraceEventType.Error, 0, message);
			else GaxTraceSource.TraceEvent(TraceEventType.Error, 0, message);
		}
		#endregion	
	}
}
