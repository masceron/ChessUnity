using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.SpecialAbility;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class Remora: Commons.PieceLogic
    {
        public Remora(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new RemoraPassive(this)));
        }
    }
}