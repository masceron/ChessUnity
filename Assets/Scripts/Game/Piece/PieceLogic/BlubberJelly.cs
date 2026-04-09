using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class BlubberJelly : Commons.PieceLogic, IPieceWithSkill
    {
        private int timeToCooldown;
        private const int Strength = 1;
        private const int Cooldown = 5;
        private const int Range1 = 2;
        private const int Range2 = 1;
        private const int Duration1 = -1;
        private const int Duration2 = 1;

        public BlubberJelly(PieceConfig cfg) : base(cfg, TentacleMoves.Quiets, TentacleMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Haste(Duration1, Strength, this)));

            SetStat(SkillStat.Cooldown, Cooldown);
            SetStat(SkillStat.Range, Range1);
            SetStat(SkillStat.Range, Range2, 2);
            SetStat(SkillStat.Duration, Duration2, 2);
            
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, GetStat(SkillStat.Range, 1)))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (pOn != null) continue;
                        list.Add(new BlubberJellyActive(this, index));
                    }
                }
                else
                {
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