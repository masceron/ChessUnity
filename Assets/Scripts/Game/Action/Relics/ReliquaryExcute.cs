using Game.Common;
using Game.Managers;
using Game.Tile;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ReliquaryExcute : Action, IRelicAction
    {
        public ReliquaryExcute() : base(-1)
        {
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
