using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Common
{
    public static class SkillRangeHelper
    {
        // --- Radius Methods ---

        /// <summary>
        ///     Gets a list of active cells containing Ally pieces within a specified radius around the maker.
        /// </summary>
        /// <param name="makerPos">The position index of the unit casting the skill.</param>
        /// <param name="radius">The radius range to search within.</param>
        /// <returns>A list of board indices matching the criteria.</returns>
        public static List<int> GetActiveAllyPieceInRadius(int makerPos, int radius)
        {
            return GetTargetsInRadius(makerPos, radius, TargetType.Ally);
        }

        /// <summary>
        ///     Gets a list of Enemy pieces within a specified radius around the maker.
        /// </summary>
        /// <param name="maker">The position index of the unit casting the skill.</param>
        /// <param name="radius">The radius range to search within.</param>
        /// <returns>A list of PieceLogic matching the criteria.</returns>
        public static List<PieceLogic> GetActiveEnemyPieceInRadius(Entity maker, int radius)
        {
            return GetTargetsInRadius(maker.Pos, radius, TargetType.Enemy).Select(BoardUtils.PieceOn).ToList();
        }

        /// <summary>
        ///     Gets a list of all active cells (occupied or empty) within a specified radius around the maker.
        /// </summary>
        /// <param name="makerPos">The position index of the unit casting the skill.</param>
        /// <param name="radius">The radius range to search within.</param>
        /// <returns>A list of board indices matching the criteria.</returns>
        public static List<int> GetActiveCellInRadius(int makerPos, int radius)
        {
            return GetTargetsInRadius(makerPos, radius, TargetType.Any);
        }

        // --- Direction Methods: Up ---

        public static List<int> GetActiveCellInDirectionUp(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Any, Direction.Up);
        }

        public static List<int> GetActiveAllyPieceInDirectionUp(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Ally, Direction.Up);
        }

        public static List<int> GetActiveEnemyPieceInDirectionUp(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Enemy, Direction.Up);
        }

        // --- Direction Methods: Down ---

        public static List<int> GetActiveCellInDirectionDown(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Any, Direction.Down);
        }

        public static List<int> GetActiveAllyPieceInDirectionDown(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Ally, Direction.Down);
        }

        public static List<int> GetActiveEnemyPieceInDirectionDown(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Enemy, Direction.Down);
        }

        // --- Direction Methods: Left ---

        public static List<int> GetActiveCellInDirectionLeft(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Any, Direction.Left);
        }

        public static List<int> GetActiveAllyPieceInDirectionLeft(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Ally, Direction.Left);
        }

        public static List<int> GetActiveEnemyPieceInDirectionLeft(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Enemy, Direction.Left);
        }

        // --- Direction Methods: Right ---

        public static List<int> GetActiveCellInDirectionRight(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Any, Direction.Right);
        }

        public static List<int> GetActiveAllyPieceInDirectionRight(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Ally, Direction.Right);
        }

        public static List<int> GetActiveEnemyPieceInDirectionRight(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Enemy, Direction.Right);
        }

        // --- Direction Methods: UpLeft ---

        public static List<int> GetActiveCellInDirectionUpLeft(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Any, Direction.UpLeft);
        }

        public static List<int> GetActiveAllyPieceInDirectionUpLeft(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Ally, Direction.UpLeft);
        }

        public static List<int> GetActiveEnemyPieceInDirectionUpLeft(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Enemy, Direction.UpLeft);
        }

        // --- Direction Methods: UpRight ---

        public static List<int> GetActiveCellInDirectionUpRight(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Any, Direction.UpRight);
        }

        public static List<int> GetActiveAllyPieceInDirectionUpRight(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Ally, Direction.UpRight);
        }

        public static List<int> GetActiveEnemyPieceInDirectionUpRight(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Enemy, Direction.UpRight);
        }

        // --- Direction Methods: DownLeft ---

        public static List<int> GetActiveCellInDirectionDownLeft(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Any, Direction.DownLeft);
        }

        public static List<int> GetActiveAllyPieceInDirectionDownLeft(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Ally, Direction.DownLeft);
        }

        public static List<int> GetActiveEnemyPieceInDirectionDownLeft(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Enemy, Direction.DownLeft);
        }

        // --- Direction Methods: DownRight ---

        public static List<int> GetActiveCellInDirectionDownRight(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Any, Direction.DownRight);
        }

        public static List<int> GetActiveAllyPieceInDirectionDownRight(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Ally, Direction.DownRight);
        }

        public static List<int> GetActiveEnemyPieceInDirectionDownRight(int makerPos, int range)
        {
            return GetTargetsInDirection(makerPos, range, TargetType.Enemy, Direction.DownRight);
        }

        // --- Single Pos At Range Methods ---

        public static List<int> GetActivePosAtRangeInDirectionUp(int makerPos, int range)
        {
            return GetSinglePosInDirection(makerPos, range, Direction.Up);
        }

        public static List<int> GetActivePosAtRangeInDirectionDown(int makerPos, int range)
        {
            return GetSinglePosInDirection(makerPos, range, Direction.Down);
        }

        public static List<int> GetActivePosAtRangeInDirectionLeft(int makerPos, int range)
        {
            return GetSinglePosInDirection(makerPos, range, Direction.Left);
        }

        public static List<int> GetActivePosAtRangeInDirectionRight(int makerPos, int range)
        {
            return GetSinglePosInDirection(makerPos, range, Direction.Right);
        }

        public static List<int> GetActivePosAtRangeInDirectionUpLeft(int makerPos, int range)
        {
            return GetSinglePosInDirection(makerPos, range, Direction.UpLeft);
        }

        public static List<int> GetActivePosAtRangeInDirectionUpRight(int makerPos, int range)
        {
            return GetSinglePosInDirection(makerPos, range, Direction.UpRight);
        }

        public static List<int> GetActivePosAtRangeInDirectionDownLeft(int makerPos, int range)
        {
            return GetSinglePosInDirection(makerPos, range, Direction.DownLeft);
        }

        public static List<int> GetActivePosAtRangeInDirectionDownRight(int makerPos, int range)
        {
            return GetSinglePosInDirection(makerPos, range, Direction.DownRight);
        }

        // --- Global Methods ---

        /// <summary>
        ///     Gets a list of all active cells on the entire board.
        /// </summary>
        /// <returns>A list of all active board indices.</returns>
        public static List<int> GetActiveCellGlobal()
        {
            return GetTargetsGlobal(TargetType.Any, -1);
        }

        /// <summary>
        ///     Gets a list of all active cells containing Ally pieces on the entire board.
        /// </summary>
        /// <param name="makerPos">The position index of the unit casting the skill (used to determine side).</param>
        /// <returns>A list of board indices matching the criteria.</returns>
        public static List<int> GetActiveAllyPieceGlobal(int makerPos)
        {
            return GetTargetsGlobal(TargetType.Ally, makerPos);
        }

        /// <summary>
        ///     Gets a list of all active cells containing Enemy pieces on the entire board.
        /// </summary>
        /// <param name="makerPos">The position index of the unit casting the skill (used to determine side).</param>
        /// <returns>A list of board indices matching the criteria.</returns>
        public static List<int> GetActiveEnemyPieceGlobal(int makerPos)
        {
            return GetTargetsGlobal(TargetType.Enemy, makerPos);
        }

        private static List<int> GetTargetsInRadius(int makerPos, int radius, TargetType type)
        {
            var results = new List<int>();

            if (!BoardUtils.VerifyIndex(makerPos)) return results;

            var makerPiece = BoardUtils.PieceOn(makerPos);

            if (type != TargetType.Any && makerPiece == null) return results;

            var makerColor = makerPiece != null && makerPiece.Color;

            var (rank, file) = BoardUtils.RankFileOf(makerPos);

            foreach (var (r, f) in MoveEnumerators.AroundUntil(rank, file, radius))
            {
                var index = BoardUtils.IndexOf(r, f);
                if (CheckCondition(index, type, makerColor)) results.Add(index);
            }

            return results;
        }

        private static List<int> GetTargetsInDirection(int makerPos, int range, TargetType type, Direction dir)
        {
            var results = new List<int>();

            if (!BoardUtils.VerifyIndex(makerPos)) return results;

            var makerPiece = BoardUtils.PieceOn(makerPos);

            if (type != TargetType.Any && makerPiece == null) return results;

            var makerColor = false;
            if (makerPiece != null) makerColor = makerPiece.Color;

            // Determine final direction (inverted for Black)
            var finalDir = DetermineDirection(dir, makerColor);
            var dirEnum = GetEnumerator(finalDir);

            var (startRank, startFile) = BoardUtils.RankFileOf(makerPos);

            foreach (var (r, f) in dirEnum(startRank, startFile, range))
            {
                var index = BoardUtils.IndexOf(r, f);
                if (CheckCondition(index, type, makerColor)) results.Add(index);
            }

            return results;
        }

        private static List<int> GetSinglePosInDirection(int makerPos, int range, Direction dir)
        {
            var results = new List<int>();

            if (!BoardUtils.VerifyIndex(makerPos)) return results;

            var makerPiece = BoardUtils.PieceOn(makerPos);

            if (makerPiece == null) return results;

            var finalDir = DetermineDirection(dir, makerPiece.Color);
            var (dRank, dFile) = GetDirectionOffsets(finalDir);

            return GetActivePosAtOffset(makerPos, range, dRank, dFile);
        }

        private static List<int> GetTargetsGlobal(TargetType type, int makerPos)
        {
            var results = new List<int>();
            var makerColor = false;

            if (type != TargetType.Any)
            {
                var p = BoardUtils.PieceOn(makerPos);
                if (p == null) return results;
                makerColor = p.Color;
            }

            for (var i = 0; i < BoardUtils.BoardSize; i++)
                if (CheckCondition(i, type, makerColor))
                    results.Add(i);

            return results;
        }

        private static bool CheckCondition(int index, TargetType type, bool makerColor)
        {
            if (!BoardUtils.VerifyIndex(index)) return false;
            if (!BoardUtils.IsActive(index)) return false;

            var piece = BoardUtils.PieceOn(index);

            if (type == TargetType.Any) return true;

            if (piece == null) return false;

            if (type == TargetType.Ally) return piece.Color == makerColor;
            if (type == TargetType.Enemy) return piece.Color != makerColor;

            return false;
        }

        private static List<int> GetActivePosAtOffset(int makerPos, int range, int dRank, int dFile)
        {
            var results = new List<int>();
            var (r, f) = BoardUtils.RankFileOf(makerPos);
            var tr = r + dRank * range;
            var tf = f + dFile * range;

            if (!BoardUtils.VerifyBounds(tr) || !BoardUtils.VerifyBounds(tf)) return results;

            var index = BoardUtils.IndexOf(tr, tf);
            if (BoardUtils.IsActive(index)) results.Add(index);

            return results;
        }

        // --- Direction Utils ---

        private static Direction DetermineDirection(Direction inputDir, bool isBlack)
        {
            // If False (White), Up is Up (Rank-). Right is Right (File+).
            // If True (Black), Up is Down (Rank+). Right is Left (File-).
            if (!isBlack) return inputDir;

            switch (inputDir)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
                case Direction.UpLeft: return Direction.DownRight;
                case Direction.UpRight: return Direction.DownLeft;
                case Direction.DownLeft: return Direction.UpRight;
                case Direction.DownRight: return Direction.UpLeft;
                default: return inputDir;
            }
        }

        private static DirectionEnumerator GetEnumerator(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return MoveEnumerators.Up;
                case Direction.Down: return MoveEnumerators.Down;
                case Direction.Left: return MoveEnumerators.Left;
                case Direction.Right: return MoveEnumerators.Right;
                case Direction.UpLeft: return MoveEnumerators.UpLeft;
                case Direction.UpRight: return MoveEnumerators.UpRight;
                case Direction.DownLeft: return MoveEnumerators.DownLeft;
                case Direction.DownRight: return MoveEnumerators.DownRight;
                default: return MoveEnumerators.Up;
            }
        }

        private static (int, int) GetDirectionOffsets(Direction dir)
        {
            // Based on MoveEnumerators logic:
            // Up = Rank - 1
            // Down = Rank + 1
            // Left = File - 1
            // Right = File + 1
            switch (dir)
            {
                case Direction.Up: return (-1, 0);
                case Direction.Down: return (1, 0);
                case Direction.Left: return (0, -1);
                case Direction.Right: return (0, 1);
                case Direction.UpLeft: return (-1, -1);
                case Direction.UpRight: return (-1, 1);
                case Direction.DownLeft: return (1, -1);
                case Direction.DownRight: return (1, 1);
                default: return (0, 0);
            }
        }

        // --- Helpers ---

        private enum TargetType
        {
            Any,
            Ally,
            Enemy
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight
        }

        private delegate IEnumerable<(int, int)> DirectionEnumerator(int rank, int file, int range);
    }
}