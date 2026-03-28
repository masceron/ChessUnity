using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using static Game.Common.BoardUtils;

namespace Game.Movesets
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public static class CoralMoves
	{
		private static readonly int[] RankOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
		private static readonly int[] FileOffsets = { -2, 0, 2, -1, 1, -2, 0, 2 };

		public static int Quiets(List<Action.Action> list, int pos, bool isPlayer)
		{
			var file = FileOf(pos);
			var rank = RankOf(pos);
			var caller = PieceOn(pos);
			var range = caller.GetMoveRange();

			for (var i = 0; i < RankOffsets.Length; i++)
			{
				var rankOff = rank + RankOffsets[i];
				var fileOff = file + FileOffsets[i];

				MakeMove(rankOff, fileOff);
			}

			return 20 + 10 * range;

			void MakeMove(int rankOff, int fileOff)
			{
				var index = IndexOf(rankOff, fileOff);
				if (!IsActive(index)) return;

				var piece = PieceOn(index);
				if (piece != null) return;

				list.Add(new NormalMove(pos, index));
			}
		}

		public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
		{
			var file = FileOf(pos);
			var rank = RankOf(pos);
			var caller = PieceOn(pos);
			var range = caller.GetAttackRange();
			var color = caller.Color;

			for (var i = 0; i < RankOffsets.Length; i++)
			{
				var rankOff = rank + RankOffsets[i];
				var fileOff = file + FileOffsets[i];

				MakeCapture(rankOff, fileOff);
			}

			return 20 + 10 * range;

			void MakeCapture(int rankOff, int fileOff)
			{
				var index = IndexOf(rankOff, fileOff);
				if (!IsActive(index)) return;

				var piece = PieceOn(index);
				if (piece == null)
				{
					if (!isPlayer)
						list.Add(new NormalCapture(pos, index));
					return;
				}

				if (piece.Color != color)
					list.Add(new NormalCapture(pos, index));
			}
		}
	}
}
