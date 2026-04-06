using System.Collections.Generic;
using Game.Action.Internal;
using Game.Effects.Others;
using Game.Piece;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class ChaoticConstructorAction : Action, IRelicAction
    {
        [MemoryPackInclude] private List<int> _storedPos = new();

        public ChaoticConstructorAction() : base(null)
        {
        }

        protected override void ModifyGameState()
        {
            _storedPos.Clear();
            for (var i = 0; i < BoardSize; ++i)
            {
                var p = PieceOn(i);
                if (p is not { PieceRank: PieceRank.Construct }) continue;

                _storedPos.Add(p.Pos);
                ActionManager.EnqueueAction(new KillPiece(null, p));
            }

            AddEffectObserver(new ChaoticConstructorSpawn(-1, _storedPos));
        }
    }
}