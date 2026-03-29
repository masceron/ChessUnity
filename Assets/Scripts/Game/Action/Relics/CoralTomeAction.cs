using Game.Action.Internal;
using Game.Piece;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class CoralTomeAction : Action, IRelicAction
    {
        [MemoryPackInclude] private string _pieceType;

        [MemoryPackInclude] private bool _relicColor;

        [MemoryPackConstructor]
        private CoralTomeAction()
        {
        }

        public CoralTomeAction(bool relicColor, string pieceType, int maker) : base(null, maker)
        {
            _relicColor = relicColor;
            _pieceType = pieceType;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(_pieceType, _relicColor, GetTargetPos())));
        }
    }
}