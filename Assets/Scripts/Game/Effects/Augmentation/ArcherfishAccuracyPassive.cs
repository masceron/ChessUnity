using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class ArcherfishAccuracyPassive : Effect
    {
        public ArcherfishAccuracyPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_archerfish_accuracy_passive")
        {
            // do nothing
        }
    }
}