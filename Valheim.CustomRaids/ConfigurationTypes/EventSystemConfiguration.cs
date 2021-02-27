using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.ConfigurationTypes
{
    public class EventSystemConfiguration
    {
        private ConfigFile Config;

        public ConfigEntry<float> EventCheckInterval;

        public ConfigEntry<float> EventTriggerChance;

        public void LoadConfig(ConfigFile configFile)
        {
            Config = configFile;

            EventCheckInterval = configFile.Bind<float>("EventSystem", "EventCheckInterval", 0.001f, "Frequency between checks for new raids. Value is in hours");
            EventTriggerChance = configFile.Bind<float>("EventSystem", "EventTriggerChance", 1f, "Chance of raid, per check interval.");
        }
    }
}
