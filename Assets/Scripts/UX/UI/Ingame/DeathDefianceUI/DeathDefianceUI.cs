using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Effects;
using UnityEngine;
using PrimeTween;
using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace UX.UI.Ingame.DeathDefianceUI
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class DeathDefianceUI: MonoBehaviour
    {
        [SerializeField] private GameObject selector;
        [SerializeField] private GameObject EffectItem;
        // [SerializeField] private TMP_Text titleText;

        private readonly string[] possibleEffects = {
            "effect_carapace",
            "effect_hardened_shield",
            "effect_piercing",
            "effect_shield",
            "effect_camouflage",
            "effect_haste",
            "effect_evasion",
            "effect_construct",
            "effect_demolisher",
            "effect_consume",
            "effect_surpass",
            "effect_ambush",
            "effect_quick_reflex",
        };
        private int piecePos;
        private List<string> selectedEffects = new();
        private static readonly System.Random Random = new System.Random();
        
        
        public void Load(int piecePos)
        {
            this.piecePos = piecePos;
            BoardUtils.PieceOn(piecePos);
            GetRandomEffect();
            CreateEffectItems();
        }

        private void GetRandomEffect()
        {
            selectedEffects = possibleEffects.OrderBy(_ => Random.Next()).Take(3).ToList();
        }
        private void CreateEffectItems()
        {
            for (var i = 0; i < 3; i++)
            {
                Instantiate(EffectItem, selector.transform, true);
            }
            for (var i = 0; i < selector.transform.childCount; i++)
            {
                selector.transform.GetChild(i).GetComponent<DeathDefianceItem>().Load(selectedEffects[i]);
            }
        }
        private void DestroyEffectItems()
        {
            for (var i = 0; i < selector.transform.childCount; i++)
            {
                Destroy(selector.transform.GetChild(i).gameObject);
            }
        }
        private void OnEnable()
        {
            var rect = (RectTransform)transform.GetChild(0);
            rect.anchoredPosition = new Vector2(-50, 0);
            Tween.UIAnchoredPosition(rect, Vector3.zero, 0.3f);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            ((RectTransform)transform.GetChild(0)).anchoredPosition = new Vector2(-50, 0);
        }

        private static Effect CreateEffect(string effectName, PieceLogic piece)
        {
            sbyte duration = 5;
            return effectName switch
            {
                // Buffs 
                // thiếu true bite 
                // đỏ mà ra mấy cái shield liên tục thì bất tử :<
                "effect_carapace" => new Game.Effects.Buffs.Carapace(duration, piece),
                "effect_hardened_shield" => new Game.Effects.Buffs.HardenedShield(piece),
                "effect_piercing" => new Game.Effects.Buffs.Piercing(duration, piece),
                "effect_shield" => new Game.Effects.Buffs.Shield(piece),
                "effect_camouflage" => new Game.Effects.Buffs.Camouflage(piece, duration),
                "effect_haste" => new Game.Effects.Buffs.Haste(duration, 1, piece),
                
                // Traits 
                "effect_evasion" => new Game.Effects.Traits.Evasion(duration, 50, piece),
                "effect_construct" => new Game.Effects.Traits.Construct(piece),
                "effect_demolisher" => new Game.Effects.Traits.Demolisher(piece),
                "effect_consume" => new Game.Effects.Traits.Consume(piece),
                "effect_surpass" => new Game.Effects.Traits.Surpass(piece),
                "effect_ambush" => new Game.Effects.Traits.Ambush(piece),
                "effect_quick_relfex" => new Game.Effects.Traits.QuickReflex(piece),
                
                _ => null
            };
        }
        public void ChooseEffect(string effectName)
        {
            var piece = BoardUtils.PieceOn(piecePos);
            var effect = CreateEffect(effectName, piece);
            ActionManager.ExecuteImmediately(new ApplyEffect(effect));
            Disable();
            DestroyEffectItems();
        }
    }
}