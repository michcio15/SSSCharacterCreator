using Exiled.API.Features;

namespace SSSCharacterCreator.Extensions;

/// <summary>
///     Converters between <see cref="Exiled.API.Features.Player" /> (Exiled)
///     and <see cref="LabApi.Features.Wrappers.Player" /> (LabAPI)
/// </summary>
public static class PlayerExtension
{
    /// <summary>
    ///     Converts <see cref="LabApi.Features.Wrappers.Player" /> (LabAPI)
    ///     to <see cref="Exiled.API.Features.Player" /> (Exiled)
    /// </summary>
    /// <param name="player">LabAPI -> <see cref="LabApi.Features.Wrappers.Player" /></param>
    /// <returns>Exiled player -> <see cref="Exiled.API.Features.Player" /></returns>
    public static Player ToExiled(this LabApi.Features.Wrappers.Player player) => Player.Get(player.ReferenceHub);

    /// <summary>
    ///     Converts <see cref="Exiled.API.Features.Player" /> (Exiled)
    ///     to <see cref="LabApi.Features.Wrappers.Player" /> (LabAPI)
    /// </summary>
    /// <param name="player">Exiled player -> <see cref="Exiled.API.Features.Player" /></param>
    /// <returns>LabAPI player -> <see cref="LabApi.Features.Wrappers.Player" /></returns>
    public static LabApi.Features.Wrappers.Player ToLab(this Player player) =>
        LabApi.Features.Wrappers.Player.Get(player.ReferenceHub);
}
