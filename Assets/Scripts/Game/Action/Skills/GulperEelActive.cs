using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;


namespace Game.Action.Skills
{
    public class GulperEelActive : Action, ISkills
    {
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
    }
}

