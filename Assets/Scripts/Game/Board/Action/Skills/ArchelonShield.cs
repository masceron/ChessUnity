using Game.Board.Action.Internal;
using Game.Board.Effects.Buffs;
using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Skills
{
    public class ArchelonShield: Action, ISkills
    {
        public ArchelonShield(int caller, int to) : base(caller, false)
        {
            From = (ushort)caller;
            To = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(-1, gameState.PieceBoard[To])));
            gameState.PieceBoard[Caller].SkillCooldown = 4;
        }
    }
}