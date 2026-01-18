using Game.Common;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;
using Game.Tile;


namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ReliquaryPending : Action, System.IDisposable, IRelicAction
    {
        private Reliquary reliquary;
        
        public ReliquaryPending(Reliquary cp, int maker, bool pos = false) : base(maker)
        {
            reliquary = cp;
            Maker = (ushort)maker;
        }

        public void Dispose()
        {
            reliquary = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {

            var FOWPos = getRandomPos();
            var FogOfWar = new FogOfWar(true);
            FogOfWar.SetPositon(FOWPos);
            FormationManager.Ins.SetFormation(FOWPos, FogOfWar);
            var SaposPos = getRandomPos();
            var Saprolegnia = new Saprolegnia(false, true);
            Saprolegnia.SetPositon(SaposPos);
            FormationManager.Ins.SetFormation(SaposPos, Saprolegnia);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            reliquary.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
        private int getRandomPos()
        {
            var pos = new System.Random().Next(1, MatchManager.Ins.StartingSize.x * MatchManager.Ins.StartingSize.y);
            while (!TileManager.Ins.IsTileEmpty(pos) && FormationManager.Ins.GetFormation(pos) != null)
            {
                pos = new System.Random().Next(1, MatchManager.Ins.StartingSize.x * MatchManager.Ins.StartingSize.y);
            }
            var mappedPos = BoardUtils.PosMap(pos, MatchManager.Ins.StartingSize);
            return mappedPos;
        }
    }
}