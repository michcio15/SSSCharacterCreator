using System.Collections.Generic;
using System.Linq;
using ASS.Features;
using ASS.Features.Collections;
using ASS.Features.Settings;
using ASS.Features.Settings.Displays;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Permissions.Extensions;
using MEC;
using PlayerRoles;
using SSSCharacterCreator.Extensions;
using SSSCharacterCreator.Helpers;
using TMPro;
using UnityEngine;
using LabPlayer = LabApi.Features.Wrappers.Player;
using ExiledPlayer = Exiled.API.Features.Player;

namespace SSSCharacterCreator.ServerSpecific;
#pragma warning disable 1591
// ReSharper disable once ClassNeverInstantiated.Global
public class CharacterCreator
{
    public const int KeycardItemNameInputID = 172;
    public const int KeycardLabelInputID = 173;
    public const int KeycardTypeDropdownID = 174;
    public const int KeycardColorInputID = 175;
    public const int KeycardColorDisplayID = 176;
    public const int KeycardPermissionColorInputID = 177;
    public const int KeycardPermissionColorDisplayID = 178;
    public const int KeycardContainmentPermissionSliderID = 179;
    public const int KeycardArmoryPermissionSliderID = 180;
    public const int KeycardAdminPermissionSliderID = 181;
    public const int KeycardWearLevelSliderID = 182;
    public const int KeycardHolderNameInputID = 183;
    public const int KeycardSerialLabelInputID = 184;
    public const int KeycardRankIndexSliderID = 185;

    private static readonly Config config = SssCharacterCreator.Config;
    private static readonly Translation translation = SssCharacterCreator.Translation;

    private static readonly Dictionary<LabPlayer, PlayerMenu> menus = new();
    private static readonly Dictionary<LabPlayer, float> lastUpdateTime = new();

    //TODO : Zrobic ze sie odswieza tylko jezeli cos tam no ten
    //private static readonly Dictionary<LabPlayer, bool> previousItem8IsKeycard = new();
    private static readonly string[] characterRoleList = DictionaryUtils.DictionaryToList(config.Role);
    private static readonly string[] spawnLocationList = DictionaryUtils.DictionaryToList(config.SpawnLocation);
    private static readonly string[] itemsList = DictionaryUtils.DictionaryToList(config.Items);

    private static readonly string[] customCardItemsList =
        new[] { config.CustomCardItemName }.Concat(itemsList).ToArray();


