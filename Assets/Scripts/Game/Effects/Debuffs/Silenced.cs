using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
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