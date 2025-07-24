using Game.Board.General;
using UnityEngine;

namespace Game.Board.Effects
{
    [CreateAssetMenu(fileName = "EffectObject", menuName = "ScriptableObjects/EffectObject")]
    public class EffectObject : ScriptableObject
    {
        [SerializeField] public EffectType type;
        [SerializeField] public ObserverPriority priority;
        [SerializeField] public ObserverType activeWhen;
        [SerializeField] public Texture2D icon;

        [SerializeField] public string effectName;
        [TextArea] public string description;
    }
}
