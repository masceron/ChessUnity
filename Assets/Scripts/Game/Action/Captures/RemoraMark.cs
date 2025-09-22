using Game.Action.Internal;
using Game.Effects.Others;
using static Game.Common.BoardUtils;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RemoraMark: Action, ICaptures
    {
        public RemoraMark(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new RemoraMarked(PieceOn(Maker), PieceOn(Target))));
        }
    }
}