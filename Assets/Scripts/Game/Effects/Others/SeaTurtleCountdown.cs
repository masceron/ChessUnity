using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;


namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtleCountdown: Effect, IEndTurnEffect
    {
        private int Pos;
        public EndTurnEffectType EndTurnEffectType { get; }
        public SeaTurtleCountdown(sbyte Duration, PieceLogic piece) : base(Duration, 1, piece, "effect_sea_turtle_countdown")
        {Pos = piece.Pos;}
        public void OnCallEnd(Action.Action action)
        {
            if (action.Maker != Pos) ActionManager.EnqueueAction(new RemoveEffect(this));
        }
        public override void OnRemove()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(1, Piece)));
        }
    }
}