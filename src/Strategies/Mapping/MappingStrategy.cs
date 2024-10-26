using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviantCoding.Registerly.Strategies.Mapping
{
    public enum MappingStrategyEnum
    {
        AsImplementedInterfaces,
        AsSelf
    }

    public static class MappingStrategy
    {
        public static IMappingStrategy From(MappingStrategyEnum mappingStrategy)
        {
            return mappingStrategy switch
            {
                MappingStrategyEnum.AsImplementedInterfaces => new AsImplementedInterfaces(),
                MappingStrategyEnum.AsSelf => new AsSelf(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
