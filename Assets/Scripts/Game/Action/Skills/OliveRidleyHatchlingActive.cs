using System;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class OliveRidleyHatchlingActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private OliveRidleyHatchlingActive()
        {
        }

        public OliveRidleyHatchlingActive(int maker) : base(maker)
        {
            Maker = maker;
            Target = maker;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(Maker));
            ActionManager.EnqueueAction(new SpawnPiece(new Piece.PieceConfig("piece_archelon", PieceOn(Maker).Color, Maker)));
            SetCooldown(Maker, -1);
        }
    }
}