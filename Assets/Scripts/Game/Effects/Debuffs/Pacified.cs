using Game.Piece;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    public class Pacified : Effect
    {

        private byte lastAttackRange;
        public Pacified(PieceLogic piece) : base(-1, 1, piece, "effect_pacified")
        {
            lastAttackRange = Piece.AttackRange;
        }
        
        public override void OnCallPieceAction(Action.Action action)
        {
            Piece.AttackRange = 0;
        }

        public override void OnRemove()
        {
            Piece.AttackRange = lastAttackRange;
        }
    }
}