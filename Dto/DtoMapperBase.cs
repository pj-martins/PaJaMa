using AutoMapper;
using PaJaMa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Dto
{
    public abstract class DtoMapperBase
    {
        public MapperConfiguration MapperConfig { get; private set; }
        public Dictionary<Type, object> Mappings { get; private set; }

        public DtoMapperBase()
        {
            Mappings = new Dictionary<Type, object>();
            MapperConfig = new MapperConfiguration(createMaps);
        }

		protected abstract void createMaps(IMapperConfigurationExpression cfg);

		public abstract DbContextBase GetDbContext();
	}
}
