using Game.Board.General;
using Game.Board.PieceLogic.Commanders;

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

        public override void ApplyAction(GameState state)
        {
            MatchManager.PieceManager.Destroy(To);
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.Destroy(To);
            
            ((Velkaris)state.MainBoard[From]).SkillCooldown = -1;
        }
    }
}