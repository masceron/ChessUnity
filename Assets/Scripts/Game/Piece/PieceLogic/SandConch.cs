using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Effects.States;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class SandConch : Commons.PieceLogic, IPieceWithSkill
    {
        private int timeToCooldown;
        private const int SkillRadius = 2;
        private const string BurrowedEffectName = "effect_burrowed";

        public SandConch(PieceConfig cfg) : base(cfg, ShellfishMoves.Quiets, KingMoves.Captures)
        {
            SetStat(SkillStat.Radius, SkillRadius);
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    var burrowed = Effects.Any(e => e.EffectName == BurrowedEffectName);
                    if (!burrowed)
                    {
                        list.Add(new SandConchActiveBurrowed(Pos));
                        return;
                    }
                    
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, GetStat(SkillStat.Radius)))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (!IsActive(index) || pOn != null) continue;
                        list.Add(new SandConchActiveMoveAndFormate(Pos, index));
                    }
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {

                    }
                    else
                    {

                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}