using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class BlueChromis : Commons.PieceLogic
    {
        public BlueChromis(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new BlueChromisPassive(this)));
        }
    }
}