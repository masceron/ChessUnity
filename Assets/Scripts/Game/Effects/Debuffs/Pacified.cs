using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.Effects.Debuffs
{
    public class Pacified : Effect
    {

        private byte lastAttackRange;
        public Pacified(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_pacified")
        {
            lastAttackRange = Piece.AttackRange;
        }
        
        public override void OnCallPieceAction(Action.Action action)
        {
            Piece.AttackRange = 0;
            Debug.Log(Piece.Type);
        }

        public override void OnRemove()
        {
            Piece.AttackRange = lastAttackRange;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}