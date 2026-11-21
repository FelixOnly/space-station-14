using Content.Shared._TP.Mechs.Components;
using Content.Shared.Vehicle;

namespace Content.Shared._TP.Mechs.Systems;

/// <summary>
///     Restricts who can climb into the Rotund: the first occupant becomes the pilot,
///     and once assigned only that pilot may enter (unless the mech is subverted).
/// </summary>
public sealed partial class StepfatherEntrySystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<StepfatherComponent, ContainerVehicleEntryAttemptEvent>(OnEntryAttempt);
    }

    private void OnEntryAttempt(Entity<StepfatherComponent> ent, ref ContainerVehicleEntryAttemptEvent args)
    {
        if (ent.Comp.Pilot is { } pilot && args.Entering != pilot && !ent.Comp.IsSubverted)
            args.Cancelled = true;
    }
}
