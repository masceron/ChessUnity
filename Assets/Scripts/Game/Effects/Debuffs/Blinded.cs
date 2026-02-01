using Game.Action;
using Game.Action.Captures;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Blinded: Effect, IBeforePieceActionEffect
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly int Probability;

        public Blinded(sbyte duration, int probability, PieceLogic piece) : base(duration, 1, piece, "effect_blinded")
        {
            Probability = probability;
        }
        
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Maker != Piece.Pos) return;
            
            if (MatchManager.Roll(Probability))
            {
                action.Result = ResultFlag.Miss;
            }
        }
    }
}