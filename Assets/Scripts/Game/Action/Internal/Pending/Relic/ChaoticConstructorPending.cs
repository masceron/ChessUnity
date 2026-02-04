using Game.Action.Relics;
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
        private readonly List<int> _storedPos;
        private readonly bool _color;

        public ChaoticConstructorPending(bool color, int maker, ChaoticConstructor cc) : base(maker)
        {
            _color = color;
            Maker = (ushort)maker;
            _storedPos = new List<int>();
        }

       
        public override void CompleteAction()
        {
            AddEffectObserver(new Effects.Others.ChaoticConstructorSpawn(-1, this, _storedPos));
            TileManager.Ins.UnmarkAll();
            for (int i = 0; i < BoardSize; ++i) 
            { 
                var p = PieceOn(i);
                if (p == null || p.PieceRank != Game.Piece.PieceRank.Construct) continue;

                _storedPos.Add(p.Pos);
                ActionManager.ExecuteImmediately(new KillPiece(p.Pos));
            }
        }

        

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
        }
    }
}

