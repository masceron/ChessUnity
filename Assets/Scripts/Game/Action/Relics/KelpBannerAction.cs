using Game.Managers;
using Game.Tile;
using static Game.Common.BoardUtils;    

namespace Game.Action.Relics
{
    public class KelpBannerAction : Action, IRelicAction
    {
        public KelpBannerAction(int maker) : base(maker)
        {
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
        }
    }
}