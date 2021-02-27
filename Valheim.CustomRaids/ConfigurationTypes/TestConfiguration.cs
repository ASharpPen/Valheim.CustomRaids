using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.ConfigurationCore;

namespace Valheim.CustomRaids
{
    public class TestConfigurationGroup : ConfigurationGroup<TestConfigurationSection>
    {
        public ConfigurationEntry<string> TestGroupKey;
    }

    public class TestConfigurationSection : ConfigurationSection
    {
        public ConfigurationEntry<string> TestKeyString;

        public ConfigurationEntry<bool> TestKeyBool;

        public ConfigurationEntry<int> TestKeyInt;

        public TestConfigurationSection()
        {

        }
    }
}
