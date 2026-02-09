using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class RedtailParrotfishPending : PendingAction, System.IDisposable
	{
		public static int formationPos = -1;
		public static int moveTo = -1;
		private readonly PieceLogic redtail;

		public RedtailParrotfishPending(int maker, int target) : base(maker)
		{
			Maker = (ushort)maker;
			Target = (ushort)target;
			redtail = PieceOn(Maker);
		}

		public override void CompleteAction()
		{
			if (formationPos == -1)
			{
				formationPos = Target;
				TileManager.Ins.UnmarkAll();
				BoardViewer.ListOf.Clear();
				for (int i = 0; i < BoardSize; ++i)
				{
					if (!IsActive(i)) { return; }
					Formation formation = GetFormation(i);
					if (formation != null) { continue; }
					if (IsOnBlackSide(Maker) == redtail.Color)
					{
						BoardViewer.ListOf.Add(new RedtailParrotfishPending(Maker, i));
						TileManager.Ins.MarkAsMoveable(i);
					}
				}
			}
			else
			{
				moveTo = Target;
				BoardViewer.Ins.ExecuteAction(new RedtailParrotfishActive(Maker, formationPos, moveTo));
				Reset();
			}
		}

		public int AIPenaltyValue(PieceLogic p)
		{
			return 0;
		}

		public void Dispose()
		{
			Reset();
			BoardViewer.SelectingFunction = 0;
		}

		private static void Reset()
		{
			formationPos = -1;
			moveTo = -1;
		}
	}
}