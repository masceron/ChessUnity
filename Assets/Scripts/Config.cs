using System.Collections.Generic;
using Game.Augmentation;
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
        new PieceConfig(PieceType.ElectricEel, true, 82)
    };

    public static readonly RelicConfig relicWhiteConfig = new RelicConfig(RelicType.SirensHarpoon, false, 5);
    public static readonly RelicConfig relicBlackConfig = new RelicConfig(RelicType.SirensHarpoon, true, 5);
}