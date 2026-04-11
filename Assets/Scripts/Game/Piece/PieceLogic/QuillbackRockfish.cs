using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using Game.Movesets;
using Game.Effects.Buffs;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class QuillbackRockfish : Commons.PieceLogic, IPieceWithSkill
    {
        private int duration = -1;
        public QuillbackRockfish(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            SetStat(SkillStat.Stack, 4);
            ActionManager.ExecuteImmediately(new ApplyEffect(new Carapace(duration, this)));
            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown != 0) return;
                if (!isPlayer) return;

                var (rank, file) = RankFileOf(Pos);
                var push = Color ? 1 : -1;
                var behindRank = rank - push;
                if (!VerifyBounds(behindRank)) return;
                var behindIndex = IndexOf(behindRank, file);
                if (!VerifyIndex(behindIndex) || !IsActive(behindIndex)) return;
                var enemy = PieceOn(behindIndex);
                if (enemy != null && enemy.Color != Color)
                    list.Add(new QuillbackRockfishActive(this, enemy));
            };
        
        }
        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }

}