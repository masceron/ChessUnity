using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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
            if (lastMainAction.Maker != Piece.Pos)
            {
                turnSinceLastMove++;
                if (turnSinceLastMove < 6) return;
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
            else
            {
                turnSinceLastMove = 0;
                turnLeftToDie--;
                if (turnLeftToDie == 0) ActionManager.EnqueueAction(new KillPiece(lastMainAction.Maker));
            }

        }

        public override string Description()
        {
            return string.Format(AssetManager.Ins.EffectData[EffectName].description, turnLeftToDie, 6 - turnSinceLastMove); 
        }
    }
}