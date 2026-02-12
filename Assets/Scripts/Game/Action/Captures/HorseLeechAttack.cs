using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    [MemoryPackable]
    public partial class HorseLeechAttack: Action, ICaptures
    {
        public HorseLeechAttack(int maker, int target) : base(maker)
        {
            Target = target;
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(4, PieceOn(Target)), PieceOn(Maker)));
            ActionManager.EnqueueAction(new KillPiece(Maker));
        }

    }
}