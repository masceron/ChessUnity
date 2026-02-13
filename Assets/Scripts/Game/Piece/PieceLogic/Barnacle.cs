using Game.Action.Skills;
using Game.Managers;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Barnacle : Commons.PieceLogic, IPieceWithSkill
    {
        public Barnacle(PieceConfig cfg) : base(cfg, ShellfishMoves.Quiets, RookMoves.Captures)
        {

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    var (_, _) = RankFileOf(Pos);
                    foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                    {
                        if (piece == null) continue;
                        if (piece.Color == Color) continue;

                        var hasShield = PieceOn(piece.Pos).Effects.Any(effect => effect.EffectName is "effect_shield" or "effect_hardened_shield");

                        if (hasShield)
                        {
                            list.Add(new BarnacleActive(Pos, piece.Pos));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}