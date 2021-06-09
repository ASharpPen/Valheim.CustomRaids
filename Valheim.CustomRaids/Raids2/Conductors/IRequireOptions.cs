using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids2.Conductors
{
    public interface IRequireOptions<T> where T : IConductorOptions
    {
        void SetOptions(T options);
    }
}
