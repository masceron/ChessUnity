using Game.Effects;
using Game.Managers;
using UnityEngine;

namespace Game.Data.Effects
{
    [CreateAssetMenu(fileName = "EffectObject", menuName = "ScriptableObjects/EffectObject")]
    public class EffectObject : ScriptableObject
    {
        [SerializeField] public EffectName typeName;
        [SerializeField] public EffectCategory category;
        [SerializeField] public EffectStack stack;
        [SerializeField] public ObserverPriority priority;
        [SerializeField] public ObserverActivateWhen activeWhen;
        [SerializeField] public Texture2D icon;

        [SerializeField] public string effectName;
        [TextArea] public string description;
    }
}
