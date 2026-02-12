using MemoryPack;
using Game.Action.Internal;
using Game.Piece;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class CoralTomeAction : Action, IRelicAction
    {
        [MemoryPackInclude]
        private readonly string _pieceType;
        [MemoryPackInclude]
        private readonly bool _relicColor;
        
        public CoralTomeAction(bool relicColor, string pieceType, int maker) : base(maker)
        {
            _relicColor = relicColor;
            _pieceType = pieceType;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new SpawnPiece(new PieceConfig(_pieceType, _relicColor , (ushort)Maker)));
        }
    }
}