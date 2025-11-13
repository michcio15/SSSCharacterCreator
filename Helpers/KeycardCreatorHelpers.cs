using ASS.Features;
using ASS.Features.Settings;
using ASS.Features.Settings.Displays;
using Exiled.API.Features;
using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Wrappers;
using SSSCharacterCreator.ServerSpecific;
using UnityEngine;
using Player = LabApi.Features.Wrappers.Player;

namespace SSSCharacterCreator.Helpers;

using LabPlayer = Player;

#pragma warning disable 1591
public static class KeycardCreatorHelpers
{
    private static readonly Config config = SssCharacterCreator.Config;
    private static readonly Translation translation = SssCharacterCreator.Translation;

    public static void CreateKeycard(LabPlayer p)
    {
        ASSNetworking.TryGetSetting(p, CharacterCreator.KeycardTypeDropdownID,
            out ASSDropdown? keycardTypeDropdown);
        if (keycardTypeDropdown == null)
        {
            Log.Warn("Didnt work :(");
            return;
        }

        switch (keycardTypeDropdown.IndexSelected)
        {
            case 0:
                {
                    KeycardItem.CreateCustomKeycardSite02
                    (p, GetKeycardItemName(p), GetKeycardHolderName(p), GetKeycardLabel(p), GetKeycardPermission(p),
                        GetKeycardColor(p), GetPermissionColor(p), Color.white, GetKeycardWearLevel(p));
                    break;
                }
            case 1:
                {
                    KeycardItem.CreateCustomKeycardTaskForce(p, GetKeycardItemName(p), GetKeycardHolderName(p),
                        GetKeycardPermission(p), GetKeycardColor(p), GetPermissionColor(p), GetKeycardSerial(p),
                        GetKeycardRankIndex(p));
                    break;
                }
            case 2:
                {
                    KeycardItem.CreateCustomKeycardManagement
                    (p, GetKeycardItemName(p), GetKeycardLabel(p), GetKeycardPermission(p), GetKeycardColor(p),
                        GetPermissionColor(p), Color.white);
                    break;
                }
            case 3:
                {
                    KeycardItem.CreateCustomKeycardMetal(p, GetKeycardItemName(p), GetKeycardHolderName(p),
                        GetKeycardLabel(p), GetKeycardPermission(p), GetKeycardColor(p), GetPermissionColor(p),
                        Color.white, GetKeycardWearLevel(p), GetKeycardSerial(p));
                    break;
                }
        }
    }


    public static void ChangeKeycardColorDisplay(LabPlayer player, ASSBase assBase)
    {
        if (assBase is not ASSTextInput colorInput)
        {
            return;
        }

        ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardColorDisplayID, out ASSTextDisplay? colorDisplay);
        if (colorDisplay == null)
        {
            Log.Warn($"colorDisplay is null for {player.Nickname}");
            return;
        }

        colorDisplay.UpdateLabel(translation.KeycardColorDisplayText.Replace("%hex%", colorInput.InputtedText),
            [player]);
    }

    public static void ChangeKeycardPermissionColorDisplay(LabPlayer player, ASSBase assBase)
    {
        if (assBase is not ASSTextInput colorInput)
        {
            return;
        }

        ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardPermissionColorDisplayID,
            out ASSTextDisplay? colorDisplay);
        if (colorDisplay == null)
        {
            Log.Warn($"colorDisplay is null for {player.Nickname}");
            return;
        }

        colorDisplay.UpdateLabel(translation.PermissionColorDisplayText.Replace("%hex%", colorInput.InputtedText),
            [player]);
    }

    #region Getters

    private static string GetKeycardItemName(LabPlayer player) =>
        !ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardItemNameInputID, out ASSTextInput? textInput)
            ? string.Empty
            : textInput.InputtedText;

    private static string GetKeycardHolderName(LabPlayer player) =>
        !ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardHolderNameInputID, out ASSTextInput? textInput)
            ? string.Empty
            : textInput.InputtedText;

    private static string GetKeycardLabel(LabPlayer player) =>
        !ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardLabelInputID, out ASSTextInput? textInput)
            ? string.Empty
            : textInput.InputtedText;

    private static byte GetKeycardWearLevel(LabPlayer player) =>
        !ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardWearLevelSliderID, out ASSSlider? slider)
            ? (byte)0
            : (byte)slider.Value;

    private static byte GetKeycardRankIndex(LabPlayer player) =>
        !ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardRankIndexSliderID, out ASSSlider? slider)
            ? (byte)0
            : (byte)slider.Value;

    private static string GetKeycardSerial(LabPlayer player) =>
        !ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardSerialLabelInputID, out ASSTextInput? textInput)
            ? string.Empty
            : textInput.InputtedText;


    private static Color GetKeycardColor(LabPlayer player)
    {
        if (!ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardColorInputID, out ASSTextInput? textInput))
        {
            return Color.white;
        }

        ColorUtility.TryParseHtmlString(textInput.InputtedText, out Color color);
        return color;
    }

    private static Color GetPermissionColor(LabPlayer player)
    {
        if (!ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardPermissionColorInputID,
                out ASSTextInput? textInput))
        {
            return Color.white;
        }

        ColorUtility.TryParseHtmlString(textInput.InputtedText, out Color color);
        return color;
    }

    private static KeycardLevels GetKeycardPermission(LabPlayer player)
    {
        int containmentPermissionLevel =
            (int)(ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardContainmentPermissionSliderID,
                out ASSSlider? containmentSlider)
                ? containmentSlider!.Value
                : 0);

        int armoryPermissionLevel =
            (int)(ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardArmoryPermissionSliderID,
                out ASSSlider? armorySlider)
                ? armorySlider!.Value
                : 0);

        int adminPermissionLevel =
            (int)(ASSNetworking.TryGetSetting(player, CharacterCreator.KeycardAdminPermissionSliderID,
                out ASSSlider? adminSlider)
                ? adminSlider!.Value
                : 0);
        return new KeycardLevels(containmentPermissionLevel, armoryPermissionLevel, adminPermissionLevel);
    }

    #endregion
}