    private static ASSGroup Generator(LabPlayer owner)
    {
        List<ASSBase> settings =
        [
            new ASSHeader(-1, translation.CharacterCreatorHeaderText),
            new ASSTextInput(151, translation.CharacterCreatorCharacterNameText, "Jan", "Jan"),
            new ASSTextInput(152, translation.CharacterCreatorCharacterDescriptionText, "",
                "Ma na sobie czarny płaszcz"),

            new ASSDropdown(153, translation.CharacterCreatorRoleText, characterRoleList),
            new ASSDropdown(154, translation.CharacterCreatorSpawnText, spawnLocationList),
            new ASSTextInput(155, translation.EffectsInputText, hint: translation.EffectsInputHint,
                onChanged: ChangeEffectsDisplay),

            new ASSTextDisplay(156, $"{translation.EffectsDisplayText} : \n{translation.EffectsNoneText}"),
            new ASSTextInput(157, translation.AmmoInputText, hint: translation.AmmoInputHint,
                onChanged: ChangeAmmoDisplay),

            new ASSTextDisplay(158, $"{translation.AmmoDisplayText} : \n{translation.AmmoNoneText}"),
            new ASSSlider(159, translation.ScaleXSettingText, 1f, 0.1f, 2f, hint: translation.ScaleXSettingHint),
            new ASSSlider(160, translation.ScaleYSettingText, 1f, 0.1f, 2f, hint: translation.ScaleYSettingHint),
            new ASSSlider(161, translation.ScaleZSettingText, 1f, 0.1f, 2f, hint: translation.ScaleZSettingHint),
            new ASSDropdown(162, translation.Item1Text, itemsList),
            new ASSDropdown(163, translation.Item2Text, itemsList),
            new ASSDropdown(164, translation.Item3Text, itemsList),
            new ASSDropdown(165, translation.Item4Text, itemsList),
            new ASSDropdown(166, translation.Item5Text, itemsList),
            new ASSDropdown(167, translation.Item6Text, itemsList),
            new ASSDropdown(168, translation.Item7Text, itemsList),
            new ASSDropdown(169, translation.Item8Text, customCardItemsList, onChanged: Update),
            new ASSButton(170, translation.CharacterCreatorCreateButtonText,
                translation.CharacterCreatorCreateButtonTooltip, 0.5f, "Kliknij", CreateCharacter)
        ];

        if (!ValidKeycardCreator(owner))
        {
            Log.Debug($"Generator called for {owner.Nickname}");
            return new ASSGroup(settings, 0, p => p == owner && Valid(p));
        }

        List<ASSBase> keycardSettings =
        [
            new ASSHeader(171, translation.KeycardCreatorHeaderText),
            new ASSTextInput(KeycardItemNameInputID, translation.KeycardItemName),
            new ASSDropdown(KeycardTypeDropdownID, translation.KeycardTypeDropdownText, config.KeycardTypesMap,
                onChanged: Update),

            new ASSTextInput(KeycardColorInputID, translation.KeycardColorInputText, hint: "Format (HEX) -> #RRGGBB",
                placeholder: "#123456",
                onChanged: KeycardCreatorHelpers.ChangeKeycardColorDisplay),

            new ASSTextDisplay(KeycardColorDisplayID, translation.KeycardColorDisplayText.Replace("%hex%", "#FFFFFF")),
            new ASSTextInput(KeycardPermissionColorInputID, translation.PermissionColorInputText,
                placeholder: "#123456",
                hint: "Format (HEX) -> #RRGGBB", onChanged: KeycardCreatorHelpers.ChangeKeycardPermissionColorDisplay),

            new ASSTextDisplay(KeycardPermissionColorDisplayID,
                translation.PermissionColorDisplayText.Replace("%hex%", "#FFFFFF")),

            new ASSSlider(KeycardContainmentPermissionSliderID, translation.ContainmentLevelPermissionSliderText, 0f,
                0f, 3f, true),

            new ASSSlider(KeycardArmoryPermissionSliderID, translation.ArmoryLevelPermissionSliderText, 0f, 0f, 3f,
                true),

            new ASSSlider(KeycardAdminPermissionSliderID, translation.AdminLevelPermissionSliderText, 0f, 0f, 3f, true)
        ];

        if (ASSNetworking.TryGetSetting(owner, KeycardTypeDropdownID, out ASSDropdown? dropdown))
        {
            switch (dropdown.IndexSelected)
            {
                case 0:
                    keycardSettings.Add(new ASSTextInput(KeycardLabelInputID, translation.KeycardLabelInputText));
                    keycardSettings.Add(new ASSTextInput(KeycardHolderNameInputID, translation.KeycardHolderNameText));
                    keycardSettings.Add(new ASSSlider(KeycardWearLevelSliderID, translation.WearLevelSliderText, 0f, 0f,
                        3f, true));
                    break;
                case 1:
                    keycardSettings.Add(new ASSTextInput(KeycardHolderNameInputID, translation.KeycardHolderNameText));
                    keycardSettings.Add(new ASSTextInput(KeycardSerialLabelInputID, translation.SerialNumberInputText,
                        characterLimit: 12, contentType: TMP_InputField.ContentType.IntegerNumber));
                    keycardSettings.Add(new ASSSlider(KeycardRankIndexSliderID, translation.RankIndexSliderText, 0f, 0f,
                        3f, true));
                    break;
                case 2:
                    keycardSettings.Add(new ASSTextInput(KeycardLabelInputID, translation.KeycardLabelInputText));
                    break;
                case 3:
                    keycardSettings.Add(new ASSTextInput(KeycardLabelInputID, translation.KeycardLabelInputText));
                    keycardSettings.Add(new ASSTextInput(KeycardHolderNameInputID, translation.KeycardHolderNameText));
                    keycardSettings.Add(new ASSSlider(KeycardWearLevelSliderID, translation.WearLevelSliderText, 0f, 0f,
                        3f, true));
                    keycardSettings.Add(new ASSSlider(KeycardRankIndexSliderID, translation.RankIndexSliderText, 0f, 0f,
                        3f, true));
                    break;
            }
        }

        settings.AddRange(keycardSettings);
        Log.Debug($"Generator called for {owner.Nickname}");
        return new ASSGroup(settings, 0, p => p == owner && Valid(p));
    }

    private static void Update(LabPlayer player, ASSBase assBase)
    {
        if (lastUpdateTime.TryGetValue(player, out float lastTime) && Time.realtimeSinceStartup - lastTime < 0.05f)
        {
            return;
        }

        lastUpdateTime[player] = Time.realtimeSinceStartup;

        if (menus.TryGetValue(player, out PlayerMenu? menu))
        {
            menu.Update(false, true, true);
        }
    }

    private static bool Valid(LabPlayer player) => player.ToExiled().CheckPermission(config.CharacterCreatorPermission);


    private static bool ValidKeycardCreator(LabPlayer player)
    {
        ASSNetworking.TryGetSetting(player, 169, out ASSDropdown? dropdown);
        if (dropdown is null || dropdown.OptionSelected != config.CustomCardItemName)
        {
            Log.Debug($"{player.Nickname} cant have KeycardCreator");
            return false;
        }

        Log.Debug($"{player.Nickname} can have keycard creator");

        return true;
    }

