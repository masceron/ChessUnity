using Game.Action;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    /// <summary>
    /// Bioluminescent Beacon Passive Effect
    /// 
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class PollutedRockPassive : Effect, IEndTurnTrigger
    {
        private const byte TurnsToActive = 2;
        private byte _numTurns = TurnsToActive;
        
        private readonly (int, int)[] _rangeSpawn = new (int, int)[]
        {
            (1, 0), (0, -1),
            (-1, 0), (0, 1)
        };
        public PollutedRockPassive(PieceLogic piece) : base(-1, 1, piece, "effect_polluted_rock_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            _numTurns--;
            if (_numTurns != 0) return;
            _numTurns = TurnsToActive;
            SpawnMedicalLeech();
        }

        private void SpawnMedicalLeech()
        {
            var random = new System.Random();
            var emptySpots = _rangeSpawn
                .Select(offset => IndexOf(RankOf(Piece.Pos) + offset.Item1, FileOf(Piece.Pos) + offset.Item2))
                .Where(index => PieceOn((ushort)index) == null)
                .ToList();

            if (emptySpots.Count > 0)
            {
                var indexToSpawn = emptySpots[random.Next(emptySpots.Count)];
                ActionManager.EnqueueAction(
                    new SpawnPiece(new PieceConfig("piece_medicinal_leech", Piece.Color, indexToSpawn))
                );
            }
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;

        public EndTurnEffectType EndTurnEffectType { get; }
    }
}

