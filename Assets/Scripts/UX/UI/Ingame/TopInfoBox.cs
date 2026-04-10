using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace UX.UI.Ingame
{
    public class TopInfoBox: MonoBehaviour
    {
        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            root.Q<Label>("RoundCounter").SetBinding("text", new DataBinding
            {
                dataSourcePath = new PropertyPath("CurrentTurn"),
                bindingMode = BindingMode.ToTarget
            });
            
            var binding = new DataBinding
            {
                dataSourcePath = new PropertyPath("IsDay"),
                bindingMode = BindingMode.ToTarget
            };
            
            var converterGroup = new ConverterGroup("DayNightConverter");
            converterGroup.AddConverter((ref bool isDay) => isDay ? "Day" : "Night");
            binding.ApplyConverterGroupToUI(converterGroup);

            root.Q<Label>("DayNight").SetBinding("text", binding);
        }
    }
}