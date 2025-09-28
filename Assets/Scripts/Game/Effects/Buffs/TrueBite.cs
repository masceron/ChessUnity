using Game.Action;
using Game.Piece.PieceLogic;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TrueBite: Effect
    {
        public TrueBite(PieceLogic piece) : base(-1, -1, piece, EffectName.TrueBite)
        {}

        public override void OnCall(Action.Action action)
        {
            
        }
    }
}