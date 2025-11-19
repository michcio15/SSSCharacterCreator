using System;
using Exiled.API.Features;
using SSSCharacterCreator.ServerSpecific;
using Player = Exiled.Events.Handlers.Player;

namespace SSSCharacterCreator;
#pragma warning disable 1591
// ReSharper disable once ClassNeverInstantiated.Global
public class SssCharacterCreator : Plugin<Config, Translation>
{
    public static SssCharacterCreator? Instance;
    public override string Name { get; } = "SSSCharacterCreator";
    public override string Author { get; } = "michcio";
    public override string Prefix { get; } = "SSSCharacterCreator";
    public static new Translation Translation { get; private set; } = null!;
    public static new Config Config { get; private set; } = null!;
    public override Version Version { get; } = new(1, 1, 1);

    public override void OnEnabled()
    {
        Instance = this;
        Config = base.Config;
        Translation = base.Translation;
        Player.Died += CharacterCreator.OnDied;
        Player.Verified += CharacterCreator.OnVerified;
        Player.Left += CharacterCreator.OnLeft;
    }

    public override void OnDisabled()
    {
        base.OnDisabled();
        Instance = null;
        Player.Died -= CharacterCreator.OnDied;
        Player.Verified -= CharacterCreator.OnVerified;
        Player.Left -= CharacterCreator.OnLeft;
    }
}