    private static void CreateCharacter(LabPlayer labPlayer, ASSBase assBase)
    {
        ExiledPlayer player = labPlayer.ToExiled();
        if (player.IsAlive || !Round.IsStarted)
        {
            return;
        }

        if (!Valid(labPlayer))
        {
            return;
        }

        if (!ASSNetworking.TryGetSetting(labPlayer, 151, out ASSTextInput? nameInput))
        {
            Log.Debug($"{player.Nickname} nameInput is null");
            return;
        }

        string playerName = nameInput.InputtedText;

        if (!ASSNetworking.TryGetSetting(labPlayer, 152, out ASSTextInput? descriptionInput))
        {
            Log.Debug($"{player.Nickname} descriptionInput is null");
            return;
        }

        string description = descriptionInput.InputtedText;

        if (!ASSNetworking.TryGetSetting(labPlayer, 153, out ASSDropdown? roleDropdown))
        {
            Log.Debug($"{player.Nickname} roleDropdown is null");
            return;
        }

        RoleTypeId roleTypeId = config.Role[roleDropdown.OptionSelected];

        player.Role.Set(roleTypeId, SpawnReason.ForceClass, RoleSpawnFlags.None);
        player.ClearAmmo();
        player.ClearInventory();

        player.CustomName = string.IsNullOrWhiteSpace(playerName)
            ? player.Nickname
            : config.CustomNameFormat.Replace("%customname%", playerName)
                .Replace("%nickname%", player.Nickname);

        player.CustomInfo = description;

        AmmoHelper.GiveAmmo(player);

        ItemHelper.GiveItems(player, out bool keycard);
        if (keycard)
        {
            KeycardCreatorHelpers.CreateKeycard(labPlayer);
        }

        player.Scale = ScaleHelpers.GetScale(labPlayer);

        player.Teleport(GetPlayerSpawnLocation(player.ToLab()));

        Timing.CallDelayed(1f,
            () => EffectsHelper.GiveEffects(player, EffectsHelper.GetEffects(labPlayer.ReferenceHub)));
    }

    private static Vector3 GetPlayerSpawnLocation(LabPlayer player)
    {
        if (!ASSNetworking.TryGetSetting(player, 154, out ASSDropdown? roleDropdown))
        {
            Log.Warn($"{player.Nickname} roleDropdown is null");
            return SpawnLocationType.InsideLczWc.GetPosition();
        }

        SpawnLocationType spawnLocationType = config.SpawnLocation[roleDropdown.OptionSelected];
        return spawnLocationType.GetPosition();
    }

    private static void ChangeEffectsDisplay(LabPlayer player, ASSBase assBase)
    {
        if (assBase is ASSTextInput textInput)
        {
            ASSNetworking.TryGetSetting(player, 156, out ASSTextDisplay? textDisplay);
            if (textDisplay == null)
            {
                Log.Warn($"{player.Nickname} textDisplay is null");
                return;
            }

            Log.Debug($"Change display for {player.Nickname}");
            textDisplay.UpdateLabel(
                $"{translation.EffectsDisplayText} : \n{EffectsHelper.EffectsToString(EffectsHelper.GetEffects(textInput.InputtedText))}",
                [player]);
        }
    }

    private static void ChangeAmmoDisplay(LabPlayer player, ASSBase assBase)
    {
        // ReSharper disable once InvertIf
        if (assBase is ASSTextInput textInput)
        {
            ASSNetworking.TryGetSetting(player, 158, out ASSTextDisplay? textDisplay);
            if (textDisplay == null)
            {
                Log.Warn($"{player.Nickname} textDisplay is null");
                return;
            }

            Log.Debug($"Change display for {player.Nickname}");
            textDisplay.UpdateLabel(
                $"{translation.AmmoDisplayText} : \n{AmmoHelper.AmmoToString(AmmoHelper.GetAmmo(textInput.InputtedText))}",
                [player]);
        }
    }

    #region Events

    #region ASS Events

    public static void OnVerified(VerifiedEventArgs ev)
    {
        foreach ((_, PlayerMenu? playerMenu) in menus)
        {
            playerMenu.Update(false, true, true);
        }

        menus[ev.Player.ToLab()] = new PlayerMenu(Generator, ev.Player.ToLab());
    }

    public static void OnLeft(LeftEventArgs ev)
    {
        LabPlayer player = ev.Player.ToLab();
        if (!menus.TryGetValue(player, out PlayerMenu menu))
        {
            return;
        }

        Log.Debug($"Destroying menu for {ev.Player.Nickname}");
        menu.Destroy();

        /*foreach (KeyValuePair<LabPlayer, PlayerMenu> kvp in Menus)
        {
            kvp.Value.Update(false, true);
        }*/
    }

    #endregion

    public static void OnDied(DiedEventArgs ev) => ev.Player.Scale = Vector3.one;

    #endregion
}

public static class DictionaryUtils
{
    public static string[] DictionaryToList<T>(Dictionary<string, T> dict) => dict.Keys.ToArray();
}
