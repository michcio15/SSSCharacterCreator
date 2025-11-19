// ngl used ai for ts since i dont hate myself that much
using System.IO;
using Exiled.API.Features;
using Newtonsoft.Json;
using SSSCharacterCreator.Features;

namespace SSSCharacterCreator.Helpers;

public static class JsonHelpers
{
    private static readonly JsonSerializerSettings settings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore
    };

    public static void CharactersFolderCreated(out string? path)
    {
        string? configDir = Path.GetDirectoryName(SssCharacterCreator.Instance?.ConfigPath);
        if (configDir == null)
        {
            Log.Warn("cant load config path");
            path = null;
            return;
        }

        string charactersFolder = Path.Combine(configDir, "Characters");

        if (!Directory.Exists(charactersFolder))
        {
            Directory.CreateDirectory(charactersFolder);
            Log.Debug($"Created characters folder: {charactersFolder}");
        }
        else
        {
            Log.Debug("characters folder already exists");
        }

        path = charactersFolder;
    }

    public static void SaveCharacterInSlot(string userId, int slotIndex, CustomCharacter character)
    {
        if (slotIndex is < 0 or >= 5)
        {
            Log.Warn("Wrong index for slot");
            return;
        }

        CharacterSlots slots = LoadCharacterSlots(userId);
        slots.Characters[slotIndex].CustomCharacter = character;
        SaveCharacterSlots(userId, slots);

        Log.Debug($"Saved character in  {slotIndex} for {userId}");
    }

    private static void SaveCharacterSlots(string userId, CharacterSlots slots)
    {
        string filePath = Path.Combine(SssCharacterCreator.CharactersFolderPath, $"{userId}.json");
        string json = JsonConvert.SerializeObject(slots, settings);
        File.WriteAllText(filePath, json);

        Log.Debug($"saved json for  {userId}: path {filePath}");
    }

    public static CharacterSlots LoadCharacterSlots(string userId)
    {
        string filePath = Path.Combine(SssCharacterCreator.CharactersFolderPath, $"{userId}.json");

        if (!File.Exists(filePath))
        {
            Log.Warn($"no json for{userId} making a new one");
            CharacterSlots newSlots = new();
            SaveCharacterSlots(userId, newSlots);
            return newSlots;
        }

        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<CharacterSlots>(json, settings) ?? new CharacterSlots();
    }
}
