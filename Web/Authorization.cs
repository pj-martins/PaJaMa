using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Web
{
	public enum AuthorizationType : int
	{
		GetEntities,
		GetEntity,
		InsertEntity,
		UpdateEntity,
		DeleteEntity
	}

	public class AuthorizationEventArgs : EventArgs
	{
		public AuthorizationType AuthorizationType { get; private set; }
		public Type EntityType { get; private set; }
		public Type EntityDtoType { get; private set; }
		public bool IsUnauthorized { get; set; }

		public AuthorizationEventArgs(AuthorizationType authorizationType, IRepository repository)
		{
			this.AuthorizationType = authorizationType;
			var args = repository.GetType().GetGenericArguments();
			this.EntityType = args[1];
			this.EntityDtoType = args[2];
		}
	}

	public delegate void AuthorizationEventHandler(object sender, AuthorizationEventArgs e);
}
