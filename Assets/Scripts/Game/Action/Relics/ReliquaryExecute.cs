using System;
using Game.Common;
using Game.Managers;
using Game.Tile;
using MemoryPack;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class ReliquaryExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        public ReliquaryExecute() : base(-1)
        {
        }

        protected override void ModifyGameState()
        {
            var fowPos = GetRandomPos();
            var fogOfWar = new FogOfWar(true);
            fogOfWar.SetPositon(fowPos);
            BoardUtils.SetFormation(fowPos, fogOfWar);

            var saposPos = GetRandomPos();
            var saprolegnia = new Saprolegnia(false, true);
            saprolegnia.SetPositon(saposPos);
            BoardUtils.SetFormation(saposPos, saprolegnia);
        }

        private int GetRandomPos()
        {
            var pos = new Random().Next(1, MatchManager.Ins.StartingSize.x * MatchManager.Ins.StartingSize.y);
            while (!TileManager.Ins.IsTileEmpty(pos) && BoardUtils.GetFormation(pos) != null)
                pos = new Random().Next(1, MatchManager.Ins.StartingSize.x * MatchManager.Ins.StartingSize.y);
            var mappedPos = BoardUtils.PosMap(pos, MatchManager.Ins.StartingSize);
            return mappedPos;
        }
    }
}