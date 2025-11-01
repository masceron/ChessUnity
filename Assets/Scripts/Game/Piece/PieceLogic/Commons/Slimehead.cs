using System;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Others;
using Game.Managers;
using Game.Movesets;
using Game.Tile;

namespace Game.Piece.PieceLogic.Commons
{
    public class Slimehead : PieceLogic
    {
        public Slimehead(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, FrontDefenderMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SlimeheadPassive(this)));
        }
    }
}