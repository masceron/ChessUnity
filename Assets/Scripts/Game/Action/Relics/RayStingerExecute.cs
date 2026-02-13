using MemoryPack;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class RayStingerExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private RayStingerExecute() { }

        private const int BleedingStack = 3;
        private const int BrokenDuration = 2;

        public RayStingerExecute(int target) : base(-1)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(BleedingStack, BoardUtils.PieceOn(Target))));
            ActionManager.EnqueueAction(new ApplyEffect(new Broken(BrokenDuration, BoardUtils.PieceOn(Target))));
        }
    }
}
