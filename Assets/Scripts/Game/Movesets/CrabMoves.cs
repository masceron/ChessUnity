using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public static class CrabMoves
	{
		public static int Quiets(List<Action.Action> list, int pos, bool isPlayer)
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
				list.Add(new NormalMove(caller, index));
			}
		}

		public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
		{
			var file = FileOf(pos);
			var rank = RankOf(pos);
			var caller = PieceOn(pos);

			var color = caller.Color;
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
				var piece = PieceOn(index);
				if (piece == null && !isPlayer)
				{
					list.Add(new NormalCapture(caller, piece));
				}
				else if (piece != null)
				{
					if (piece.Color == color ||
						Pathfinder.LineBlocker(rank, file, rankOff, fileOff).Item1 != -1)
						return;
					list.Add(new NormalCapture(caller, piece));
				}
			}
		}
	}
}
