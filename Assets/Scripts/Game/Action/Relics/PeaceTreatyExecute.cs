using MemoryPack;
using Game.Common;
using Game.Managers;
using UX.UI.Ingame;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class PeaceTreatyExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private PeaceTreatyExecute() { }

        [MemoryPackInclude]
        private bool _color;
        private const int TurnToEnd = 50;
        
        public PeaceTreatyExecute(bool color) : base(-1)
        {
            _color = color;
        }

        protected override void ModifyGameState()
        {
            if (MatchManager.Ins.GameState.CurrentTurn >= TurnToEnd)
            {
                BoardUtils.NotifyGameEnd(EndGameUI.MessageID.Win);
            }
            
            var relic = BoardUtils.GetRelicOf(_color);
            if (relic != null)
            {
                if (_color)
                {
                    MatchManager.Ins.GameState.BlackRelic = null;
                }
                else
                {
                    MatchManager.Ins.GameState.WhiteRelic = null;
                }
            }
        }
    }
}
