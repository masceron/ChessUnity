using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class CorruptedWisperExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private CorruptedWisperExecute() { }

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
