using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OrnetesEdictPending : PendingAction, System.IDisposable, IRelicAction, IInternal
    {
        private OrnetesEdict ornetesEdict;
        
        public OrnetesEdictPending(OrnetesEdict cp, int maker, bool pos = false) : base(maker)
        {
            ornetesEdict = cp;
            Target = (ushort)maker;
            Maker = (ushort)maker;
        }

        public override void CompleteAction()
        {
            var excute = new OrnetesEdictExecute(Target);
            BoardViewer.Ins.ExecuteAction(excute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            if(ornetesEdict != null)
            {
                ornetesEdict.SetCooldown();
            }
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            ornetesEdict = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}