using System.Collections.Generic;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Effects.Others;
using Game.Managers;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StormCapacitor : RelicLogic
    {
        private const int Size = 2;
        private readonly Charge charge;
        private Tile.Tile hoveringTile;

        public StormCapacitor(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
            charge = new Charge(0, Color);
            BoardUtils.AddEffectObserver(charge);
        }

        public override void Activate()
        {
            Debug.Log("Charge: " + charge.Strength);
            if (charge.Strength >= 3)
            {
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;

                Tile.Tile.OnPointEnterHandle = thisTile =>
                {
                    if (hoveringTile == thisTile) return;
                    if (hoveringTile != null) TileManager.Ins.MarkTileInRange(hoveringTile, Size, false);

                    hoveringTile = thisTile;
                    TileManager.Ins.MarkTileInRange(hoveringTile, Size, true);
                    
                    //Làm lại
                    // var pending = new StormCapacitorPending(hoveringTile, this, Size);

                    // if (!BoardViewer.ListOf.Contains(pending, new ActionComparer())) BoardViewer.ListOf.Add(pending);
                };
            }

            if (CurrentCooldown > 0) charge.Strength = 0;
        }

        public override void ActiveForAI()
        {
            if (charge.Strength < 3) return;
            var maxSize = 0;
            var topGroup = new List<int>();
            for (var i = 0; i < BoardUtils.BoardSize; ++i)
            {
                var (rank, file) = BoardUtils.RankFileOf(i);
                var pieces = BoardUtils.GetPiecesInSize(rank, file, Size, Corner.BottomRight,
                    p => p != null && p.Color != Color);

                if (pieces.Count > maxSize)
                {
                    maxSize = pieces.Count;
                    topGroup.Clear();
                }

                if (pieces.Count == maxSize) topGroup.Add(i);
            }

            var pos = topGroup[Random.Range(0, topGroup.Count() - 1)];
            var maxArea = BoardUtils.GetPiecesInSize(BoardUtils.RankOf(pos), BoardUtils.FileOf(pos), Size,
                Corner.BottomRight, p => p != null && p.Color != Color);
            foreach (var piece in maxArea) BoardViewer.Ins.ExecuteAction(new ApplyEffect(new Stunned(2, piece)));

            charge.Strength = 0;
        }
    }
}