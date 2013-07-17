/// <summary>
/// Summary description for $safeitemrootname$
/// It needs to implement the methods GetObjectData
/// and a special constructor 
/// </summary>

#region Using declaration
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
#endregion

namespace $rootnamespace$
{
	[Serializable]
	public class $safeitemrootname$ : ISerializable
	{
		$SerializableType$ $SerializableField$;

		[NonSerialized]
		$NonSerializableType$ $NonSerializableField$;

		public $safeitemrootname$()
		{
		}

		#region ISerializable Members

		/// <summary>
		/// Initializes an instance of the class.
		/// </summary>
		protected $safeitemrootname$(SerializationInfo info, StreamingContext context)
		{
		}

		/// <summary>
		/// <seealso cref="ISerializable.GetObjectData"/>
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
		}

		#endregion ISerializable Members
	}
}