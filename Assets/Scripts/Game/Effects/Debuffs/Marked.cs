using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    public class Marked : Effect
    {
        public Marked(int duration, PieceLogic piece) : base(duration, 1, piece, "effect_marked")
        {
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 10;
        }
    }
}