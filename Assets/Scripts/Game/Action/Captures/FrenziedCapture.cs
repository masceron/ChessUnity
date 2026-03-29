using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class FrenziedCapture : Action, IDontEndTurn, ICaptures
    {
        [MemoryPackConstructor]
        private FrenziedCapture()
        {
        }

        public FrenziedCapture(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Complete FrenziedCapture");
            PieceManager.Ins.Destroy(GetTargetPos());
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
            MatchManager.Ins.GameState.Kill(GetTarget() as PieceLogic);
            MatchManager.Ins.GameState.Move(GetMaker() as PieceLogic, GetTargetPos());
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}