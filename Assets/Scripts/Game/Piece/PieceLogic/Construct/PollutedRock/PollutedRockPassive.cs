using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Tile;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Construct.PollutedRock
{
    /// <summary>
    /// Bioluminescent Beacon Passive Effect
    /// 
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class PollutedRockPassive : Effect, IEndTurnEffect
    {
        private const byte TurnsToActive = 2;
        private byte numTurns = TurnsToActive;
        
        private readonly (int, int)[] rangeSpawn = new (int, int)[4]
        {
            (1, 0), (0, -1),
            (-1, 0), (0, 1)
        };
        public PollutedRockPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.PollutedRockPassive)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            numTurns--;
            if (numTurns == 0)
            {
                numTurns = TurnsToActive;
                SpawnMedicalLeech();
            }
        }

        private void SpawnMedicalLeech()
        {
            var random = new System.Random();
            var emptySpots = rangeSpawn
                .Select(offset => IndexOf(RankOf(Piece.Pos) + offset.Item1, FileOf(Piece.Pos) + offset.Item2))
                .Where(index => PieceOn((ushort)index) == null)
                .ToList();

            if (emptySpots.Count > 0)
            {
                ushort indexToSpawn = (ushort)emptySpots[random.Next(emptySpots.Count)];
                ActionManager.ExecuteImmediately(
                    new SpawnPiece(new PieceConfig(PieceType.MedicinalLeach, Piece.Color, indexToSpawn))
                );
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; }
    }
}

