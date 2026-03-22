using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.SpecialAbility
{
    public class PlumedSeaFirPassive : Effect
    {
        public PlumedSeaFirPassive(PieceLogic piece) : base(-1, 1, piece, "effect_plumed_sea_fir_passive")
        {
            SetStat(EffectStat.Counter, 0);
        }
    }
}