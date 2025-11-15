using System.Collections.Generic;
using System.Linq;
using ASS.Features;
using ASS.Features.Settings;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using SSSCharacterCreator.Extensions;
using SSSCharacterCreator.ServerSpecific;
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
        List<ItemType> items = GetPlayerItems(player.ToLab(), out List<CustomItem>? customItems, out bool add);
        foreach (ItemType itemType in items)
        {
            Log.Info(itemType.ToString());
            player.AddItem(itemType);
        }

        if (!customItems.IsEmpty())
        {
            foreach (CustomItem customItem in customItems)
            {
                CustomItem.TryGive(player, customItem.Id);
            }
        }

        addKeycard = add;
    }


    private static List<ItemType> GetPlayerItems(LabPlayer player, out List<CustomItem> customItems,
        out bool addKeycard)
    {
        addKeycard = false;
        List<ItemType> items = [];
        customItems = [];

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
            else
            {
                if (CharacterCreator.CustomItems.FirstOrDefault(kv => kv.Name == selectedText) is { } customItem)
                {
                    customItems.Add(customItem);
                }
            }

            Log.Debug($"{id} selected: {selectedText}");
        }

        return items;
    }
}
