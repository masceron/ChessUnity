using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class DwarfCuttlefishActive : Action, ISkills
    {
        public DwarfCuttlefishActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(Target));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}