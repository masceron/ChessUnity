using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LedgerStonePending : PendingAction, IDisposable, IRelicAction, IInternal
    {
        private readonly bool _isFirstOption;
        private LedgerStone _ledgerStone;

        public LedgerStonePending(LedgerStone cp, bool isFirstOption) : base(-1)
        {
            _ledgerStone = cp;
            _isFirstOption = isFirstOption;
        }

        public void Dispose()
        {
            _ledgerStone = null;
        }

        protected override void CompleteAction()
        {
            var execute = new LedgerStoneExecute(_isFirstOption);
            CommitResult(execute);
            _ledgerStone?.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
    }
}