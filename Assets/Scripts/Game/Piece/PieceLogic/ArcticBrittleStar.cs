using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArcticBrittleStar : Commons.PieceLogic, IPieceWithSkill
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