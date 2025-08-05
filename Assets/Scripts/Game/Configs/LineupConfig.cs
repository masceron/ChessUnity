using System;
using System.Collections.Generic;
using Game.Data.Pieces;

namespace Game.Configs
{
    [Serializable]
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