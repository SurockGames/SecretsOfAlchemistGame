using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(fileName = "Data", menuName = "Ammunitions/Patron", order = 1)]
    public class Patron : Item
    {
        public override ItemTypes ItemType => ItemTypes.Ammunition;

    }
}