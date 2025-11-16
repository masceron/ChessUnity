using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Condition;
using Game.Effects.Others;
using Game.Movesets;
using Game.Common;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic.Elites
{
    public class EpauletteShark : PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;
        public EpauletteShark(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new EpauletteSharkPurify(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new DiurnalAmbush(this)));
            Skills = list =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);

                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var pOn = PieceOn(index);
                    if (pOn == null || pOn == this || pOn.PieceRank != PieceRank.Swarm) continue;
                    if (pOn.Color != Color)
                    {
                        list.Add(new EpauletteSharkActive(Pos, index));
                    }
                }
            };
        }
        
        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}