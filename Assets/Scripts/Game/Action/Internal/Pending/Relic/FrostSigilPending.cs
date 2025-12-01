using System.Collections.Generic;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;
using Game.AI;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class FrostSigilPending : Action, IPendingAble, System.IDisposable, IAIAction
    {
        private Tile.Tile thisTile;

        private int probabilityBound = 25;

        private FrostSigil frostSigil;
        public FrostSigilPending(int maker, Tile.Tile hoveringTile, FrostSigil fs) : base(maker)
        {
            thisTile = hoveringTile;
            Maker = (ushort)maker;
            frostSigil = fs;
        }

        public void CompleteAction()
        {
            var pieces = BoardUtils.GetPiecesInRadius(thisTile.rank, thisTile.file, 3, _ => true);

            foreach (var piece in pieces)
            {
                if (piece == null || piece.Color == MatchManager.Ins.GameState.OurSide) continue;

                ActionManager.ExecuteImmediately(new ApplyEffect(new Slow(3, 1, piece)));

                if (MatchManager.Roll(probabilityBound))
                {
                    ActionManager.ExecuteImmediately(new ApplyEffect(new Bound(3, piece)));
                }
            }

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            frostSigil.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();

            Dispose();
        }

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
            frostSigil = null;

            Tile.Tile.OnPointEnterHandle = null;
        }

        protected override void ModifyGameState()
        {
            throw new System.NotImplementedException();
        }

        public void CompleteActionForAI()
        {
            var bestArea = new List<PieceLogic>();
            var bestPieceAreas = new List<List<PieceLogic>>();
            var maxEnemyCount = 0;
            for (int rank = 1; rank <= BoardUtils.MaxLength - 1; rank++)
            {
                for (int file = 1; file <= BoardUtils.MaxLength - 1; file++)
                {
                    var enemy = BoardUtils.GetPiecesInRadius(rank, file, 3, piece => piece.Color != MatchManager.Ins.GameState.OurSide);
                    var enemyCount = enemy.Count;
                    if (enemyCount > maxEnemyCount)
                    {
                        bestPieceAreas.Clear();
                        bestPieceAreas.Add(enemy);
                        maxEnemyCount = enemyCount;
                    }
                    else if (enemyCount == maxEnemyCount && enemyCount > 0)
                    {
                        bestPieceAreas.Add(enemy);
                    }
                }
            }

            if (bestPieceAreas.Count == 0)
            {
                // No enemies found, do nothing
            } 
            else if (bestPieceAreas.Count == 1)
            {
                bestArea = bestPieceAreas[0];
            }
            else
            {
                bestArea = bestPieceAreas[UnityEngine.Random.Range(0, bestPieceAreas.Count)];
            }
            
            foreach (var piece in bestArea)
            {
                ActionManager.ExecuteImmediately(new ApplyEffect(new Slow(3, 1, piece)));
                if (MatchManager.Roll(probabilityBound))
                {
                    ActionManager.ExecuteImmediately(new ApplyEffect(new Bound(3, piece)));
                }
            }

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            frostSigil.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();

            Dispose();
        }
    }
}

