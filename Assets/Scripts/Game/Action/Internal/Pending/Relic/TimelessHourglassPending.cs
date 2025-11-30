using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class TimelessHourglassPending : Action, System.IDisposable, IPendingAble
    {
        private TimelessHourglass _timelessHourglass;
        public TimelessHourglassPending(TimelessHourglass t, int maker, bool pos = false) : base(maker, pos)
        {
            _timelessHourglass = t;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            if(BoardUtils.PieceOn(Target).Color == _timelessHourglass.Color)
            {
                BoardUtils.PieceOn(Target).SkillCooldown -= 2;
            }
            else
            {
                BoardUtils.PieceOn(Target).SkillCooldown += 2;
            }

            TileManager.Ins.UnmarkAll();


            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            _timelessHourglass.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        protected override void ModifyGameState()
        {
        }
        
        public void Dispose()
        {
            _timelessHourglass = null;
            BoardViewer.SelectingFunction = 0;
        }

        public void CompleteActionForAI()
        {
            if (_timelessHourglass == null) return;

            // Gather all pieces
            var pieces = MatchManager.Ins.GameState.PieceBoard
                .Where(p => p != null)
                .ToList();

            if (pieces.Count == 0) return;

            bool relicColor = _timelessHourglass.Color;

            // Candidates: allies with cooldown >= 2, enemies with cooldown == 1
            var candidates = new List<PieceLogic>();
            foreach (var p in pieces)
            {
                if (p.Color == relicColor)
                {
                    if (p.SkillCooldown >= 2) candidates.Add(p);
                }
                else
                {
                    if (p.SkillCooldown == 1) candidates.Add(p);
                }
            }

            if (candidates.Count == 0) return;
            
            var bestScore = candidates.Max((p) => p.GetValueForAI());
            var top = candidates.Where(p => p.GetValueForAI() == bestScore).ToList();
            var chosen = top.Count == 1 ? top[0] : top[Random.Range(0, top.Count)];

            Target = (ushort)chosen.Pos;
            ActionManager.DoManualAction(this);
        }
    }
}
