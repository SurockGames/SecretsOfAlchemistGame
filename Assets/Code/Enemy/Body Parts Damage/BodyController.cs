using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BodyController : MonoBehaviour
{
    public abstract void RegisterHit(BodyPartType bodyPart, int damageAmount, Vector3 position);
}


