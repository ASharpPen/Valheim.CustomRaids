using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids2.Schedulers
{
    public interface IRaidScheduler
    {
        /// <summary>
        /// Update scheduler. Called on FixedUpdate with time since last.
        /// </summary>
        /// <param name="deltaTime">Seconds since last FixedUpdate.</param>
        void Update(float deltaTime);

        //void ManageRaid(string raidId, object raidOptions);

        void Save();

        void Load();
    }
}
