using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineFlatwormPassive : Effect, IDeadTrigger
    {
        public MarineFlatwormPassive(PieceLogic piece, int range) : base(-1, 1, piece, "effect_marine_flatworm_passive")
        {
            SetStat(EffectStat.Range, range);
        }

        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie == Piece) return;
            // quan dong minh
            if (pieceToDie.Color == Piece.Color && pieceToDie.Effects.Any(e => e.EffectName == "effect_illusion"))
                if (Distance(Piece.Pos, pieceToDie.Pos) <= GetStat(EffectStat.Range))
                    SetFormation(pieceToDie.Pos, new SiltCloud(Piece.Color));
        }
    }
}