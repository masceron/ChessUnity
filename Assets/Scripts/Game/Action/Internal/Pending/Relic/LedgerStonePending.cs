using System.Linq;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UnityEngine;
using UX.UI.Ingame;
using Game.Action.Relics;
using static Game.Common.BoardUtils;
using Game.Piece;
using Game.Action.Internal.Pending;
using Game.Action.Internal;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LedgerStonePending : PendingAction, System.IDisposable, IRelicAction, IInternal
    {
        private LedgerStone ledgerStone;
        private bool isFirstOption;
        
        public LedgerStonePending(LedgerStone cp, bool isFirstOption) : base(-1)
        {
            this.ledgerStone = cp;
            this.isFirstOption = isFirstOption;
        }

        public override void CompleteAction()
        {
           
            
            var excute = new LedgerStoneExcute(isFirstOption);
            BoardViewer.Ins.ExecuteAction(excute);
            if(ledgerStone != null)
            {
                ledgerStone.SetCooldown();
            }
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            ledgerStone = null;
        }
    }
}