using Game.Piece.PieceLogic;
using Game.Action;
using Game.Action.Internal;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PureMinded: Effect, IEndTurnEffect
    {
        sbyte RoundsPerSelfPurify = 5;
        sbyte count;
        public PureMinded(PieceLogic piece) : base(-1, 1, piece, EffectName.PureMinded)
        {   
            count = RoundsPerSelfPurify;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (count > 0) count--;
            if (count == 0) {
                ActionManager.EnqueueAction(new Purify(Piece.Pos, Piece.Pos));
                count = RoundsPerSelfPurify;
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; }

    }
}