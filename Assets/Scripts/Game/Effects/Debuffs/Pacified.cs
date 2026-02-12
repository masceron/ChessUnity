using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    public class Pacified : Effect
    {
        public Pacified(int duration, PieceLogic piece) : base(duration, 1, piece, "effect_pacified")
        {}

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}