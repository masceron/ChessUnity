using System.Linq;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
using Game.AI;
using Game.Effects.Buffs;
using Game.Effects.Traits;

namespace Game.Action.Skills
{
    public class ThreadPipefishActive : Action, ISkills, IAIAction
    {
        public ThreadPipefishActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new ThreadPipefishEffect(PieceOn(Maker), PieceOn(Target))));
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    }
}