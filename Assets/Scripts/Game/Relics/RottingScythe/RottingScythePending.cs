using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Managers;
using UX.UI.Ingame;
namespace Game.Relics.RottingScythe
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RottingScythePending : Action.Action, IPendingAble, System.IDisposable
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

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}