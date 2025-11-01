using System.Collections.Generic;
using Game.Piece;
using System;
using Game.Effects.RegionalEffect;

public class FreePlayConfig
{
    public Action<PieceConfig> OnWhitePieceAdded;
    public List<PieceConfig> PieceConfigWhite = new()
    {

    };

    public List<PieceConfig> PieceConfigBlack = new()
    {

    };
    public List<RegionalEffect> regionalEffects = new();


}