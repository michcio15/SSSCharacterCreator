using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SSSCharacterCreator;
#pragma warning disable 1591
public class Translation : ITranslation
{
    #region CharacterCreatorSettings

    [Description("===CHARACTER CREATOR MAIN SETTINGS TEXT---")]
    public string CharacterCreatorHeaderText { get; set; } =
        "\U0001F464 <color=#673873>\U0001F1E8</color><color=#623E7A>\U0001F1ED</color><color=#5D4481>\U0001F1E6</color><color=#584A88>\U0001F1F7</color><color=#53508F>\U0001F1E6</color><color=#4E5696>\U0001F1E8</color><color=#495C9D>\U0001F1F9</color><color=#4362A4>\U0001F1EA</color><color=#3F68AB>\U0001F1F7</color> <color=#3A6EB2>\U0001F1E8</color><color=#3574B9>\U0001F1F7</color><color=#307AC0>\U0001F1EA</color><color=#2B80C7>\U0001F1E6</color><color=#2686CE>\U0001F1F9</color><color=#218CD5>\U0001F1F4</color><color=#1798E3>\U0001F1F7</color> \U0001F464";

    public string CharacterCreatorCharacterNameText { get; set; } = "Character's Name";

    public string CharacterCreatorCharacterNamePlaceholder { get; set; } = "John";
    public string CharacterCreatorCharacterDescriptionText { get; set; } = "Character's Custom Info";

    public string CharacterCreatorCharacterDescriptionHint { get; set; } = "Is interesting";

    public string CharacterCreatorRoleText { get; set; } = "Role";

    public string CharacterCreatorSpawnText { get; set; } = "Spawn";

    public string CharacterCreatorCreateButtonText { get; set; } = "Create Character";

    public string CharacterCreatorCreateButtonTooltip { get; set; } = "Create";

    #endregion

    #region EffectsSettings

    public string EffectsInputText { get; set; } = "Effects Input";

    [Description("Its important to let the player know abt the format wich is\nEffectType:Duration,Intensity;")]
    public string EffectsInputHint { get; set; } = "Format -> EffectType:Duration,Intensity;";

    public string EffectsDisplayText { get; set; } = "<color=#4DFFB8>Effects list</color>";
    public string EffectsNoneText { get; set; } = "None";

    #endregion

    #region AmmoSetttings

    public string AmmoInputText { get; set; } = "Ammo Input";

    [Description("Plugin reads ammy with AmmoType : Amount;\nso its important to let the player know how to use it")]
    public string AmmoInputHint { get; set; } = "Format -> AmmoType:Amount;";

    public string AmmoDisplayText { get; set; } = "<color=#4DFFB8>Ammo</color>";
    public string AmmoNoneText { get; set; } = "None";

    #endregion

    #region ItemSettings

    public string Item1Text { get; set; } = "Item 1";
    public string Item2Text { get; set; } = "Item 2";
    public string Item3Text { get; set; } = "Item 3";
    public string Item4Text { get; set; } = "Item 4";
    public string Item5Text { get; set; } = "Item 5";
    public string Item6Text { get; set; } = "Item 6";
    public string Item7Text { get; set; } = "Item 7";
    public string Item8Text { get; set; } = "Item 8";

    #endregion

    #region Scale Settings

    [Description("---SCALE SETTINGS TEXT")]
    public string ScaleXSettingText { get; set; } = "Scale X";

    public string ScaleXSettingHint { get; set; } = "Width";
    public string ScaleYSettingText { get; set; } = "Scale Y";
    public string ScaleYSettingHint { get; set; } = "Height";

    public string ScaleZSettingText { get; set; } = "Scale Z";
    public string ScaleZSettingHint { get; set; } = "Length";

    #endregion

    #region KeycardCreator

    [Description("---Keycard Creator---")]
    public string KeycardCreatorHeaderText { get; set; } =
        "\U0001F4B3 <color=#D3B522>\U0001F1F0\U0001F1EA\U0001F1FE\U0001F1E8\U0001F1E6\U0001F1F7\U0001F1E9 \U0001F1E8\U0001F1F7\U0001F1EA\U0001F1E6\U0001F1F9\U0001F1F4\U0001F1F7</color> \U0001F4B3";

    public string KeycardItemName { get; set; } = "Keycard Inventory Text";
    public string KeycardLabelInputText { get; set; } = "Keycard Label Text";
    public string KeycardHolderNameText { get; set; } = "Keycard Holder Name";
    public string KeycardTypeDropdownText { get; set; } = "Keycard Type";
    public string KeycardColorInputText { get; set; } = "Keycard Color Input";

    [Description("&hex& is replaced with the text inputted in KeycardColorInput")]
    public string KeycardColorDisplayText { get; set; } = "Card color: <color=%hex%>███████████</color>";

    [Description("&hex& is replaced with the text inputted in PermissionColorInput")]
    public string PermissionColorDisplayText { get; set; } = "Permission color: <color=%hex%>███████████</color>";

    public string PermissionColorInputText { get; set; } = "Permission Color Input";
    public string ContainmentLevelPermissionSliderText { get; set; } = "Containment Permission";
    public string ArmoryLevelPermissionSliderText { get; set; } = "Armory Permission";
    public string AdminLevelPermissionSliderText { get; set; } = "Admin Permission";
    public string WearLevelSliderText { get; set; } = "Wear Level";
    public string SerialNumberInputText { get; set; } = "Serial Number";
    public string RankIndexSliderText { get; set; } = "Rank Index";

    #endregion
}
