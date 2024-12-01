using DamageNumbersPro;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [SerializeField] private DamageNumber numberPrefab;
    private EnemyController enemyController;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();

        enemyController.OnGetDamage += GetDamage;
    }

    public void GetDamage(int damageAmount, Vector3 position, bool isKrit)
    {
        DamageNumber damageNumber = numberPrefab.Spawn(position, damageAmount);

        if (isKrit)
            damageNumber.SetColor(Color.red);
        else
            damageNumber.SetColor(Color.white);
        //Debug.Log("Got damage in visuals!");
    }

    public void OnDestroy()
    {
        enemyController.OnGetDamage -= GetDamage;
    }
}
