using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    public class CorruptedWisperExecute : Action, IRelicAction
    {
        public CorruptedWisperExecute(int target) : base(-1)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Controlled(-1, PieceOn(Target))));
        }
    }
}
