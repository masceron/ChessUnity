using Game.Action.Internal;
using static Game.Common.BoardUtils;
using System.Collections.Generic;

namespace Game.Action.Relics
{
    public class ChaoticConstructorAction : Action, IRelicAction
    {
    private readonly List<int> _storedPos;

        public ChaoticConstructorAction(int maker) : base(maker)
        {
            Target = (ushort)maker;
            Maker = (ushort)maker;
            _storedPos = new List<int>();
        }

        protected override void ModifyGameState()
        {
            for (int i = 0; i < BoardSize; ++i)
            {
                var p = PieceOn(i);
                if (p == null || p.PieceRank != Game.Piece.PieceRank.Construct) continue;

                _storedPos.Add(p.Pos);
                ActionManager.EnqueueAction(new KillPiece(p.Pos));
            }
            AddEffectObserver(new Effects.Others.ChaoticConstructorSpawn(-1, _storedPos));
        }
    }
}