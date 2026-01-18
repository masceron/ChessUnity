    using System.Linq;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UnityEngine;
using UX.UI.Ingame;
using Game.Action.Relics;


namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OrnetesEdictPending : Action, System.IDisposable, IRelicAction
    {
        private OrnetesEdict ornetesEdict;
        
        public OrnetesEdictPending(OrnetesEdict cp, int maker, bool pos = false) : base(maker)
        {
            ornetesEdict = cp;
            Target = (ushort)maker;
        }

        public void Dispose()
        {
            ornetesEdict = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
            int numberOfDebuffedPieces = BoardUtils.PieceOn(Target).Effects.Count(e => e.Category == EffectCategory.Debuff);
            int rate = 7 * (numberOfDebuffedPieces);
            if (MatchManager.Roll(rate)) 
            {
                ActionManager.EnqueueAction(new KillPiece(Target));
            }

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            ornetesEdict.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
    }
}