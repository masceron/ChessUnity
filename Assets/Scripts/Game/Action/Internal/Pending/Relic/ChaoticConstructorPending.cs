using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Relics;
using System.Collections.Generic;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class ChaoticConstructorPending : PendingAction, System.IDisposable, IRelicAction
    {
        private ChaoticConstructor chaoticConstructor;
        private int TurnToActive = 1;
        private int currentTurn = 1;
        private List<int> storedPos;
        private bool color;

        public ChaoticConstructorPending(bool color, int maker, ChaoticConstructor cc) : base(maker)
        {
            this.color = color;
            Maker = (ushort)maker;
            chaoticConstructor = cc;
            storedPos = new List<int>();
        }

       
        public override void CompleteAction()
        {
            BoardUtils.AddEffectObserver(new Effects.Others.ChaoticConstructorSpawn(-1, color, this, storedPos));
            TileManager.Ins.UnmarkAll();
            for (int i = 0; i < BoardSize; ++i) 
            { 
                var p = PieceOn(i);
                if (p == null || p.PieceRank != Game.Piece.PieceRank.Construct) continue;

                storedPos.Add(p.Pos);
                ActionManager.ExecuteImmediately(new KillPiece(p.Pos));
            }
        }

        

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
            chaoticConstructor = null;
        }
    }
}

