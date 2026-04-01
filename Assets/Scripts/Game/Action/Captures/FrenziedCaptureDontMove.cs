using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class FrenziedCaptureDontMove : Action, IDontEndTurn
    {
        [MemoryPackConstructor]
        private FrenziedCaptureDontMove()
        {
        }

        public FrenziedCaptureDontMove(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Complete FrenziedCaptureDontMove");
            PieceManager.Ins.Destroy(GetTargetPos());
            MatchManager.Ins.GameState.Kill(GetTargetAsPiece());
        }
    }
}