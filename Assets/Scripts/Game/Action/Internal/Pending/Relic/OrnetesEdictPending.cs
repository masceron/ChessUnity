using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OrnetesEdictPending : PendingAction, IDisposable, IRelicAction
    {
        private OrnetesEdict _ornetesEdict;

        public OrnetesEdictPending(OrnetesEdict cp, int maker) : base(null, maker)
        {
            _ornetesEdict = cp;
        }

        public void Dispose()
        {
            _ornetesEdict = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            var execute = new OrnetesEdictExecute(GetTargetPos());
            CommitResult(execute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            _ornetesEdict?.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
    }
}