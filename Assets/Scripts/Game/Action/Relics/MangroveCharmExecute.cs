using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Buffs;
using static Game.Common.BoardUtils;
namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class MangroveCharmExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private MangroveCharmExecute() { }

        [MemoryPackInclude]
        private int _firstTarget;
        [MemoryPackInclude]
        private int _secondTarget;

        public MangroveCharmExecute(int firstTarget, int secondTarget) : base(-1)
        {
            _firstTarget = firstTarget;
            _secondTarget = secondTarget;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(_firstTarget))));
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(PieceOn(_secondTarget))));

        }
    }
}
