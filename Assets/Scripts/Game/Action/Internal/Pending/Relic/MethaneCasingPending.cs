// Assets/Scripts/Game/Action/Internal/Pending/Relic/SeafoamPhialPending.cs

using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Game.Action.Relics;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Common;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MethaneCasingPending : PendingAction, System.IDisposable
    {
        private MethaneCasing methaneCasing;

        public MethaneCasingPending(MethaneCasing methaneCasing, int maker) : base(maker)
        {
            this.methaneCasing = methaneCasing;
            Maker = (ushort)maker;
        }

        

        public void Dispose()
        {
            methaneCasing = null;
            BoardViewer.SelectingFunction = 0;
        }

        public override void CompleteAction()
        {
            methaneCasing.SetCooldown();

            var excute = new MethaneCasingExcute(Maker);
            BoardViewer.Ins.ExecuteAction(excute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            BoardViewer.Ins.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }
    }
}
