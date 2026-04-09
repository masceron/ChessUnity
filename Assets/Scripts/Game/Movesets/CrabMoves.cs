using System.Collections.Generic;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public static class CrabMoves
	{
		public static int Quiets(List<int> list, int pos)
		{
			var file = FileOf(pos);
			var rank = RankOf(pos);
			var caller = PieceOn(pos);
			var effectiveMoveRange = caller.GetMoveRange();

			foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, effectiveMoveRange))
				MakeMove(rankOff, fileOff);

			return 10 + 10 * effectiveMoveRange;

			bool IsForbiddenSquare(int rankOff, int fileOff)
			{
				return fileOff == file && (rankOff == rank + 1 || rankOff == rank - 1);
			}

			void MakeMove(int rankOff, int fileOff)
			{
				if (IsForbiddenSquare(rankOff, fileOff)) return;
				var index = IndexOf(rankOff, fileOff);
				if (!IsActive(index)) return;
				var piece = PieceOn(index);
				if (piece != null ||
					Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
					return;
				list.Add(index);
			}
		}

		public static int Captures(List<int> list, int pos)
		{
			var file = FileOf(pos);
			var rank = RankOf(pos);
			var caller = PieceOn(pos);
			var attackRange = caller.GetAttackRange();

			foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, attackRange))
				MakeCapture(rankOff, fileOff);

			return 10 + 10 * attackRange;

			bool IsForbiddenSquare(int rankOff, int fileOff)
			{
				return fileOff == file && (rankOff == rank + 1 || rankOff == rank - 1);
			}

			void MakeCapture(int rankOff, int fileOff)
			{
				if (IsForbiddenSquare(rankOff, fileOff)) return;
				var index = IndexOf(rankOff, fileOff);
				if (!IsActive(index)) return;
				if (Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1) return;
				list.Add(index); // trả tất cả ô reachable
			}
		}
	}
}
