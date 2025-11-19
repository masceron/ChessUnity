using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;

namespace Game.Piece.PieceLogic
{
    /// <summary>
    /// Contagion Corpse Construct
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ContagionCorpse : Commons.PieceLogic
    {
        public ContagionCorpse(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ContagionCorpsePassive(this)));
        }
    }
}

