using Sirenix.OdinInspector;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    //public Animator animator;

    [Button]
    public void TurnOnRagdoll()
    {
        var rigidbodies = GetComponentsInChildren<Rigidbody>();

        //animator.enabled = false;

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
    }
}
