using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BodyPartsDamageFactor", order = 1)]
public class BodyPartsDamageFactor : SerializedScriptableObject
{
    [SerializeField] private Dictionary<BodyPartType, float> bodyPartsFactors;

    public float GetBodyFactor(BodyPartType bodyPart)
    {
        return bodyPartsFactors[bodyPart];
    }
}


