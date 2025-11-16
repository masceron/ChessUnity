using Game.Piece.PieceLogic;

namespace Game.Effects.Augmentation
{
    public class ProtectiveLensPassive : Effect
    {

        public ProtectiveLensPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, EffectName.ProtectiveLensPassive)
        { }
    }
}