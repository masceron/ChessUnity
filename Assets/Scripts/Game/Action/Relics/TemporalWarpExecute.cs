using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Others;
using MemoryPack;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class TemporalWarpExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private TemporalWarpExecute()
        {
        }

        public TemporalWarpExecute(int maker, int target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(GetFrom(), GetTargetPos()));
            ActionManager.EnqueueAction(new ApplyEffect(new TemporalWarpReturn(GetMaker() as PieceLogic, GetTargetPos())));
        }
    }
}