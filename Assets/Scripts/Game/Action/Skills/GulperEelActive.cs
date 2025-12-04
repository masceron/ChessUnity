using Game.AI;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;


namespace Game.Action.Skills
{
    public class GulperEelActive : Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        public GulperEelActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            TileManager.Ins.DestroyTile(Target);
            FormationManager.Ins.RemoveFormation(Target);

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    }
}
