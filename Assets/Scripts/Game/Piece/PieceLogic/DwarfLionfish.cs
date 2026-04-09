using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DwarfLionfish : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 1;
        public DwarfLionfish(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, KingMoves.Captures)
        {
            SetStat(SkillStat.Range, Range);
            ActionManager.ExecuteImmediately(new ApplyEffect(new DwarfLionfishPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    list.Add(new DwarfLionfishActive(this));
                }
                else
                {
                    //query for AI in here
                    var (rank, file) = RankFileOf(Pos);
                    var listA = GetPiecesInRadius(rank, file, GetStat(SkillStat.Range), p =>
                        p != null && p.Color != Color &&
                        (p.Effects == null || !p.Effects.Any(e => e.EffectName == "effect_extremophile")));

                    if (listA.Count >= 1)
                    {
                        if (!excludeEmptyTile)
                            list.Add(new DwarfLionfishActive(this));
                        else
                            list.Add(new DwarfLionfishActive(this));
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}