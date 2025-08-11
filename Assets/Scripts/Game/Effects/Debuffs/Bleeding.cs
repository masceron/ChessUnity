using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Bleeding: Effect, IEndTurnEffect
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public byte TurnLeftToDie = 3;
        // ReSharper disable once MemberCanBePrivate.Global
        public byte TurnSinceLastMove;
        
        public Bleeding(PieceLogic piece) : base(-1, 1, piece, EffectName.Bleeding)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction.Maker != Piece.Pos)
            {
                TurnSinceLastMove++;
                if (TurnSinceLastMove < 6) return;
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
            else
            {
                TurnSinceLastMove = 0;
                TurnLeftToDie--;
                if (TurnLeftToDie == 0) ActionManager.EnqueueAction(new KillPiece(lastMainAction.Maker));
            }

        }
    }
}