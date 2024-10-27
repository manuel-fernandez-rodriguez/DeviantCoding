using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviantCoding.Registerly.Strategies
{
    public interface ILifetimeStrategy
    {
        ServiceLifetime Lifetime { get; }
    }
}
