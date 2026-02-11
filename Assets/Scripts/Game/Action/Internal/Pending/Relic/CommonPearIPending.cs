using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CommonPearlPending : PendingAction, System.IDisposable, IRelicAction
    {
        private CommonPearl _commonPearl;
        
        public CommonPearlPending(CommonPearl cp, int maker) : base(maker)
        {
            _commonPearl = cp;
            Target = (ushort)maker;
            Maker = (ushort)maker;
        }

        protected override void CompleteAction()
        {
            _commonPearl.SetCooldown();
            var execute = new CommonPearlExecute(Target);
            CommitResult(execute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            _commonPearl = null;
            BoardViewer.SelectingFunction = 0;
        }

        // protected override void ModifyGameState()
        // {
        //     ActionManager.EnqueueAction(new ApplyEffect(GetRandomBuffEffect(PieceOn(Target)), commonPearl));

        //     BoardViewer.Selecting = -1;
        //     BoardViewer.SelectingFunction = 0;
        //     commonPearl.SetCooldown();
        //     MatchManager.Ins.InputProcessor.Unmark();
        //     MatchManager.Ins.InputProcessor.UpdateRelic();
        //     Dispose();
        // }



        // public void CompleteActionForAI()
        // {
        //     UnityEngine.Debug.Log("CompleteActionForAI");
        //     var allPieces = MatchManager.Ins.GameState.PieceBoard;
        //     var listPieces = allPieces.Where(p => p != null && p.Color == commonPearl.Color).ToList();
        //     int minBuff = int.MaxValue;
        //     foreach (var piece in listPieces)
        //     {
        //         int coutBuff = piece.Effects.Count(e => e.Category == EffectCategory.Buff && e.EffectName != "effect_extremophile");
        //         if (coutBuff < minBuff)
        //         {
        //             minBuff = coutBuff;
        //         }
        //     }
        //     var bestPiece = listPieces.Where(p => p.Effects.Count(e => e.Category == EffectCategory.Buff) == minBuff).ToList();
        //     var random = new System.Random();
        //     var selectedPiece = bestPiece[random.Next(bestPiece.Count)];
        //     var effect = GetRandomBuffEffect(selectedPiece);
        //     if (effect == null) return;
        //     ActionManager.ExecuteImmediately(new ApplyEffect(effect));
        //     commonPearl.SetCooldown();
        //     MatchManager.Ins.InputProcessor.UpdateRelic();
        // }
    }
}