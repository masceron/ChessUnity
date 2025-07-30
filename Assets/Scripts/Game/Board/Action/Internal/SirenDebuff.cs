using Game.Board.Effects.Debuffs;
using static Game.Board.General.MatchManager;

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
            var affected = gameState.PieceBoard[To];
            
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, affected)));
        }
    }
}