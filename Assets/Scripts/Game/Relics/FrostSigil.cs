using System.Linq;
using Game.Action;
using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Managers;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;
using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class FrostSigil : RelicLogic
    {
        private Tile.Tile hoveringTile;
        public FrostSigil(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;

                Tile.Tile.OnPointEnterHandle = (thisTile) =>
                {
                    if (hoveringTile != null)
                    {
                        TileManager.Ins.MarkTileInRange(hoveringTile, 3, isMark: false);
                    }

                    hoveringTile = thisTile;
                    TileManager.Ins.MarkTileInRange(hoveringTile, 3, isMark: true, onlyMarkEnemy: false);

                    var pos = BoardUtils.IndexOf(hoveringTile.rank, hoveringTile.file);
                    var pending = new FrostSigilPending(pos, hoveringTile, this);

                    if (!BoardViewer.ListOf.Contains(pending, new ActionComparer()))
                    {
                        BoardViewer.ListOf.Add(pending);
                    }
                };
            }
            else
            {
                Debug.Log("Frost Sigil is on cooldown for " + CurrentCooldown + "more turn");
            }
        }

        public override void ActiveForAI()
        {
            var bestAreas = new List<(int rank, int file, List<PieceLogic> enemies)>();
            int maxEnemyCount = 0;

            for (int rank = 1; rank < BoardUtils.MaxLength; rank++)
            {
                for (int file = 1; file < BoardUtils.MaxLength; file++)
                {
                    if (!BoardUtils.IsActive(BoardUtils.IndexOf(rank, file))) continue;
                    var enemies = BoardUtils.GetPiecesInRadius(
                        rank,
                        file,
                        3,
                        piece => piece.Color != MatchManager.Ins.GameState.OurSide);

                    int count = enemies.Count;

                    if (count > maxEnemyCount)
                    {
                        bestAreas.Clear();
                        bestAreas.Add((rank, file, enemies));
                        maxEnemyCount = count;
                    }
                    else if (count == maxEnemyCount && count > 0)
                    {
                        bestAreas.Add((rank, file, enemies));
                    }
                }
            }
            
            if (bestAreas.Count == 0)
            {
                return;  
            } 

            var bestArea = bestAreas[Random.Range(0, bestAreas.Count)];
            var pos = BoardUtils.IndexOf(bestArea.rank, bestArea.file);
            var hoveringTile = TileManager.Ins.GetTile(pos);

            var pending = new FrostSigilPending(pos, hoveringTile, this);
            BoardViewer.Ins.ExecuteAction(pending);
        }
    }
}

