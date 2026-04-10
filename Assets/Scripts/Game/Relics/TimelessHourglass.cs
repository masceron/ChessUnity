using System.Collections.Generic;
using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;
using ZLinq;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TimelessHourglass : RelicLogic
    {
        public TimelessHourglass(RelicConfig config) : base(config)
        {
            Type = config.Type;
            Color = config.Color;
            TimeCooldown = config.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate(List<Action.Action> actions)
        {
            throw new System.NotImplementedException();
        }

        public override void ActiveForAI()
        {
            // Gather all pieces
            var pieces = BoardUtils.PieceBoard()
                .Where(pieceLogic => pieceLogic != null)
                .ToList();

            if (pieces.Count == 0) return;

            var relicColor = Color;

            // Candidates: allies with cooldown >= 2, enemies with cooldown == 1
            var candidates = new List<PieceLogic>();
            foreach (var piece in pieces)
                if (piece.Color == relicColor)
                {
                    if (piece.SkillCooldown >= 2) candidates.Add(piece);
                }
                else
                {
                    if (piece.SkillCooldown == 1) candidates.Add(piece);
                }

            if (candidates.Count == 0) return;

            var bestScore = candidates.Max(pieceLogic => pieceLogic.GetValueForAI());
            var top = candidates.Where(pieceLogic => pieceLogic.GetValueForAI() == bestScore).ToList();
            var chosen = top.Count == 1 ? top[0] : top[Random.Range(0, top.Count)];

            var excute = new TimelessHourglassExecute(Color, chosen.Pos);
            // BoardViewer.Ins.ExecuteAction(excute);

            // var pending = new TimelessHourglassPending(this, chosen.Pos);
            // if (pending is PendingAction p)
            // {
            //     BoardViewer.Ins.ExecuteAction(await p.WaitForCompletion());
            // }
        }
    }
}