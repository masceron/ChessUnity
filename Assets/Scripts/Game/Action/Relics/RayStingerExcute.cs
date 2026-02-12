using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;

namespace Game.Action.Relics
{
    public class RayStingerExcute : Action, IRelicAction
    {
        private readonly int bleedingStack = 3;
        private readonly int brokenDuration = 2;

        public RayStingerExcute(int target) : base(-1)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(bleedingStack, BoardUtils.PieceOn(Target))));
            ActionManager.EnqueueAction(new ApplyEffect(new Broken(brokenDuration, BoardUtils.PieceOn(Target))));
        }
    }
}
