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
}
