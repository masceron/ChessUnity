    using System.Linq;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UnityEngine;
using UX.UI.Ingame;
using Game.Action.Relics;
using Game.Tile;
using Game.Common;


namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PeaceTreatyPending : Action, System.IDisposable, IRelicAction
    {
        private PeaceTreaty peaceTreaty;
        private const int TurnToEnd = 50;
        public PeaceTreatyPending(PeaceTreaty cp, int maker, bool pos = false) : base(maker)
        {
            peaceTreaty = cp;
            Maker = (ushort)maker;
        }

        public void Dispose()
        {
            peaceTreaty = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
            if (MatchManager.Ins.GameState.CurrentTurn >= TurnToEnd)
            {
                BoardUtils.NotifyGameEnd(EndGameUI.MessageID.Win);
            }
            
            if (peaceTreaty.Color)
            {
                MatchManager.Ins.GameState.BlackRelic = null;
            }
            else
            {
                MatchManager.Ins.GameState.WhiteRelic = null;
            }
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            peaceTreaty.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

    }
}   