using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Effects.Others;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic
{
    public class BlueDragon : PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;
        public BlueDragon(PieceConfig cfg) : base(cfg, SpinningMoves.Quiets, SpinningMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Silenced(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new BlueDragonPassive(this)));
            
            Skills = list =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);

                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var pOn = PieceOn(index);
                    if (pOn == null || pOn == this) continue;
                    if (pOn.Color != Color)
                    {
                        list.Add(new BlueDragonActive(Pos, index));
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