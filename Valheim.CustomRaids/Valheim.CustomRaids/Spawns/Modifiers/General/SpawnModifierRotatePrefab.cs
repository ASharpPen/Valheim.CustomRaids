using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Spawns.Modifiers.General;

public class SpawnModifierRotatePrefab : ISpawnModifier
{
    private static SpawnModifierRotatePrefab _instance;

    public static SpawnModifierRotatePrefab Instance
    {
        get
        {
            return _instance ??= new SpawnModifierRotatePrefab();
        }
    }

    public void Modify(SpawnContext context)
    {
        if (context.Spawn is null)
        {
            return;
        }

        float rotationX = context.Config.RotationX.Value;
        float rotationY = context.Config.RotationY.Value;
        float rotationZ = context.Config.RotationZ.Value;
#if DEBUG
        Log.LogDebug($"Rotating object: x{rotationX}, y{rotationY}, z{rotationZ}");
#endif
        context.Spawn.transform.Rotate(rotationX, rotationY, rotationZ);
    }
}
