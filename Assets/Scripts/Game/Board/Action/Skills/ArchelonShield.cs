using Game.Board.Action.Internal;
using Game.Board.Effects.Buffs;
using Game.Board.General;

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
            var gameState = MatchManager.Ins.GameState;
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(gameState.PieceBoard[To])));
            gameState.PieceBoard[Caller].SkillCooldown = 2;
        }
    }
}