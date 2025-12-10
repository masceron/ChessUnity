using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    public class Shortreach : Effect
    {
        public Shortreach(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_shortreach")
        {
        }
        
        public override int GetValueForAI()
        {
            return base.GetValueForAI() - (Strength * 15 + Duration * 5);
        }
    }
}