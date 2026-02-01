using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class CursedPlatePassive : Effect
    {
        public CursedPlatePassive(PieceLogic piece) : base(-1, 1, piece, "effect_cursed_plate_passive")
        {
        }
    }
}