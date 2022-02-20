using System;

namespace Valheim.CustomRaids.Core.Configuration
{
    [Serializable]
    public abstract class Config
    {
        public string SectionName;

        public string SectionKey;
    }
}
