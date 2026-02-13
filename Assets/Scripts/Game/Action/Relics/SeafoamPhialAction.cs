using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Buffs;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class SeafoamPhialAction : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private SeafoamPhialAction() { }

        public SeafoamPhialAction(int maker) : base(maker)
        {
            Maker = maker;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new Purify(Maker, Maker));
            ActionManager.EnqueueAction(new ApplyEffect(new Haste(3, 1, PieceOn(Maker))));
        }
    }
}