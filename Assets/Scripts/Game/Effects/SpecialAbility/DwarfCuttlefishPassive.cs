using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.SpecialAbility
{
    public class DwarfCuttlefishPassive : Effect
    {
        private const int EvasionChanceBuff = 10;
        
        public DwarfCuttlefishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_dwarf_cuttlefish_passive")
        {
            
        }
    }
}