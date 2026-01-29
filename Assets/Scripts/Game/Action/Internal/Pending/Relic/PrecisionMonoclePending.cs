using System.Linq;
using Game.Action.Relics;
using Game.Common;
using Game.Effects.Traits;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    public class PrecisionMonoclePending : PendingAction, System.IDisposable
    {
        private PrecisionMonocle precisionMonocle;
        public PrecisionMonoclePending(PrecisionMonocle pm, int maker, bool pos = false) : base(maker)
        {
            precisionMonocle = pm;
            Maker = (ushort)maker;
        }

        public override void CompleteAction()
        {
            BoardViewer.Ins.ExecuteAction(new PrecisionMonocleAction(Maker));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            precisionMonocle.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
            Dispose();
        }

        public void Dispose()
        {
            precisionMonocle = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}