using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisKill: Action, ISkills
    {
        public VelkarisKill(int p, ushort f, ushort t) : base(p, false)
        {
            From = f;
            To = t;
        }

        protected override void Animate()
        {
            pieceManager.Destroy(To);
        }

        protected override void ModifyGameState()
        {
            gameState.Destroy(To);
            gameState.PieceBoard[From].SkillCooldown = -1;
        }
    }
}