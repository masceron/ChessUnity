using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.SpecialAbility;
using Game.Effects.States;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class PennantCoralfish : Commons.PieceLogic
    {
        private const int Strength = 1;
        private const int Duration = 2;
        public PennantCoralfish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new PennantCoralfishPassive(this, Strength, Duration)));
        }
    }
}