using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;
using static Game.Common.BoardUtils;
using ZLinq;
using Game.Common;

namespace Game.Effects.SpecialAbility
{
    public class OliveRidleyEggsPassive : Effect, IStartTurnTrigger
    {
        private int roundToSpawn;
        public OliveRidleyEggsPassive(PieceLogic piece) : base(-1, 1, piece, "effect_olive_ridley_eggs_passive")
        {
            SetStat(EffectStat.Counter, 5);
            SetStat(EffectStat.Number, 1, 1);
            SetStat(EffectStat.Number, 1, 2);
            roundToSpawn = 0;
        }
        StartTurnTriggerPriority IStartTurnTrigger.Priority => StartTurnTriggerPriority.Buff;
        public StartTurnEffectType StartTurnEffectType => StartTurnEffectType.StartOfAllyTurn;
        public void OnCallStart(Action.Action lastMainAction)
        {
            int number1 = GetStat(EffectStat.Number, 1);
            int number2 = GetStat(EffectStat.Number, 2);
            if (Piece == null) return;
            roundToSpawn++;

            int counter = GetStat(EffectStat.Counter);
            if (roundToSpawn < counter) return;

            var (rank, file) = RankFileOf(Piece.Pos);
            List<int> candidateTiles = new List<int>();

            foreach (var (r, f) in MoveEnumerators.AroundUntil(rank, file, 1))
            {
                int index = IndexOf(r, f);
                if (!IsActive(index)) continue;
                if (PieceOn(index) != null) continue;

                candidateTiles.Add(index);
            }
            if (candidateTiles.Count == 0) return;

            int countSpawn = Mathf.Min(candidateTiles.Count, number2, number1);
            for (int i = 0; i < countSpawn; i++)
            {
                int randIdx = Random.Range(0, candidateTiles.Count);
                int spawnPos = candidateTiles[randIdx];
                candidateTiles.RemoveAt(randIdx);

                ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig("piece_olive_ridley_hatchling", Piece.Color, spawnPos)));
            }
        }

    }
}