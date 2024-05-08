using UnityEngine;

namespace HL
{
    public class Item : ScriptableObject
    {
        public Sprite itemIcon;
        public string itemName;
        [TextArea] public string itemDescription;
    }
}