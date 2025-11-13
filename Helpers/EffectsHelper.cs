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
public static class EffectsHelper
{
    private static readonly Config config = SssCharacterCreator.Config;
    private static readonly Translation translation = SssCharacterCreator.Translation;

    public static void GiveEffects(ExiledPlayer player, Dictionary<EffectType, Dictionary<int, byte>> effects)
    {
        foreach ((EffectType effectType, Dictionary<int, byte>? value) in effects)
        {
            foreach ((int duration, byte intensity) in value)
            {
                player.EnableEffect(effectType, intensity, duration);
                Log.Info($"Applied effect {effectType} | Duration: {duration}, Intensity: {intensity}");
            }
        }
    }

    public static Dictionary<EffectType, Dictionary<int, byte>> GetEffects(ReferenceHub hub)
    {
        ASSNetworking.TryGetSetting(LabPlayer.Get(hub), 155, out ASSTextInput? textInput);
        Dictionary<EffectType, Dictionary<int, byte>> result = new();

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

        string[]? effects = inputText.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (string effect in effects)
        {
            string[]? effectSplit = effect.Split(':');
            if (effectSplit.Length != 2)
            {
                continue;
            }

            string effectTypeText = effectSplit[0].Trim();
            if (!Enum.TryParse(effectTypeText, true, out EffectType effectType))
            {
                continue;
            }

            MemberInfo? member = typeof(EffectType).GetMember(effectType.ToString()).FirstOrDefault();
            if (member?.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length > 0)
            {
                continue;
            }

            string[]? values = effectSplit[1].Split(',');
            if (values.Length != 2)
            {
                continue;
            }

            if (!int.TryParse(values[0].Trim(), out int duration))
            {
                continue;
            }

            if (!byte.TryParse(values[1].Trim(), out byte intensity))
            {
                continue;
            }

            result[effectType] = new Dictionary<int, byte> { { duration, intensity } };
        }

        return result;
    }

    public static Dictionary<EffectType, Dictionary<int, byte>>? GetEffects(string inputText)
    {

        Dictionary<EffectType, Dictionary<int, byte>> result = new();

        if (string.IsNullOrWhiteSpace(inputText))
        {
            return result;
        }

        string[]? effects = inputText.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (string effect in effects)
        {
            string[]? effectSplit = effect.Split(':');
            if (effectSplit.Length != 2)
            {
                continue;
            }

            string effectTypeText = effectSplit[0].Trim();
            if (!Enum.TryParse(effectTypeText, true, out EffectType effectType))
            {
                continue;
            }

            MemberInfo? member = typeof(EffectType).GetMember(effectType.ToString()).FirstOrDefault();
            if (member?.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length > 0)
            {
                continue;
            }

            string[]? values = effectSplit[1].Split(',');
            if (values.Length != 2)
            {
                continue;
            }

            if (!int.TryParse(values[0].Trim(), out int duration))
            {
                continue;
            }

            if (!byte.TryParse(values[1].Trim(), out byte intensity))
            {
                continue;
            }

            result[effectType] = new Dictionary<int, byte> { { duration, intensity } };
        }

        return result;
    }

    public static string EffectsToString(Dictionary<EffectType, Dictionary<int, byte>>? effects)
    {
        if (effects == null || effects.Count == 0)
        {
            return translation.EffectsNoneText;
        }

        StringBuilder builder = new();

        foreach (KeyValuePair<EffectType, Dictionary<int, byte>> effect in effects)
        {
            foreach ((int duration, byte intensity) in effect.Value)
            {
                string durationToWrite = duration == 0 ? "∞" : duration.ToString();
                builder.AppendLine(
                    config.EffectsDisplayFormat
                        .Replace("%effecttype%", effect.Key.ToString())
                        .Replace("%duration%", durationToWrite)
                        .Replace("%intensity%", intensity.ToString())
                );
            }
        }

        return builder.ToString();
    }
}
