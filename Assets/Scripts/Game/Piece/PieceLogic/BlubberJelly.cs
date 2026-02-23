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

        public BlubberJelly(PieceConfig cfg) : base(cfg, TentacleMoves.Quiets, TentacleMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Haste(-1, 1, this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (pOn != null) continue;
                        list.Add(new BlubberJellyActive(Pos, index));
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