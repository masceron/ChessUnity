using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Bleeding: Effect, IEndTurnEffect
    {
        private byte turnLeftToDie = 3;
        private byte turnSinceLastMove;
        
        public Bleeding(PieceLogic piece) : base(-1, 1, piece, EffectName.Bleeding)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction.Caller != Piece.Pos)
            {
                turnSinceLastMove++;
                if (turnSinceLastMove < 6) return;
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
            else
            {
                turnSinceLastMove = 0;
                turnLeftToDie--;
                if (turnLeftToDie == 0) ActionManager.EnqueueAction(new DestroyPiece(lastMainAction.Caller));
            }

        }

        public override string Description()
        {
            return string.Format(AssetManager.Ins.EffectData[EffectName].description, turnLeftToDie, 6 - turnSinceLastMove); 
        }
    }
}