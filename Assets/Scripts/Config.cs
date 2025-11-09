using System.Collections.Generic;
using Game.Augmentation;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics;

public static class Config
{
    public static readonly List<PieceConfig> PieceConfigWhite = new()
    {
        new PieceConfig(PieceType.Swordfish, false, 80),
        new PieceConfig(PieceType.MoorishIdols, false, 94),
    };

    public static readonly List<PieceConfig> PieceConfigBlack = new()
    {
        new PieceConfig(PieceType.FeatherStar, true, 83),
        new PieceConfig(PieceType.ElectricEel, true, 82),
        new PieceConfig(PieceType.SloaneSViperFish, true, 81),
        new PieceConfig(PieceType.Lionfish, true, 79),
    };

    public static readonly RelicConfig relicWhiteConfig = new RelicConfig(RelicType.SirensHarpoon, false, 5);
    public static readonly RelicConfig relicBlackConfig = new RelicConfig(RelicType.SirensHarpoon, true, 5);
    public static readonly RegionalEffectType regionalEffectType = RegionalEffectType.Whirpool;
}