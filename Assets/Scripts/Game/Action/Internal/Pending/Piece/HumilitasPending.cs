using System;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using Game.Managers;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class HumilitasPending : PendingAction, IDisposable, ISkills
	{
		private static int _firstTargetPos = -1;
		private static int _secondTargetPos = -1;

		public HumilitasPending(int makerPos, int targetPos) : base(PieceOn(makerPos), targetPos)
		{
		}

		public void Dispose()
		{
			ResetTargets();
			BoardViewer.SelectingFunction = 0;
		}

		protected override void CompleteAction()
		{
			var maker = GetMakerAsPiece();
			var target = PieceOn(Target);
			if (maker == null || target == null) return;
			if (target.Color == maker.Color) return;

			if (_firstTargetPos == -1)
			{
				_firstTargetPos = Target;
				TileManager.Ins.UnMark(_firstTargetPos);
				return;
			}

			_secondTargetPos = Target;

			var first = PieceOn(_firstTargetPos);
			var second = PieceOn(_secondTargetPos);

			CommitResult(new HumilitasActive(maker, first.ID, second.ID));
			ResetTargets();
		}

		public int AIPenaltyValue(PieceLogic pieceAI)
		{
			var maker = GetMakerAsPiece();
			if (maker == null || pieceAI == null) return 0;
			return pieceAI.Color != maker.Color ? -20 : 0;
		}

		private static void ResetTargets()
		{
			_firstTargetPos = -1;
			_secondTargetPos = -1;
		}
	}
}
