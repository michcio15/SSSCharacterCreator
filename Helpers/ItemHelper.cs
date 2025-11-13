using System.Collections.Generic;
using ASS.Features;
using ASS.Features.Settings;
using Exiled.API.Features;
using SSSCharacterCreator.Extensions;
using LabPlayer = LabApi.Features.Wrappers.Player;
using ExiledPlayer = Exiled.API.Features.Player;

namespace SSSCharacterCreator.Helpers;
#pragma warning disable 1591
public static class ItemHelper
{
    private static readonly Config config = SssCharacterCreator.Config;
    private static readonly Translation translation = SssCharacterCreator.Translation;

    public static void GiveItems(ExiledPlayer player, out bool addKeycard)
    {
        List<ItemType> items = GetPlayerItems(player.ToLab(), out bool add);
        foreach (ItemType itemType in items)
        {
            Log.Info(itemType.ToString());
            player.AddItem(itemType);
        }

        addKeycard = add;
    }

    private static List<ItemType> GetPlayerItems(LabPlayer player, out bool addKeycard)
    {
        addKeycard = false;
        List<ItemType> items = [];

        int[] itemsId = [162, 163, 164, 165, 166, 167, 168, 169];

        foreach (int id in itemsId)
        {
            ASSNetworking.TryGetSetting(player, id, out ASSDropdown? dropdown);
            if (dropdown == null)
            {
                return items;
            }

            string selectedText = dropdown.OptionSelected;
            if (config.Items.TryGetValue(selectedText, out ItemType item) &&
                item != ItemType.None)
            {
                items.Add(item);
            }
            else if (selectedText == config.CustomCardItemName)
            {
                addKeycard = true;
            }
        }

        return items;
    }
}
