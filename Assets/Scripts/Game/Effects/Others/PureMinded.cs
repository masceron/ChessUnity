using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PureMinded: Effect, IEndTurnEffect
    {
        private const sbyte RoundsPerSelfPurify = 5;
        sbyte count;
        public PureMinded(PieceLogic piece) : base(-1, 1, piece, "effect_pure_minded")
        {   
            count = RoundsPerSelfPurify;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (count > 0) count--;
            if (count == 0) {
                ActionManager.EnqueueAction(new Purify(Piece.Pos, Piece.Pos));
                count = RoundsPerSelfPurify;
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40/5;
        }
    
    }
}