using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;

#pragma warning disable 1591
namespace SSSCharacterCreator;

public class Config : IConfig
{
    [Description("Format used while givin custom name\n%customname% is what player has chosen\n%nickname% is his nick")]
    public string CustomNameFormat { get; set; } = "%customname% | %nickname%";

    [Description("%ammotype% is the ammo type\n%amount% it the amount that was inputted")]
    public string AmmoDisplayFormat { get; set; } = "%ammotype%: <color=#00FFFF>%amount%</color>";

    [Description(
        "%effecttype% is the effect type\n %duration% is the int duration changes to infinity symbol if 0\n%intensity% is intensity")]
    public string EffectsDisplayFormat { get; set; } =
        "%effecttype%: <color=#00B7EB>Duration</color> = %duration%, <color=#00FFFF>Intensity</color> = %intensity%";

    [Description("Available Roles and their name")]
    public Dictionary<string, RoleTypeId> Role { get; set; } = new()
    {
        { "<color=#FF8E00>Class D</color>", RoleTypeId.ClassD },
        { "<color=#FFFF7C>Scientist</color>", RoleTypeId.Scientist },
        { "<color=#5B6370>Facility Guard</color>", RoleTypeId.FacilityGuard },
        { "<color=#70C3FF>Ntf Private</color>", RoleTypeId.NtfPrivate },
        { "<color=#0096FF>Ntf Sergeant</color>", RoleTypeId.NtfSergeant },
        { "<color=#00D9FF>Ntf Specialist</color>", RoleTypeId.NtfSpecialist },
        { "<color=#003DCA>Ntf Captain</color>", RoleTypeId.NtfCaptain },
        { "<color=#559101>Chaos Conscript</color>", RoleTypeId.ChaosConscript },
        { "<color=#008F1C>Chaos Rifleman</color>", RoleTypeId.ChaosRifleman },
        { "<color=#066328>Chaos Marauder</color>", RoleTypeId.ChaosMarauder },
        { "<color=#15853D>Chaos Repressor</color>", RoleTypeId.ChaosRepressor }
    };

    [Description("Available SpawnLocations and their name")]
    public Dictionary<string, SpawnLocationType> SpawnLocation { get; set; } =
        new()
        {
            { "Inside 330", SpawnLocationType.Inside330 },
            { "Inside 330 Chamber", SpawnLocationType.Inside330Chamber },
            { "Inside 049 Armory", SpawnLocationType.Inside049Armory },
            { "Inside 079 Secondary", SpawnLocationType.Inside079Secondary },
            { "SCP-096 Room", SpawnLocationType.Inside096 },
            { "SCP-173 Armory near Spawn", SpawnLocationType.Inside173Armory },
            { "SCP-173 Bottom of Stairs", SpawnLocationType.Inside173Bottom },
            { "SCP-173 Top of Stairs (Connector)", SpawnLocationType.Inside173Connector },
            { "Exit — First Door", SpawnLocationType.InsideEscapePrimary },
            { "Exit — Second Door", SpawnLocationType.InsideEscapeSecondary },
            { "Intercom", SpawnLocationType.InsideIntercom },
            { "LCZ Armory", SpawnLocationType.InsideLczArmory },
            { "LCZ Café PC-15", SpawnLocationType.InsideLczCafe },
            { "Surface — Nuclear Room", SpawnLocationType.InsideSurfaceNuke },
            { "SCP-079 First Door", SpawnLocationType.Inside079First },
            { "SCP-173 Gate", SpawnLocationType.Inside173Gate },
            { "SCP-914 Inside", SpawnLocationType.Inside914 },
            { "Gate A", SpawnLocationType.InsideGateA },
            { "Gate B", SpawnLocationType.InsideGateB },
            { "GR-18", SpawnLocationType.InsideGr18 },
            { "HCZ Armory (3-way)", SpawnLocationType.InsideHczArmory },
            { "Micro HID Chamber", SpawnLocationType.InsideHidChamber },
            { "LCZ Toilets", SpawnLocationType.InsideLczWc },
            { "GR-18 Glass Box", SpawnLocationType.InsideGr18Glass },
            { "SCP-106 First Door", SpawnLocationType.Inside106Primary },
            { "SCP-106 Second Door", SpawnLocationType.Inside106Secondary },
            { "SCP-939 Cryochamber", SpawnLocationType.Inside939Cryo },
            { "SCP-079 Armory", SpawnLocationType.Inside079Armory },
            { "SCP-127 Laboratory", SpawnLocationType.Inside127Lab },
            { "Micro HID Laboratory", SpawnLocationType.InsideHidLab }
        };

