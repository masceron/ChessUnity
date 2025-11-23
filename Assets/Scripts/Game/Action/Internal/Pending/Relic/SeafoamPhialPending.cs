using Game.Effects.Buffs;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeafoamPhialPending : Action, IPendingAble, System.IDisposable
    {
        private SeafoamPhial seafoamPhial;

        public SeafoamPhialPending(SeafoamPhial seafoamPhial, int maker, bool pos) : base(maker, pos)
        {
            this.seafoamPhial = seafoamPhial;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            ActionManager.ExecuteImmediately(new Purify(Maker, Maker));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Haste(3, 1, PieceOn(Maker))));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            seafoamPhial.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }
        
        public void Dispose()
        {
            seafoamPhial = null;
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