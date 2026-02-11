using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    public class MaskedPufferPassive : Effect, IAfterPieceActionEffect
    {
        public MaskedPufferPassive(PieceLogic piece) : base(-1, -1, piece, "effect_masked_puffer_passive")
        { }

        public override int GetValueForAI()
        {
            throw new System.NotImplementedException();
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange + Strength;
        }

        void IAfterPieceActionEffect.OnCallAfterPieceAction(Action.Action action)
        {
            if (action == null || action is not ICaptures) return;

            if (action.Maker == Piece.Pos) return;
            if (action.Target != Piece.Pos) return;

            if (action.Result != ResultFlag.Success)
            {
                ActionManager.EnqueueAction(new Purify(Piece.Pos, Piece.Pos));
            }
        }
    }
}

