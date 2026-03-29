using Game.Action.Internal;
using Game.Effects.Buffs;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class SeafoamPhialAction : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private SeafoamPhialAction()
        {
        }

        public SeafoamPhialAction(int maker) : base(maker)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new Purify(GetMakerPos(), GetMakerPos()));
            ActionManager.EnqueueAction(new ApplyEffect(new Haste(3, 1, GetMaker() as PieceLogic)));
        }
    }
}