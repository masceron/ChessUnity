using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    public class PrecisionMonoclePending : PendingAction, System.IDisposable
    {
        private PrecisionMonocle _precisionMonocle;
        public PrecisionMonoclePending(PrecisionMonocle pm, int maker) : base(maker)
        {
            _precisionMonocle = pm;
            Maker = maker;
        }

        protected override void CompleteAction()
        {
            CommitResult(new PrecisionMonocleAction(Maker));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            _precisionMonocle.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
            Dispose();
        }

        public void Dispose()
        {
            _precisionMonocle = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}