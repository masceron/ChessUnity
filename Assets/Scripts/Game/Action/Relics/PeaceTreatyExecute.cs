using Game.Common;
using MemoryPack;
using UX.UI.Ingame;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class PeaceTreatyExecute : Action, IRelicAction
    {
        private const int TurnToEnd = 50;

        [MemoryPackInclude] private bool _color;

        [MemoryPackConstructor]
        private PeaceTreatyExecute()
        {
        }

        public PeaceTreatyExecute(bool color) : base(null)
        {
            _color = color;
        }

        protected override void ModifyGameState()
        {
            if (BoardUtils.GetCurrentTurn() >= TurnToEnd) BoardUtils.NotifyGameEnd(EndGameUI.MessageID.Win);
        }
    }
}