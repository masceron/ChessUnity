using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.Others
{
    public class BryozoanPassive : Effect, IStartTurnTrigger
    {
        private const int Interval = 1; // 1 for testing.
        private const int SummonRange = 1;
        private int turnCounter = 0;
        public BryozoanPassive(PieceLogic piece) : base(-1, 1, piece, "effect_bryozoan_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
        }

        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Buff;
        public StartTurnEffectType StartTurnEffectType { get; }
        public void OnCallStart(Action.Action lastMainAction)
        {
            turnCounter++;
            if (turnCounter % Interval == 0)
            {
                SummonBryozoan();
            }

        }
        
        private void SummonBryozoan()
        {
            var (rank, file) = RankFileOf(Piece.Pos);
            List<int> emptyTiles = new();

            foreach (var (r, f) in MoveEnumerators.AroundUntil(rank, file, SummonRange))
            {
                var index = IndexOf(r, f);
                if (PieceOn(index) == null)
                    emptyTiles.Add(index);
            }

            if (emptyTiles.Count == 0) return;

            var randomIndexTile = Random.Range(0, emptyTiles.Count);
            var spawnIndex = emptyTiles[randomIndexTile];

            // 1. Spawn Bryozoan
            ActionManager.EnqueueAction(
                new SpawnPiece(new PieceConfig(
                    "piece_bryozoan",
                    Piece.Color,
                    spawnIndex),
                    spawned =>
                    {
                        var spawnedPiece = PieceOn(spawnIndex);
                        if (spawnedPiece == null) return;
            
                        Effect randomEffect = GetRandomBryozoanEffect(spawnedPiece);
            
                        ActionManager.EnqueueAction(
                            new ApplyEffect(randomEffect, Piece)
                        );
                    })
            );
            
        }
        
        private Effect GetRandomBryozoanEffect(PieceLogic target)
        {
            int roll = Random.Range(0, 3);

            return roll switch
            {
                0 => new BryozoanTrueBite(target),
                1 => new BryozoanRally(target),
                2 => new BryozoanLongReach(target),
                _ => new BryozoanTrueBite(target)
            };
        }
    }
}