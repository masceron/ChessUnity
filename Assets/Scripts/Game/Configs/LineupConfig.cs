using System;
using System.Collections.Generic;
using Game.Data.Pieces;

namespace Game.Configs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public struct LineupConfig
    {
        public readonly List<PieceConfig> WhiteConfig;
        public readonly List<PieceConfig> BlackConfig;

        public LineupConfig(List<PieceConfig> w, List<PieceConfig> b)
        {
            WhiteConfig = w;
            BlackConfig = b;
        }
    }
}