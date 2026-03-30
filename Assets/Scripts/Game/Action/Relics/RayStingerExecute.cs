using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class RayStingerExecute : Action, IRelicAction
    {
        private const int BleedingStack = 3;
        private const int BrokenDuration = 2;

        [MemoryPackConstructor]
        private RayStingerExecute()
        {
        }

        public RayStingerExecute(int target) : base(null, target)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(BleedingStack, GetTarget() as PieceLogic)));
            ActionManager.EnqueueAction(new ApplyEffect(new Broken(BrokenDuration, GetTarget() as PieceLogic)));
        }
    }
}