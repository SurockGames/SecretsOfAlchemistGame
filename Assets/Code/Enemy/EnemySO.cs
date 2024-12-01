using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Enemies/Enemy", order = 1)]
public class EnemySO : ScriptableObject
{
    public int TockenValue;

    public EnemyController Enemy;
}
