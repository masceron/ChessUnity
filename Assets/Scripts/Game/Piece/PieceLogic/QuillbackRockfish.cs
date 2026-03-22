using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;
using Game.Action.Skills;
using Game.Effects.Others;
using Game.Movesets;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class QuillbackRockfish : Commons.PieceLogic, IPieceWithSkill
    {
        public QuillbackRockfish(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            SetStat(SkillStat.Stack, 4);
            ActionManager.ExecuteImmediately(new ApplyEffect(new Carapace(-1, this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (!isPlayer) return;
                list.Add(new QuillbackRockfishActive(Pos));
            };
        
        }
        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }

}