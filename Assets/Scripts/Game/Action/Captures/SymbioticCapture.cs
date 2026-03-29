using Game.Action.Internal;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SymbioticCapture : Action, ICaptures
    {
        [MemoryPackConstructor]
        private SymbioticCapture()
        {
        }

        public SymbioticCapture(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            var makerPiece = GetMaker();
            var targetPiece = GetTarget();

            if (makerPiece == null || targetPiece == null) return;

            // Chỉ nối được những quân ở State: None
            if (targetPiece.CurrentState == StateType.None)
            {
                // Chiều dài tối đa của dây nối là ((Moverange gốc + 1) * 2)
                int maxRange = (makerPiece.MoveRange() + 1) * 2;
                
                // Set biến IsTethered bên Symbiotic
                var symbioticEffect = makerPiece.Effects.Find(e => e is Symbiotic) as Symbiotic;
                if (symbioticEffect != null) symbioticEffect.IsTethered = true;

                // Khi nối sẽ chuyển State: None của quân được nối sang State: Tethered
                ActionManager.EnqueueAction(new ApplyEffect(new Tethered(targetPiece, makerPiece, maxRange), makerPiece));
            }
        }
    }
}
