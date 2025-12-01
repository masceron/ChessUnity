using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.AI;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RottingScythePending : Action, IPendingAble, System.IDisposable, IRelicAction
    {
        private RottingScythe rottingScythe;

        public RottingScythePending(RottingScythe rottingScythe, int maker, bool pos) : base(maker, pos)
        {
            this.rottingScythe = rottingScythe;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            ActionManager.ExecuteImmediately(new KillPiece(Maker));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            rottingScythe.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }
        
        public void Dispose()
        {
            rottingScythe = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
        }

    }
}