using Game.Board.Effects;
using Game.Board.General;
using UnityEngine;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuff: Action
    {
        public SirenDebuff(ushort p, ushort f, ushort t) : base(p, false, false, true)
        {
            From = f;
            To = t;
        }

        public override void ApplyAction(GameState state)
        {
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            var affected = state.MainBoard[To];
            
            ActionManager.Execute(new ApplyEffect(new Slow(1, 1, affected)));
        }
    }
}