using AutoMapper;
using System.Collections.Generic;

namespace Parleo.BLL.Extensions
{
    public interface IMapperFactory
    {
        IMapper GetMapper(string mapperName = "");
    }

    public class MapperFactory : IMapperFactory
    {
        public Dictionary<string, IMapper> Mappers { get; set; } = new Dictionary<string, IMapper>();

        public IMapper GetMapper(string mapperName)
        {
            return Mappers[mapperName];
        }
    }
}
