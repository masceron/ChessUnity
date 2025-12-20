using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    public class Shortreach : Effect
    {
        public Shortreach(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_shortreach")
        {
            if (Strength > 0)
            {
                Piece.AttackRange -= (byte)Strength;
                if (Piece.AttackRange < 1)
                {
                    Piece.AttackRange = 1;
                }
            }
        }

        public override void OnRemove()
        {
            if (Strength > 0)
            {
                Piece.AttackRange += (byte)Strength;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - (Strength * 15 + Duration * 5);
        }
    }
}