using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class  PenpointGunnelPassive: Effect
    {
        public PenpointGunnelPassive(PieceLogic piece) : base(-1, 1, piece, "effect_penpoint_gunnel_passive")
        {}

        public override void OnCallPieceAction(Action.Action action)    
        {
            if (action == null) return;
            if (action != ICaptures) return;
            if (action.Target == Piece.Pos && action.Succeed)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Leashed(BoardUtils.PieceOn(action.Maker), Piece.Pos, -1)));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI();
        }
    }
}