using MemoryPack;
using Game.Action.Internal;
using Game.Piece;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class CoralTomeAction : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private CoralTomeAction() { }

        [MemoryPackInclude]
        private string _pieceType;
        [MemoryPackInclude]
        private bool _relicColor;
        
        public CoralTomeAction(bool relicColor, string pieceType, int maker) : base(maker)
        {
            _relicColor = relicColor;
            _pieceType = pieceType;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(_pieceType, _relicColor , Maker)));
        }
    }
}