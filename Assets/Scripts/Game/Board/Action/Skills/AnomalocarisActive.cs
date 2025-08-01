using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;

namespace Game.Board.Action.Skills
{
    public class AnomalocarisActive: Action, ISkills
    {
        public AnomalocarisActive(int caller, int to) : base(caller, false)
        {
            From = (ushort)caller;
            To = (ushort)to;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, MatchManager.Ins.GameState.PieceBoard[To])));
            MatchManager.Ins.GameState.PieceBoard[Caller].SkillCooldown = 1;
        }
    }
}