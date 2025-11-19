using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
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
        
        private readonly (int, int)[] rangeSpawn = new (int, int)[]
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
                var indexToSpawn = (ushort)emptySpots[random.Next(emptySpots.Count)];
                ActionManager.ExecuteImmediately(
                    new SpawnPiece(new PieceConfig("piece_medicinal_leech", Piece.Color, indexToSpawn))
                );
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; }
    }
}

