using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UnityEngine;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FractureZonePassive : Effect, IEndTurnEffect
    {
        private int countToSpawnEffect;
        private readonly int intervalToSpawn = 2;
        private readonly int radius = 2;
        private readonly int aliveTime = 10;
        private List<(int,int)> tileInradius = new List<(int, int)>();

        public FractureZonePassive(PieceLogic piece) : base(-1, 1, piece, "effect_fracture_zone_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;

            var (rank, file) = BoardUtils.RankFileOf(piece.Pos);
            for (var  i = rank-radius;  i <= rank + radius;  i++)
            {
                for (var j = file - radius; j <= file + radius; j++)
                {
                    if (i == rank && j == file) continue;
                    if (BoardUtils.VerifyBounds(i) 
                        && BoardUtils.VerifyBounds(j) 
                        && BoardUtils.IsActive(BoardUtils.IndexOf(i, j)))
                    {
                        tileInradius.Add((i, j));
                    }
                }

            }
        }

        public EndTurnEffectType EndTurnEffectType
        { get; private set; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            countToSpawnEffect++;

            if (countToSpawnEffect < intervalToSpawn) return;
            countToSpawnEffect = 0;

            RandomApplyEffectInRadius();
        }

        private void RandomApplyEffectInRadius()
        {
            var (randRank, randFile) = GetRandomPos();
            if (randRank == null || randFile == null) return;

            Formation bubbleVent = new BubbleVent(aliveTime, true, Piece.Color);
            FormationManager.Ins.SetFormation(BoardUtils.IndexOf(randRank.Value, randFile.Value), bubbleVent);

            var pieceOn = BoardUtils.PieceOn(BoardUtils.IndexOf(randRank.Value, randFile.Value));
            if (pieceOn != null)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, pieceOn)));
            }
        }

        private (int?, int?) GetRandomPos()
        {
            var availableTiles = new List<(int, int)>();
            availableTiles.AddRange(tileInradius);
            
            do
            {
                if (availableTiles.Count == 0) return (null, null);

                var randIndex = Random.Range(0, availableTiles.Count);
                var (randRank, randFile) = availableTiles[randIndex];
                var formation = FormationManager.Ins.GetFormation(BoardUtils.IndexOf(randRank, randFile));
                if (formation == null)
                {
                    availableTiles.RemoveAt(randIndex);

                    return (randRank, randFile);
                }
            } while (true);
        }
    }

}
