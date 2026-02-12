using Game.Action;
using Game.Action.Captures;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Blinded: Effect, IBeforePieceActionEffect
    {
        public Blinded(int duration, int probability, PieceLogic piece) : base(duration, probability, piece, "effect_blinded")
        {
        }
        
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Maker != Piece.Pos) return;
            
            if (MatchManager.Roll(Strength))
            {
                action.Result = ResultFlag.Miss;
            }
        }
    }
}