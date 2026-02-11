using Game.Managers;
using Game.Relics;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LedgerStonePending : PendingAction, System.IDisposable, IRelicAction, IInternal
    {
        private LedgerStone _ledgerStone;
        private readonly bool _isFirstOption;
        
        public LedgerStonePending(LedgerStone cp, bool isFirstOption) : base(-1)
        {
            _ledgerStone = cp;
            _isFirstOption = isFirstOption;
        }

        protected override void CompleteAction()
        {
            var execute = new LedgerStoneExcute(_isFirstOption);
            CommitResult(execute);
            _ledgerStone?.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            _ledgerStone = null;
        }
    }
}