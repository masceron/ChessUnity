using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Commons
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArcticBrittleStar : PieceLogic, IPieceWithSkill
    {
        public ArcticBrittleStar(PieceConfig cfg) : base(cfg, KingMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 15, this)));
            Skills = list =>
            {
                if (SkillCooldown > 0) return;
                var (rank, file) = RankFileOf(Pos);

                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                {
                    var index = IndexOf(rankOff, fileOff);
                    list.Add(new ArcticBrittleStarActive(Pos, index));
                }
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
    
}