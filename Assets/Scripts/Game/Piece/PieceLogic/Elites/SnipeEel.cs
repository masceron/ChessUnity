using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnipeEel : PieceLogic, IPieceWithSkill
    {
        public SnipeEel(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnipeEelPassive(this)));
            Skills = list =>
            {
                if (SkillCooldown == 0) {
                    var (rank, file) = RankFileOf(Pos);
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 5))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        list.Add(new SnipeEelActive(Pos, index));
                    }
                }
            };
        }
        
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}   