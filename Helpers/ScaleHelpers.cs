using ASS.Features;
using ASS.Features.Settings;
using Exiled.API.Features;
using UnityEngine;
using Player = LabApi.Features.Wrappers.Player;

namespace SSSCharacterCreator.Helpers;
#pragma warning disable 1591
public static class ScaleHelpers
{
    public static Vector3 GetScale(Player player)
    {
        float x = 1f;
        float y = 1f;
        float z = 1f;

        if (ASSNetworking.TryGetSetting(player, 159, out ASSSlider? scaleX))
        {
            x = scaleX.Value;
        }
        else
        {
            Log.Warn($"{player.Nickname} missing X scale slider (ID 159) for {player.Nickname}");
        }

        if (ASSNetworking.TryGetSetting(player, 160, out ASSSlider? scaleY))
        {
            y = scaleY.Value;
        }
        else
        {
            Log.Warn($"{player.Nickname} missing Y scale slider (ID 160) for {player.Nickname}");
        }

        if (ASSNetworking.TryGetSetting(player, 161, out ASSSlider? scaleZ))
        {
            z = scaleZ.Value;
        }
        else
        {
            Log.Warn($"{player.Nickname} missing Z scale slider (ID 161)  for {player.Nickname}");
        }

        return new Vector3(x, y, z);
    }
}
