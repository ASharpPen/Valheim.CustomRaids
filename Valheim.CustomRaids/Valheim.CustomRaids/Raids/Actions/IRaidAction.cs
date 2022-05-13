using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids.Actions;

/// <summary>
/// Actions to perform for raids.
/// Eg., on stop, or start
/// </summary>
public interface IRaidAction
{
    void Execute(RaidContext context);
}
