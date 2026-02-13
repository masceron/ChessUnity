using MemoryPack;
using Game.Action.Internal;
using static Game.Common.BoardUtils;
using System.Collections.Generic;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class ChaoticConstructorAction : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private ChaoticConstructorAction() { }

        [MemoryPackInclude] private List<int> _storedPos = new();

        public ChaoticConstructorAction(int maker) : base(maker)
        {
            Target = maker;
            Maker = maker;
        }

        protected override void ModifyGameState()
        {
            _storedPos.Clear();
            for (var i = 0; i < BoardSize; ++i)
            {
                var p = PieceOn(i);
                if (p is not { PieceRank: Piece.PieceRank.Construct }) continue;

                _storedPos.Add(p.Pos);
                ActionManager.EnqueueAction(new KillPiece(p.Pos));
            }

            AddEffectObserver(new Effects.Others.ChaoticConstructorSpawn(-1, _storedPos));
        }
    }
}