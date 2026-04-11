using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    public class CooldownSkill: Action, IInternal
    {
        private int newSkillCooldown = -1;
        public CooldownSkill(PieceLogic pieceLogic, int newSkillCooldown = -1): base(null, pieceLogic)
        {
        }
        protected override void ModifyGameState()
        {
            if (newSkillCooldown != -1)
            {
                GetTargetAsPiece().SkillCooldown = newSkillCooldown;
            }
            else
            {
                GetTargetAsPiece().SkillCooldown = ((IPieceWithSkill)GetTargetAsPiece()).TimeToCooldown;
            }
            
        }
    }
}