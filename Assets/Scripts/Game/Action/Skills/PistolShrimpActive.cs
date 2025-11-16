using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    public class PistolShrimpActive : Action, ISkills
    {
        public PistolShrimpActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.ExecuteImmediately(new KillPiece(Target));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}