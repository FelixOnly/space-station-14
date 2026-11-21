using Content.Shared.Alert;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._TP.Mechs.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class StepfatherComponent : Component
{
    [DataField]
    public ProtoId<AlertPrototype> BatteryAlert = "BorgBattery";

    [DataField]
    public ProtoId<AlertPrototype> NoBatteryAlert = "BorgBatteryNone";

    [DataField]
    public float ResetDrawTime;

    [DataField]
    public bool IsSubverted;

    [DataField]
    public float DrainAccumulator;

    /// <summary>
    ///     The first occupant who entered. Locked in until they die.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Pilot;

    /// <summary>
    ///     Inventory slot holding the built-in pilot pinpointer.
    /// </summary>
    [DataField]
    public string PilotPinpointerSlot = "pocket1";

    [DataField]
    public float MechToPilotDamageMultiplier;

    /// <summary>
    ///     Played when a pilot climbs into the Rotund.
    /// </summary>
    [DataField]
    public SoundSpecifier? EnterSound = new SoundPathSpecifier("/Audio/Mecha/mechmove03.ogg");
}
