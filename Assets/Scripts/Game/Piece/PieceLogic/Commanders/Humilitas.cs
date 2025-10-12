using Game.Movesets;
using Game.Action.Skills;
using Game.Action;
using Game.Common;
using static Game.Common.BoardUtils;
using Game.Effects.Others;
using Game.Action.Internal;
using Game.Effects.Traits;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Humilitas: PieceLogic, IPieceWithSkill
    {
        private int deathDefianceCount = 4 ;
        private readonly System.Func<int> getCount;
        private readonly System.Action<int> setCount;
        private int count;
        public Humilitas(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            deathDefianceCount = 4;
            count = 2;
            getCount = () => count;
            setCount = (v) => count = v;
            ActionManager.EnqueueAction(new ApplyEffect(new PureMinded(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new Relentless(this, deathDefianceCount)));
            ActionManager.EnqueueAction(new ApplyEffect(new DeathDefiance(this)));

            // active này sẽ chọn 1 hoặc 2 nhưng khi chọn vào thì sẽ bị hiệu ứng luôn những con đã chọn
            // chứ không có chọn 1 con rồi tắt đi và chọn lại 2 con

            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 5))
                {
                    var idx = IndexOf(rank, file);
                    var pOn = PieceOn(idx);
                    if (pOn != null && pOn.Color != Color)
                    {
                        list.Add(new HumilitasActive(Pos, idx, count, getCount, setCount));
                    }
                }
            };
        }


        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
