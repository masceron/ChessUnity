using System.Linq;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Game.Action.Relics;
using ZLinq;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CommonPearlPending : Action, System.IDisposable, IRelicAction
    {
        private CommonPearl commonPearl;
        
        public CommonPearlPending(CommonPearl cp, int maker, bool pos = false) : base(maker)
        {
            commonPearl = cp;
            Target = (ushort)maker;
            Maker = (ushort)maker;
        }
        public Effect GetRandomBuffEffect(PieceLogic piece)
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            
            return CreateEffectFromName(selectedEffectName, piece);
        }

        private static Effect CreateEffectFromName(string effectName, PieceLogic piece)
        {
            var randomDuration = (sbyte)new System.Random().Next(2, 6);

            return effectName switch
            {
                "effect_shield" => new Effects.Buffs.Shield(piece),
                "effect_carapace" => new Effects.Buffs.Carapace(randomDuration, piece),
                "effect_haste" => new Effects.Buffs.Haste(randomDuration, 1, piece),
                "effect_piercing" => new Effects.Buffs.Piercing(randomDuration, piece),
                "effect_hardened_shield" => new Effects.Buffs.HardenedShield(piece),
                "effect_true_bite" => new Effects.Buffs.TrueBite(piece),
                "effect_camouflage" => new Effects.Buffs.Camouflage(piece),
                _ => null
            };
        }
        // public void CompleteAction()
        // {
        //     ActionManager.ExecuteImmediately(new ApplyEffect(GetRandomBuffEffect(PieceOn(Target))));

        //     BoardViewer.Selecting = -1;
        //     BoardViewer.SelectingFunction = 0;
        //     commonPearl.SetCooldown();
        //     MatchManager.Ins.InputProcessor.Unmark();
        //     MatchManager.Ins.InputProcessor.UpdateRelic();
        //     Dispose();
        // }

        public void Dispose()
        {
            commonPearl = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(GetRandomBuffEffect(PieceOn(Target)), commonPearl));

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            commonPearl.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }



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