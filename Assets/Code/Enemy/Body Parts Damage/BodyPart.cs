using Assets.Code.Shooting;
using UnityEngine;

public class BodyPart : MonoBehaviour, IDamagable
{
    [SerializeField] private BodyPartType type;

    public bool TryDealDamage(int amount, Vector3 position)
    {
        if (TryGetBodyController(out BodyController bodyController))
        {
            bodyController.RegisterHit(type, amount, position);
            return true;
        }
        return false;
    }

    private bool TryGetBodyController(out BodyController bodyController)
    {
        var controller = GetComponentInParent<BodyController>();

        bodyController = controller;
        return controller;
    }
}

public enum BodyPartType
{
    None, 
    Head,
    Body,
    Arms,
    Legs
}


