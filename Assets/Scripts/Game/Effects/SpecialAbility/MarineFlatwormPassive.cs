using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Tile;

namespace Game.Effects.SpecialAbility
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineFlatwormPassive : Effect, IDeadTrigger
    {
        public MarineFlatwormPassive(PieceLogic piece) : base(-1, 1, piece, "effect_marine_flatworm_passive")
        {
        }
        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie == Piece) {
                return;
            }
            // quan dong minh
            if (pieceToDie.Color == Piece.Color && pieceToDie.Effects.Any(e => e.EffectName == "effect_illusion")) {
                if (Distance(Piece.Pos, pieceToDie.Pos) <= 4) {
                    SetFormation(pieceToDie.Pos, new SiltCloud(Piece.Color));
                }

            }
        }

    }
}