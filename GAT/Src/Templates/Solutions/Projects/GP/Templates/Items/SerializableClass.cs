/// <summary>
/// Summary description for $safeitemrootname$
/// </summary>

#region Using declaration
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
#endregion

namespace $rootnamespace$
{
	[Serializable]
	public class $safeitemrootname$
	{
		$SerializableType$ $SerializableField$;

		[NonSerialized]
		$NonSerializableType$ $NonSerializableField$;

		public $safeitemrootname$()
		{
		}
	}
}