using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Effects.SpecialAbility;
using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AbbottsMoray : Commons.PieceLogic
    {
        public AbbottsMoray(PieceConfig cfg) : base(cfg, EmptyDiamondMove.Quiets, EmptyDiamondMove.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Ambush(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 10, this)));   
            ActionManager.ExecuteImmediately(new ApplyEffect(new AbbottsMorayPassive(this)));
        }
    }
}