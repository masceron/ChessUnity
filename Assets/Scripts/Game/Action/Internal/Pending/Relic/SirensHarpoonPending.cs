using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class SirensHarpoonPending : Action, IPendingAble, System.IDisposable
    {
        private SirensHarpoon _sirensHarpoon;

        public SirensHarpoonPending(SirensHarpoon s, int maker, bool pos = false) : base(maker, pos)
        {
            _sirensHarpoon = s;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            ActivateRelic();
        }


        public void Dispose()
        {
            _sirensHarpoon = null;
            BoardViewer.SelectingFunction = 0;
        }


        public void ActivateRelic()
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Controlled(1, BoardUtils.PieceOn(Maker))));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Pacified(BoardUtils.PieceOn(Maker))));

            _sirensHarpoon.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        protected override void ModifyGameState()
        {

        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
            var listPieces = new List<PieceLogic>();

            for (var i = 0; i < BoardUtils.BoardSize; ++i)
            {
                var piece = BoardUtils.PieceOn(i);
                if (piece == null || piece.Color == _sirensHarpoon.Color) continue;

                if (piece.Effects.Any(e => (
                        e.EffectName == "effect_extremophile" || e.EffectName == "effect_sanity"))
                   ) continue;

                listPieces.Add(piece);
            }

            if (listPieces.Count == 0) return;

            if (listPieces.Count == 1)
            {

            }
        }
    }
}
