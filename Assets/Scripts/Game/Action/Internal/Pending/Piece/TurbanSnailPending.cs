using System;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;

// <-- thêm để dùng LINQ

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TurbanSnailPending : PendingAction
    {

        public TurbanSnailPending(int maker) : base(maker)
        {
            Target = maker;
        }


        protected override void CompleteAction()
        {
            CommitResult(new TurbanSnailActive(Maker));
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
    }
}