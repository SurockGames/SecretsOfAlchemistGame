using System.Collections;
using UnityEngine;

namespace Assets.Code.Items
{
    public class PickupItem : MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private int amount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ItemGrabber itemGrabber))
            {
                if (itemGrabber.TakeItem(item, amount))
                    Destroy(gameObject);
            }
        }
    }
}