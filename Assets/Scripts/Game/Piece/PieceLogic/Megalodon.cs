using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
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
        private readonly System.Func<int> getCount;
        private readonly System.Action<int> setCount;
        private readonly System.Func<List<int>> getTargeted;
        private readonly List<int> targeted = new List<int>();
        private int count;
        private List<Commons.PieceLogic> availablePieces;
        public Megalodon(PieceConfig cfg) : base(cfg, RookMoves.Quiets,
            MegalodonMoves.Captures)
        {
            count = 2;
            getCount = () => count;
            setCount = (v) => count = v;
            getTargeted = () => targeted;
            ActionManager.ExecuteImmediately(new ApplyEffect(new FrenziedVeteran(this)));

            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                if (!CanActive()) return;
                foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), AttackRange))
                {
                    var idx = IndexOf(rank, file);
                    var pOn = PieceOn(idx);
                    if (pOn != null && pOn.Color != Color)
                    {
                        if (targeted.Contains(idx)) continue;
                        list.Add(new MegalodonActive(Pos, idx, count, getCount, setCount, getTargeted));
                    }
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
                if (targeted.Contains(idx)) continue;

                if (pOn.Color == Color) hasAlly = true;
                else hasEnemy = false;
                
                if (hasAlly && hasEnemy) 
                    return true;
            }

            return false;
        }
    }
    
}