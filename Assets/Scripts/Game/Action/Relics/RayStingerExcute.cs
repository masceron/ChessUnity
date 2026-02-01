using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;

namespace Game.Action.Relics
{
    public class RayStingerExcute : Action, IRelicAction
    {
        private int bleedingStack = 3;
        private int brokenDuration = 2;

        public RayStingerExcute(int target) : base(-1)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(bleedingStack, BoardUtils.PieceOn(Target))));
            ActionManager.EnqueueAction(new ApplyEffect(new Broken((sbyte)brokenDuration, BoardUtils.PieceOn(Target))));
        }
    }
}
