using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Code.Shooting
{
    [CreateAssetMenu(fileName = "Data", menuName = "Guns/GunData", order = 4)]
    public class GunSO : ScriptableObject
    {
        [Title("Settings")]
        [SerializeField] private int maxAmmoAmount;
        [SerializeField] private LayerMask targetMask;

        [Header("Damage")]
        [SerializeField] private int damageAmount;
        [SerializeField, Range(0, 1)] private float minDamageOnMaxDistanceFactor;
        [SerializeField, ShowIf("@minDamageOnMaxDistanceFactor != 1"), PropertyRange(0, nameof(range))] private float minRangeDamageFalloff;

        [Header("Weapon accuracy")]
        [SerializeField, Range(0, 1)] private float projectileAccuracy;
        [SerializeField] private float range, aimAssistDefault, accuracyDefault, stability;
        [SerializeField] private float minAimAssist, maxAccuracy, shootingAimAssist, shootingAccuracy;

        [SerializeField, Range(0, 3), Tooltip("How strong will AA or AC change after shoot in percant")] private float aimAssistRecoilFactor, accuracyRecoilFactor;

        [Header("Visuals")]
        [SerializeField] private float recoilStrenght;
        [SerializeField] private bool isCameraShakeingWhenShootNotAiming;
        [SerializeField] private float conesOneUnitChangeTime;
        [SerializeField] private float recoilACNormalizeOneUnitChangeTime, recoilAANormalizeOneUnitChangeTime;

        public int MaxAmmoAmount => maxAmmoAmount;
        public float Range => range; 
        public float AimAssistDefault => aimAssistDefault; 
        public float AccuracyDefault => accuracyDefault; 
        public float Stability => stability;
        public float MinAimAssist => minAimAssist; 
        public float MaxAccuracy => maxAccuracy; 
        public float ShootingAimAssist => shootingAimAssist; 
        public float ShootingAccuracy => shootingAccuracy;

        public float RecoilStrenght => recoilStrenght; 
        public bool IsCameraShakeingWhenShootNotAiming => isCameraShakeingWhenShootNotAiming; 
        public float ConesOneUnitChangeTime => conesOneUnitChangeTime; 
        public float RecoilACNormalizeOneUnitChangeTime => recoilACNormalizeOneUnitChangeTime; 
        public float RecoilAANormalizeOneUnitChangeTime => recoilAANormalizeOneUnitChangeTime;

        public float ProjectileAccuracy => projectileAccuracy; 
        public float MinRangeDamageFalloff => minRangeDamageFalloff; 
        public float MinDamageOnMaxDistanceFactor => minDamageOnMaxDistanceFactor; 
        public int DamageAmount => damageAmount; 
        public float AimAssistRecoilFactor => aimAssistRecoilFactor; 
        public float AccuracyRecoilFactor => accuracyRecoilFactor;

        public LayerMask TargetMask => targetMask; 
    }
}