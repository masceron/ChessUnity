using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OrnetesEdictPending : PendingAction, System.IDisposable, IRelicAction
    {
        private OrnetesEdict _ornetesEdict;
        
        public OrnetesEdictPending(OrnetesEdict cp, int maker) : base(maker)
        {
            _ornetesEdict = cp;
            Target = maker;
            Maker = maker;
        }

        protected override void CompleteAction()
        {
            var execute = new OrnetesEdictExecute(Target);
            CommitResult(execute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            _ornetesEdict?.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            _ornetesEdict = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}