    [Description("Available Items and their name")]
    public Dictionary<string, ItemType> Items { get; set; } = new()
    {
        { "None", ItemType.None },
        { "Janitor Keycard", ItemType.KeycardJanitor },
        { "Scientist Keycard", ItemType.KeycardScientist },
        { "Research Coordinator Keycard", ItemType.KeycardResearchCoordinator },
        { "Zone Manager Keycard", ItemType.KeycardZoneManager },
        { "Guard Keycard", ItemType.KeycardGuard },
        { "MTF Private Keycard", ItemType.KeycardMTFPrivate },
        { "Containment Engineer Keycard", ItemType.KeycardContainmentEngineer },
        { "MTF Operative Keycard", ItemType.KeycardMTFOperative },
        { "MTF Captain Keycard", ItemType.KeycardMTFCaptain },
        { "Facility Manager Keycard", ItemType.KeycardFacilityManager },
        { "Chaos Insurgency Keycard", ItemType.KeycardChaosInsurgency },
        { "O5 Keycard", ItemType.KeycardO5 },
        { "Radio", ItemType.Radio },
        { "COM-15", ItemType.GunCOM15 },
        { "Medkit", ItemType.Medkit },
        { "Flashlight", ItemType.Flashlight },
        { "MicroHID", ItemType.MicroHID },
        { "SCP-500", ItemType.SCP500 },
        { "SCP-207", ItemType.SCP207 },
        { "E-11 SR", ItemType.GunE11SR },
        { "Crossvec", ItemType.GunCrossvec },
        { "FSP-9", ItemType.GunFSP9 },
        { "Logicer", ItemType.GunLogicer },
        { "HE Grenade", ItemType.GrenadeHE },
        { "Flash Grenade", ItemType.GrenadeFlash },
        { "COM-18", ItemType.GunCOM18 },
        { "SCP-018", ItemType.SCP018 },
        { "SCP-268", ItemType.SCP268 },
        { "Adrenaline", ItemType.Adrenaline },
        { "Painkillers", ItemType.Painkillers },
        { "Coin", ItemType.Coin },
        { "Light Armor", ItemType.ArmorLight },
        { "Combat Armor", ItemType.ArmorCombat },
        { "Heavy Armor", ItemType.ArmorHeavy },
        { "Revolver", ItemType.GunRevolver },
        { "A7", ItemType.GunA7 },
        { "Shotgun", ItemType.GunShotgun },
        { "SCP-330", ItemType.SCP330 },
        { "SCP-2176", ItemType.SCP2176 },
        { "SCP-244-A", ItemType.SCP244a },
        { "SCP-244-B", ItemType.SCP244b },
        { "SCP-1853", ItemType.SCP1853 },
        { "Particle Disruptor", ItemType.ParticleDisruptor },
        { "COM-45", ItemType.GunCom45 },
        { "SCP-1576", ItemType.SCP1576 },
        { "Jailbird", ItemType.Jailbird },
        { "Anti-SCP-207", ItemType.AntiSCP207 },
        { "FRMG-0", ItemType.GunFRMG0 },
        { "Lantern", ItemType.Lantern },
        { "SCP-1344", ItemType.SCP1344 },
        { "Surface Access Pass", ItemType.SurfaceAccessPass },
        { "SCP-127", ItemType.GunSCP127 },
        { "SCP-1509", ItemType.SCP1509 }
    };


    [Description("Available KeycardTypes and their name DO NOT CHANGE THE ORDER")]
    public string[] KeycardTypesMap { get; set; } = ["Site 02", "Task Force", "Management", "Metal Case"];


    public string CharacterCreatorPermission { get; set; } = "cc.create";
    public string CustomCardItemName { get; set; } = "<b>Custom Keycard</b>";

    [Description("What Custom Items should not be able to be given with the menu")]
    public List<uint> ExcludedExiledCustomItemsID { get; set; } = [];

    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;
}
