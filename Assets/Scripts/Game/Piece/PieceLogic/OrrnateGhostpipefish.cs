using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class OrnateGhostpipefish : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 7;
        private const int Duration = 3;
        public OrnateGhostpipefish(PieceConfig cfg) : base(cfg, RangerMove.Quiets, None.Captures)
        {
            SetStat(SkillStat.Range, Range);
            SetStat(SkillStat.Duration, Duration);

            ActionManager.ExecuteImmediately(new ApplyEffect(new QuickReflex(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Rally(-1, this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                
                var listPieces = SkillRangeHelper.GetActiveCellInRadius(Pos, GetStat(SkillStat.Range));
                if (!listPieces.Contains(Pos)) listPieces.Add(Pos);
                foreach (var piece in listPieces)
                {
                    var p = PieceOn(piece);
                    if (p == null) continue;
                    
                    list.Add(new OrnateGhostpipefishActive(this, p, GetStat(SkillStat.Duration)));
                }
            };
        }

        public SkillsDelegate Skills { get; set; }
        int IPieceWithSkill.TimeToCooldown { get; set; }
    }
}