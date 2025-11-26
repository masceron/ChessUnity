using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguana: Commons.PieceLogic, IPieceWithSkill
    {
        public MarineIguana(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Extremophile(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new FreeMovement(this)));
            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                var caller = PieceOn(Pos);
                var push = caller.Color ? +1 : -1;
                var (rank, file) = RankFileOf(Pos);

                MakeSkill(list, IndexOf(rank + push * 1, file));

                MakeSkill(list, IndexOf(rank, file - 1));
                MakeSkill(list, IndexOf(rank, file + 1));

                MakeSkill(list, IndexOf(rank - push * 1, file - 1));
                MakeSkill(list, IndexOf(rank - push * 1, file));
                MakeSkill(list, IndexOf(rank - push * 1, file + 1));

                MakeSkill(list, IndexOf(rank - push * 2, file - 2));
                MakeSkill(list, IndexOf(rank - push * 2, file - 1));
                MakeSkill(list, IndexOf(rank - push * 2, file));
                MakeSkill(list, IndexOf(rank - push * 2, file + 1));
                MakeSkill(list, IndexOf(rank - push * 2, file + 2));
            };
        }


        private void MakeSkill(List<Action.Action> list, int index)
        {

            var piece = PieceOn(index);
            if (piece == null || piece.Color == Color) return;
            list.Add(new MarineIguanaAttack(Pos, index));
            
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}