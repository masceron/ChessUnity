using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Piece
{
	//Làm lại
	// public class ToxicZoanthidPending : PendingAction, IDisposable, ISkills
	// {
	// 	private static readonly List<int> SelectedPositions = new();
	// 	private static int _targetCount;
	// 	public ToxicZoanthidPending(PieceLogic maker, int target) : base(maker, target)
	// 	{
	// 		_targetCount = GetMakerAsPiece().GetStat(SkillStat.Target);
	// 		SelectedPositions.Clear();
	// 	}
	//
	// 	public int AIPenaltyValue(PieceLogic maker)
	// 	{
	// 		return 0;
	// 	}
	//
	// 	public void Dispose()
	// 	{
	// 		Reset();
	// 		BoardViewer.SelectingFunction = 0;
	// 	}
	//
	// 	protected override void CompleteAction()
	// 	{
	// 		SelectedPositions.Add(GetTargetPos());
	// 		TileManager.Ins.UnMark(GetTargetPos());
	//
	// 		if (SelectedPositions.Count < _targetCount) return;
	//
	// 		CommitResult(new ToxicZoanthidActive(GetMakerAsPiece(), new List<int>(SelectedPositions)));
	// 		Reset();
	// 	}
	//
	// 	private static void Reset()
	// 	{
	// 		SelectedPositions.Clear();
	// 		_targetCount = 0;
	// 	}
	// }
}
