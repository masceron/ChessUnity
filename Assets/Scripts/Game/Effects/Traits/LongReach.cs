using Game.Piece.PieceLogic;

namespace Game.Effects.Buffs{
    public class LongReach : Effect
    {

        public LongReach(PieceLogic piece) : base(-1, 1, piece, EffectName.LongReach)
        {
            piece.AttackRange += 2;

        }

        public override void OnRemove()
        {
            Piece.AttackRange -= 2;
        }
    }
}

