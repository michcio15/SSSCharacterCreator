using System.IO;
using Exiled.API.Features;
using SSSCharacterCreator.Features;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SSSCharacterCreator.Helpers;

public static class YamlHelpers
{

    private static readonly IDeserializer deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    private static readonly ISerializer serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .DisableAliases()
        .Build();

    public static void CharactersFolderCreated(out string? path)
    {
        string? configDir = Path.GetDirectoryName(SssCharacterCreator.Instance?.ConfigPath);
        if (configDir == null)
        {
            Log.Warn("Nie można znaleźć ścieżki configu.");
            path = null;
            return;
        }

        string charactersFolder = Path.Combine(configDir, "Characters");

        if (!Directory.Exists(charactersFolder))
        {
            Directory.CreateDirectory(charactersFolder);
            Log.Debug($"Utworzono folder Characters: {charactersFolder}");
        }
        else
        {
            Log.Debug("Folder Characters już istnieje.");
        }

        path = charactersFolder;
    }
    public static void SaveCharacterInSlot(string userId, int slotIndex, CustomCharacter character)
    {
        if (slotIndex is < 0 or >= 5)
        {
            Log.Warn($"Wrong index come on");
            return;
        }

        var slots = LoadCharacterSlots(userId);

        slots.Slots[slotIndex].Character = character;

        SaveCharacterSlots(userId, slots);

        Log.Debug($"Zapisano postać w slocie {slotIndex} dla UserId {userId}");
    }

    private static void SaveCharacterSlots(string userId, CharacterSlots slots)
    {
        string filePath = Path.Combine(SssCharacterCreator.CharactersFolderPath, $"{userId}.yml");
        string yml = serializer.Serialize(slots);
        File.WriteAllText(filePath, yml);
        Log.Debug($"Zapisano plik yml dla UserId {userId}: {filePath}");
    }

    public static CharacterSlots LoadCharacterSlots(string userId)
    {
        string filePath = Path.Combine(SssCharacterCreator.CharactersFolderPath, $"{userId}.yml");
        if (!File.Exists(filePath))
        {
            Log.Warn($"yml for {userId} not found creating new");
            var newSlots = new CharacterSlots();
            SaveCharacterSlots(userId, newSlots);
            return newSlots;
        }

        string yml = File.ReadAllText(filePath);
        return deserializer.Deserialize<CharacterSlots>(yml) ?? new CharacterSlots();
    }
}
