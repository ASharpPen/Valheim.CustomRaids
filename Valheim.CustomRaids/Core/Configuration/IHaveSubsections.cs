namespace Valheim.CustomRaids.Core.Configuration
{
    public interface IHaveSubsections
    {
        Config GetSubsection(string subsectionName);
    }
}
