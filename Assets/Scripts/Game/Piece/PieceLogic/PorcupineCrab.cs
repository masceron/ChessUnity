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
    public class PorcupineCrab : Commons.PieceLogic, IPieceWithSkill
    {
        private int timeToCooldown;
        private const int Cooldown = 6;
        private const int Range = 3;

        public PorcupineCrab(PieceConfig cfg) : base(cfg, ShellfishMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Carapace(-1,this)));
            
            SetStat(SkillStat.Cooldown, Cooldown);
            SetStat(SkillStat.Range, 3);
            
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, GetStat(SkillStat.Range)))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (pOn != null && pOn.Color != Color) continue;          
                        list.Add(new PorcupineCrabActive(this, index));
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