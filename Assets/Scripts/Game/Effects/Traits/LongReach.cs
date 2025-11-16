using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits{
    public class LongReach : Effect
    {

        public LongReach(PieceLogic piece) : base(-1, 1, piece, "effect_long_reach")
        {
            piece.AttackRange += 2;

        }

        public override void OnRemove()
        {
            Piece.AttackRange -= 2;
        }
    }
}

