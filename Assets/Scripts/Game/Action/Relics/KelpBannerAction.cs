using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class KelpBannerAction : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private KelpBannerAction()
        {
        }

        public KelpBannerAction(int maker) : base(maker)
        {
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(GetFrom());
            var formation = GetFormation(GetFrom());

            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;

                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    var kelpPos = IndexOf(rankOff, fileOff);
                    SetFormation(kelpPos, new Kelp(false, formation.GetColor()));
                }
            }
        }
    }
}