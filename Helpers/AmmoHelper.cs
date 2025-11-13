using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ASS.Features;
using ASS.Features.Settings;
using Exiled.API.Enums;
using Exiled.API.Features;
using LabPlayer = LabApi.Features.Wrappers.Player;
using ExiledPlayer = Exiled.API.Features.Player;

namespace SSSCharacterCreator.Helpers;
#pragma warning disable 1591
public static class AmmoHelper
{
    private static readonly Config config = SssCharacterCreator.Config;
    private static readonly Translation translation = SssCharacterCreator.Translation;

    public static void GiveAmmo(ExiledPlayer player)
    {
        Dictionary<AmmoType, ushort> dictionary = GetAmmo(player.ReferenceHub);
        foreach ((AmmoType ammoType, ushort amount) in dictionary)
        {
            player.AddAmmo(ammoType, amount);
        }
    }

    private static Dictionary<AmmoType, ushort> GetAmmo(ReferenceHub hub)
    {
        ASSNetworking.TryGetSetting(LabPlayer.Get(hub), 155, out ASSTextInput? textInput);
        Dictionary<AmmoType, ushort> result = new();
        if (textInput == null)
        {
            Log.Warn("null");
            return result;
        }

        string inputText = textInput.InputtedText;


        if (string.IsNullOrWhiteSpace(inputText))
        {
            return result;
        }

        string[]? ammoEntries = inputText.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (string ammoEntry in ammoEntries)
        {
            string[]? split = ammoEntry.Split(':');
            if (split.Length != 2)
            {
                continue;
            }

            string ammoTypeText = split[0].Trim();

            if (!Enum.TryParse(ammoTypeText, true, out AmmoType ammoType))
            {
                continue;
            }

            MemberInfo? member = typeof(AmmoType).GetMember(ammoType.ToString()).FirstOrDefault();
            if (member?.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length > 0)
            {
                continue;
            }

            if (!ushort.TryParse(split[1].Trim(), out ushort amount))
            {
                continue;
            }

            result[ammoType] = amount;
        }

        return result;
    }

    public static Dictionary<AmmoType, ushort> GetAmmo(string inputText)
    {
        Dictionary<AmmoType, ushort> result = new();

        if (string.IsNullOrWhiteSpace(inputText))
        {
            return result;
        }

        string[]? ammoEntries = inputText.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (string ammoEntry in ammoEntries)
        {
            string[]? split = ammoEntry.Split(':');
            if (split.Length != 2)
            {
                continue;
            }

            string ammoTypeText = split[0].Trim();

            if (!Enum.TryParse(ammoTypeText, true, out AmmoType ammoType))
            {
                continue;
            }

            MemberInfo? member = typeof(AmmoType).GetMember(ammoType.ToString()).FirstOrDefault();
            if (member?.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length > 0)
            {
                continue;
            }

            if (!ushort.TryParse(split[1].Trim(), out ushort amount))
            {
                continue;
            }

            result[ammoType] = amount;
        }

        return result;
    }

    public static string AmmoToString(Dictionary<AmmoType, ushort>? ammoDictionary)
    {
        if (ammoDictionary == null || ammoDictionary.Count == 0)
        {
            return translation.AmmoNoneText;
        }

        StringBuilder builder = new();

        foreach ((AmmoType ammoType, ushort amount) in ammoDictionary)
        {
            builder.AppendLine(config.AmmoDisplayFormat.Replace("%ammotype%", ammoType.ToString())
                .Replace("%amount%", amount.ToString()));
        }

        return builder.ToString();
    }
}
