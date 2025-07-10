using Core.General;

namespace Board.Action
{
    public class SirenDebuff: Action
    {
        public SirenDebuff(ushort p, ushort f, ushort t) : base(p, false, false)
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
            state.MainBoard[To].Effects.Add(new Effect(EffectType.Slow, 1, 1));
            state.LastMove = this;
        }
    }
}