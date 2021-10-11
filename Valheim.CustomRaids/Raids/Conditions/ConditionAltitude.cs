
namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionAltitude : IRaidCondition
    {
        public float? MinAltitude { get; set; }

        public float? MaxAltitude { get; set; }

        public bool IsValid(RaidContext context)
        {
            if (MinAltitude is not null)
            {
                if (context.Position.z < MinAltitude)
                {
                    return false;
                }
            }

            if (MaxAltitude is not null)
            {
                if (context.Position.z > MaxAltitude)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
