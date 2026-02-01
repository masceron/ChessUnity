using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    public class OvergrownSlugPending : PendingAction, System.IDisposable
    {
        private OvergrownSlug overgrownSlug;
        public OvergrownSlugPending(OvergrownSlug ogs, int maker, bool pos = false) : base(maker)
        {
            overgrownSlug = ogs;
            Maker = (ushort)maker;
        }

        // protected override void ModifyGameState()
        // {
        //     var (rank, file) = RankFileOf(Maker);
        //     var caller = PieceOn(Maker);
        //
        //     for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
        //     {
        //         if (!VerifyBounds(rankOff)) continue;
        //         
        //         for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
        //         {
        //             var p = PieceOn(IndexOf(rankOff, fileOff));
        //             if (p == null || p.Color != caller.Color) continue;
        //             var poison = p.Effects.Find(effect => effect.EffectName == "effect_poison");
        //             if (poison != null)
        //             {
        //                 poison.Strength--;
        //                 if (poison.Strength <= 0)
        //                 {
        //                     ActionManager.EnqueueAction(new RemoveEffect(poison));
        //                 }
        //             }
        //         }
        //     }
        //     BoardViewer.Selecting = -1;
        //     BoardViewer.SelectingFunction = 0;
        //
        //     overgrownSlug.SetCooldown();
        //     MatchManager.Ins.InputProcessor.Unmark();
        //     MatchManager.Ins.InputProcessor.UpdateRelic(); 
        //     Dispose();
        // }

        public override void CompleteAction()
        {
            BoardViewer.Ins.ExecuteAction(new OvergrownSlugAction(Maker));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            
            overgrownSlug.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
            Dispose();
        }

        public void Dispose()
        {
            overgrownSlug = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}