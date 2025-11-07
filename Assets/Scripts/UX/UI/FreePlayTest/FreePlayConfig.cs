using System.Collections.Generic;
using Game.Piece;
using System;
using Game.Effects.RegionalEffect;
using Game.Relics;
public class FreePlayConfig
{
    // public Action<PieceConfig> OnWhitePieceAdded;
    public List<PieceConfig> PieceConfigWhite = new()
    {

    };

    public List<PieceConfig> PieceConfigBlack = new()
    {

    };
    public RegionalEffectType regionalEffectType;
    public RelicConfig relicWhiteConfig = new RelicConfig(RelicType.SirensHarpoon, false, 5);
    public RelicConfig relicBlackConfig = new RelicConfig(RelicType.SirensHarpoon, true, 5);

}