using System.Collections.Generic;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using Game.Action.Relics;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrostSigil : RelicLogic
    {
        private Tile.Tile hoveringTile;

        public FrostSigil(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
        }

        public override void Activate(List<Action.Action> actions)
        {
            
        }

        public override void ActiveForAI()
        {
            var bestAreas = new List<(int rank, int file, List<PieceLogic> enemies)>();
            var maxEnemyCount = 0;

            for (var rank = 1; rank < BoardUtils.MaxLength; rank++)
            for (var file = 1; file < BoardUtils.MaxLength; file++)
            {
                if (!BoardUtils.IsActive(BoardUtils.IndexOf(rank, file))) continue;
                var enemies = BoardUtils.GetPiecesInRadius(
                    rank,
                    file,
                    3,
                    piece => piece.Color != BoardUtils.OurSide());

                var count = enemies.Count;

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

            if (bestAreas.Count == 0) return;

            var bestArea = bestAreas[Random.Range(0, bestAreas.Count)];
            var pos = BoardUtils.IndexOf(bestArea.rank, bestArea.file);

            //Làm lại
            SetCooldown();
            var execute = new Game.Action.Relics.FrostSigilExecute(CommanderPiece, pos, Color);
            Game.Action.ActionManager.EnqueueAction(execute);
        }
    }
}