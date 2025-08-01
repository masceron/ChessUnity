using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Bleeding: Effect, IEndTurnEffect
    {
        public byte TurnLeftToDie;
        public byte TurnSinceLastMove;
        
        public Bleeding(PieceLogic piece) : base(-1, 1, piece, EffectName.Bleeding)
        {
            EndTurnEffectType = EndTurnEffectType.AtAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction.Caller != Piece.Pos)
            {
                TurnSinceLastMove++;
                if (TurnSinceLastMove < 5) return;
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
            else
            {
                TurnLeftToDie--;
                if (TurnLeftToDie == 0) ActionManager.EnqueueAction(new DestroyPiece(lastMainAction.Caller));
            }

        }
    }
}