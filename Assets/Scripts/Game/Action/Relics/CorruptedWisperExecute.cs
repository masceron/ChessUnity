using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Others;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    public class CorruptedWisperExecute : Action, IRelicAction
    {
        private int duration;
        public CorruptedWisperExecute(int duration, int target) : base(-1)
        {
            Target = target;
            this.duration = duration;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Controlled(126, PieceOn(Target))));
        }
    }
}
