using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    public class CooldownSkill: Action, IInternal
    {
        public CooldownSkill(PieceLogic pieceLogic): base(null, pieceLogic)
        {
        }
        protected override void ModifyGameState()
        {
            GetTargetAsPiece().SkillCooldown = ((IPieceWithSkill)GetTargetAsPiece()).TimeToCooldown;
        }
    }
}