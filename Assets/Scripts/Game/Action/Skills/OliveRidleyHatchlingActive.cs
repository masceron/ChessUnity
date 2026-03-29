using System;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
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
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetMakerPos()));
            ActionManager.EnqueueAction(
                new SpawnPiece(new Piece.PieceConfig("piece_archelon", GetMaker() as PieceLogic.Color, GetMakerPos())));
            SetCooldown(GetMaker() as PieceLogic, -1);
        }
    }
}