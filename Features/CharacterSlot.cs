using System.Collections.Generic;

namespace SSSCharacterCreator.Features;

public record CharacterSlot()
{
    public CustomCharacter? Character { get; set; } = null;

}

public class CharacterSlots
{
    public List<CharacterSlot> Slots { get; set; } =
    [
        new CharacterSlot { Character = null },
        new CharacterSlot { Character = null },
        new CharacterSlot { Character = null },
        new CharacterSlot { Character = null },
        new CharacterSlot { Character = null }
    ];
}
