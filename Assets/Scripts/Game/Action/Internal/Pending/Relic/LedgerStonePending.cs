using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LedgerStonePending : PendingAction, System.IDisposable, IRelicAction, IInternal
    {
        private LedgerStone ledgerStone;
        private bool isFirstOption;
        
        public LedgerStonePending(LedgerStone cp, bool isFirstOption) : base(-1)
        {
            ledgerStone = cp;
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