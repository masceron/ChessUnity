using Game.Common;
using Game.Managers;
using UX.UI.Ingame;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PeaceTreatyExcute : Action, IRelicAction
    {
        private bool Color;
        private const int TurnToEnd = 50;
        
        public PeaceTreatyExcute(bool color) : base(-1)
        {
            Color = color;
        }

        protected override void ModifyGameState()
        {
            if (MatchManager.Ins.GameState.CurrentTurn >= TurnToEnd)
            {
                BoardUtils.NotifyGameEnd(EndGameUI.MessageID.Win);
            }
            
            var relic = BoardUtils.GetRelicOf(Color);
            if (relic != null)
            {
                if (Color)
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
