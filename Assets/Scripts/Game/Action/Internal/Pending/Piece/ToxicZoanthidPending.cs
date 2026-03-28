using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
	public class ToxicZoanthidPending : PendingAction, IDisposable, ISkills
	{
		private static readonly List<int> SelectedPositions = new();
		private static int _targetCount;
		public ToxicZoanthidPending(int maker, int target) : base(maker)
		{
			Maker = maker;
			Target = target;
			var makerPiece = PieceOn(maker);
			_targetCount = makerPiece.GetStat(SkillStat.Target);
			SelectedPositions.Clear();
		}

		public int AIPenaltyValue(PieceLogic maker)
		{
			return 0;
		}

		public void Dispose()
		{
			Reset();
			BoardViewer.SelectingFunction = 0;
		}

		protected override void CompleteAction()
		{

			SelectedPositions.Add(Target);
			TileManager.Ins.UnMark(Target);

			if (SelectedPositions.Count < _targetCount) return;

			CommitResult(new ToxicZoanthidActive(Maker, new List<int>(SelectedPositions)));
			Reset();
		}

		private static void Reset()
		{
			SelectedPositions.Clear();
			_targetCount = 0;
		}
	}
}
