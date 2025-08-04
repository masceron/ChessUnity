using Game.Board.Effects.Debuffs;
using Game.Common;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuff: Action, IInternal
    {
        public SirenDebuff(ushort p, ushort f, ushort t) : base(p)
        {
            Maker = f;
            Target = t;
        }

        protected override void ModifyGameState()
        {
            var affected = BoardUtils.PieceOn(Target);
            
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, affected)));
        }
    }
}