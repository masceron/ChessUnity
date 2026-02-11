using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    public class EpauletteSharkActive : Action, ISkills
    {
        public EpauletteSharkActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
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