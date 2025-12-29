using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Effects.Augmentation;
using Game.Effects.Buffs;
using Game.Action.Internal;
using Game.Action;
using System.Linq;
using Game.Common;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SohalSurgeonfish : Commons.PieceLogic, IPieceWithSkill
    {
        public SohalSurgeonfish(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 6))
                    {
                        var idx = IndexOf(rank, file);
                        var pOn = PieceOn(idx);
                        if (pOn != null && pOn.Color != PieceOn(Pos).Color && pOn.Effects.Any(e => e.EffectName == "effect_slow"))
                        {
                            list.Add(new SohalSurgeonfishActive(Pos, idx));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
