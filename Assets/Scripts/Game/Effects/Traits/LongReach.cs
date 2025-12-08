using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits{
    public class LongReach : Effect
    {

        public LongReach(PieceLogic piece, sbyte duration = 1) : base(duration, 1, piece, "effect_long_reach")
        {
            piece.AttackRange += 2;

        }

        public override void OnRemove()
        {
            Piece.AttackRange -= 2;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + Strength * 15 + Duration * 5;
        }
    }
}

