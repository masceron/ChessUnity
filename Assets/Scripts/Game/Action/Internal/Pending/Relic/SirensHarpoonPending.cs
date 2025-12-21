using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class SirensHarpoonPending : Action, IPendingAble, System.IDisposable, IRelicAction
    {
        private SirensHarpoon _sirensHarpoon;

        public SirensHarpoonPending(SirensHarpoon s, int maker, bool pos = false) : base(maker, pos)
        {
            _sirensHarpoon = s;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            ActivateRelic(Maker);
        }


        public void Dispose()
        {
            _sirensHarpoon = null;
            BoardViewer.SelectingFunction = 0;
        }


        public void ActivateRelic(int maker)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Controlled(1, BoardUtils.PieceOn(maker))));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Pacified(1, BoardUtils.PieceOn(maker))));

            _sirensHarpoon.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        protected override void ModifyGameState()
        {

        }

        // public void CompleteActionForAI()
        // {
        //     //Implement for AI automatically
        //     var listPieces = new List<PieceLogic>();

        //     for (var i = 0; i < BoardUtils.BoardSize; ++i)
        //     {
        //         var piece = BoardUtils.PieceOn(i);
        //         if (piece == null || piece.Color == _sirensHarpoon.Color) continue;

        //         if (piece.Effects.Any(e => (
        //                 e.EffectName == "effect_extremophile" || e.EffectName == "effect_sanity"))
        //            ) continue;

        //         listPieces.Add(piece);
        //     }

        //     if (listPieces.Count == 0) return;

        //     if (listPieces.Count == 1)
        //     {
        //         ActivateRelic(listPieces[0].Pos);
        //         return;
        //     }
            
        //     listPieces.Sort((a, b) => 
        //         b.GetValueForAI().CompareTo(a.GetValueForAI())
        //     );
            
        //     int topValue = listPieces[0].GetValueForAI();
        //     var topGroup = listPieces.Where(p => p.GetValueForAI() == topValue).ToList();
            
        //     if (topGroup.Count == 1)
        //     {
        //         ActivateRelic(topGroup[0].Pos);
        //     }
        //     else
        //     {
        //         int idx = UnityEngine.Random.Range(0, topGroup.Count);
        //         ActivateRelic(topGroup[idx].Pos);
        //     }
        // }
    }
}
