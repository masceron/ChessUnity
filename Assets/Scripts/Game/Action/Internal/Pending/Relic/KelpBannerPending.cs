using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using Game.Tile;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class KelpBannerPending : Action, System.IDisposable, IRelicAction
    {
        private KelpBanner kelpBanner;
        public KelpBannerPending(KelpBanner kp, int maker, bool pos = false) : base(maker)
        {
            kelpBanner = kp;
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var formation = FormationManager.Ins.GetFormation(Maker);
 
            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;
                
                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    var kelpPos = IndexOf(rankOff, fileOff);
                    FormationManager.Ins.SetFormation(kelpPos, new Kelp(false, formation.GetColor()));
                }
            }
            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            kelpBanner.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            kelpBanner = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}