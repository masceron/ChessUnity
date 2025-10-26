using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Silenced : Effect
    {
        public Silenced(PieceLogic piece) : base(-1, 1, piece, EffectName.Silenced)
        {
            
        }

        public override void OnApply()
        {
            Piece.CanUseSkill = false;
        }
        
        public override void OnRemove()
        {
            Piece.CanUseSkill = true;
        }
    }
}