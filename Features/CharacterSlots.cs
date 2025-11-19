using System.Collections.Generic;

namespace SSSCharacterCreator.Features;

public class CharacterSlots
{
    public List<CharacterSlot> Characters { get; set; } =
    [
        new() { CustomCharacter = null },
        new() { CustomCharacter = null },
        new() { CustomCharacter = null },
        new() { CustomCharacter = null },
        new() { CustomCharacter = null }
    ];
}

public class CharacterSlot
{
    public CustomCharacter? CustomCharacter { get; set; }
}
