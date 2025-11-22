using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Others;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Megalodon: Commons.PieceLogic, IPieceWithSkill
    {
        public Megalodon(PieceConfig cfg) : base(cfg, RookMoves.Quiets,
            MegalodonMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new FrenziedVeteran(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new TrueBite(this)));

            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                if (!CanActive()) return;
                foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), AttackRange))
                {
                    var idx = IndexOf(rank, file);
                    var pOn = PieceOn(idx);
                    if (pOn == null) continue;
                    
                    list.Add(new MegalodonActive(Pos, idx, Color));
                    
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        
        private bool CanActive()
        {
            bool hasAlly = false;
            bool hasEnemy = false;
            foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), AttackRange))
            {
                var idx = IndexOf(rank, file);
                var pOn = PieceOn(idx);
                
                if (pOn == null) continue;

                if (pOn.Color == Color) 
                    hasAlly = true;
                else 
                    hasEnemy = true;
                
                if (hasAlly && hasEnemy) 
                    return true;
            }

            return false;
        }
    }
    
}