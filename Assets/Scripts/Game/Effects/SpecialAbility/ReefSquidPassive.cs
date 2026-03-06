using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Traits;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class ReefSquidPassive : Effect, IAfterPieceActionTrigger
    {
        public AfterActionPriority Priority => AfterActionPriority.Buff;
        public ReefSquidPassive(PieceLogic pieceLogic) : base(-1, 1, pieceLogic, "effect_reef_squid_passive")
        {
            
        }
        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is ICaptures && action.Maker == Piece.Pos && action.Result == ResultFlag.Evade)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Piercing(-1, Piece)));
                ActionManager.EnqueueAction(new ApplyEffect(new Traits.SnappingStrike(Piece)));
            }
        }
    }

}
