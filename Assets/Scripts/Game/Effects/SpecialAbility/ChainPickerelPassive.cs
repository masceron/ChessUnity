using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using System;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChainPickerelPassive : Effect, IAfterPieceActionTrigger
    {
        private int Radius;
        public ChainPickerelPassive(PieceLogic piece, int radius) : base(-1, 1, piece, "effect_chain_pickerel_passive")
        {
            Radius = radius;
        }

        private bool CheckPieceInRange(int target)
        {
            var (x1, y1) = RankFileOf(Piece.Pos);
            var (x2, y2) = RankFileOf(target);

            if (Math.Abs(x1 - x2) > Radius || Math.Abs(y1 - y2) > Radius) 
                return false;

            return true;
        }

        public AfterActionPriority Priority => AfterActionPriority.Other;

        public void OnCallAfterPieceAction(Action.Action action) 
        { 
            if (CheckPieceInRange(action.Target))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Leashed(PieceOn(action.Maker), action.Maker, -1)));
            }
        }
    }
}