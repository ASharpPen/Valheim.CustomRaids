using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids2.Conductors
{
    public interface IRaidConductor
    {
        public void Update(float deltaTime);

        public void Save();

        public void Load();
    }
}
