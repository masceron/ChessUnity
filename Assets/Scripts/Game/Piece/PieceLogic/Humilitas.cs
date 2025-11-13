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
    public class Humilitas: PieceLogic, IPieceWithSkill
    {
        private int deathDefianceCount = 4 ;
        private readonly System.Func<int> getCount;
        private readonly System.Action<int> setCount;
        private readonly System.Func<List<int>> getTargeted;
        private readonly List<int> targeted = new List<int>();
        private int count;
        public Humilitas(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            deathDefianceCount = 4;
            count = 2;
            getCount = () => count;
            setCount = (v) => count = v;
            getTargeted = () => targeted;
            ActionManager.EnqueueAction(new ApplyEffect(new PureMinded(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new Relentless(this, deathDefianceCount)));
            ActionManager.EnqueueAction(new ApplyEffect(new DeathDefiance(this, deathDefianceCount)));

            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 5))
                {
                    var idx = IndexOf(rank, file);
                    var pOn = PieceOn(idx);
                    if (pOn != null && pOn.Color != Color)
                    {
                        if (targeted.Contains(idx)) continue;
                        list.Add(new HumilitasActive(Pos, idx, count, getCount, setCount, getTargeted));
                    }
                }
            };
        }


        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
