using Game.Board.Effects.Debuffs;
using Game.Board.General;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuff: Action, IInternal
    {
        public SirenDebuff(ushort p, ushort f, ushort t) : base(p, false)
        {
            From = f;
            To = t;
        }

        protected override void ModifyGameState()
        {
            var affected = MatchManager.Ins.GameState.PieceBoard[To];
            
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, affected)));
        }
    }
}