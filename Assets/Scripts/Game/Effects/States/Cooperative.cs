using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Cooperative</b><br/>
    ///     Người chơi không thể điều khiển quân này trực tiếp.<br/>
    ///     Mỗi đầu turn (<see cref="IStartTurnTrigger"/>): quân tự thực hiện random 1 trong 3
    ///     action (Move, Capture, Cast Skill). Các action này <b>không kết thúc turn</b>.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CooperativeState : StateEffect, IStartTurnTrigger
    {
        private readonly Random _random = new();

        public override StateType StateType => StateType.Cooperative;

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Other;
        public StartTurnEffectType StartTurnEffectType => StartTurnEffectType.StartOfAllyTurn;

        public CooperativeState(PieceLogic piece) : base(-1, 0, piece, "effect_cooperative")
        {
        }

        public void OnCallStart(Action.Action lastMainAction)
        {
            if (!BoardUtils.IsAlive(Piece)) return;

            var moveList = new List<Game.Action.Action>();
            var skills = new List<Game.Action.Action>();
            Piece.Quiets(moveList, Piece.Pos, true);
            Piece.Captures(moveList, Piece.Pos, true);
            if (Piece is IPieceWithSkill pieceWithSkill)
            {
                pieceWithSkill.Skills(skills, false, true);
                moveList.AddRange(skills);
            }

            if (moveList.Count == 0) return;

            var chosen = moveList[_random.Next(0, moveList.Count)];

            if (chosen == null) return;

            // Enqueue action tương ứng với IDontEndTurn
            if (chosen is ICaptures)
                ActionManager.EnqueueAction(new CaptureWithoutEndTurn(Piece.Pos, chosen.Target));
            else if (chosen is IQuiets)
                ActionManager.EnqueueAction(new MoveWithoutEndturn(Piece.Pos, chosen.Target));
            // else if (chosen is ISkills)
            //     ActionManager.EnqueueAction(new SkillWithoutEndTurn(Piece.Pos, chosen.Target));
        }
    }
}
