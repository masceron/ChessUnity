using System.Linq;
using Game.Action.Skills;
using Game.Effects;
using Game.Managers;
using Game.Movesets;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Summon
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Barnacle: PieceLogic, IPieceWithSkill
    {
        public Barnacle(PieceConfig cfg) : base(cfg, ShellfishMoves.Quiets, RookMoves.Captures)
        {

            Skills = list =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;
                    if (piece.Color == Color) continue;

                    bool hasShield = false;
                    foreach (var effect in PieceOn(piece.Pos).Effects
                                 .Where(effect => (effect.EffectName == EffectName.Shield
                                                   || effect.EffectName == EffectName.HardenedShield)))
                    {
                        hasShield = true;
                        break;
                    }

                    if (hasShield)
                    {
                        list.Add(new BarnacleActive(Pos, piece.Pos));
                    }
                }


            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}