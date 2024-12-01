using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Shooting
{
    public class RevolverVisuals : MonoBehaviour
    {
        [SerializeField] private PlayerGunShootingService gunShootingService;
        [SerializeField] private Animator revolverAnimator;
        [SerializeField] private Transform revolverTransform;

        [SerializeField] private Vector3 gunAimPosition;
        [SerializeField] private Vector3 gunAimRotation;

        [SerializeField] private Vector3 gunDefaultPosition;
        [SerializeField] private Vector3 gunDefaultRotation;
        [SerializeField] private float interpolationPositionSpeed;
        [SerializeField] private float interpolationRotationSpeed;

        [SerializeField] private ParticleSystem shootParticle;


        protected static readonly int ShootHash = Animator.StringToHash("Shoot");
        protected static readonly int ReloadHash = Animator.StringToHash("Reload");
        protected static readonly int RunningHash = Animator.StringToHash("Running");
        protected static readonly int AimingHash = Animator.StringToHash("Aiming");
        protected static readonly int GunEquipHash = Animator.StringToHash("GunEquiped");

        protected static readonly int ShootStateTagHash = Animator.StringToHash("ShootTag");

        private bool isReloading;
        private bool isShooting;
        private bool shootStateChangedThisFrame;
        protected const float crossFadeDuration = 0.1f;

        private void Start()
        {
            gunShootingService.OnGunGetOut += PlayHoldingGunAnimation;
            gunShootingService.OnGunTakeAway += StopHoldingGunAnimation;
            //gunShootingService.OnGunGetOut += StartAimingAnimation;
            //gunShootingService.OnGunTakeAway += StopAimingAnimation;
            gunShootingService.OnStartRunning += PlayRunAnimation;
            gunShootingService.OnStopRunning += StopRunAnimation;
            gunShootingService.OnGunShoot += PlayShootAnimation;
            gunShootingService.OnReload += PlayReloadAnimation;
            gunShootingService.OnStopReload += StopReloadAnimation;
        }

        private void OnDestroy()
        {
            gunShootingService.OnGunGetOut -= PlayHoldingGunAnimation;
            gunShootingService.OnGunTakeAway -= StopHoldingGunAnimation;
            //gunShootingService.OnGunGetOut -= StartAimingAnimation;
            //gunShootingService.OnGunTakeAway -= StopAimingAnimation;
            gunShootingService.OnStartRunning -= PlayRunAnimation;
            gunShootingService.OnStopRunning -= StopRunAnimation;
            gunShootingService.OnGunShoot -= PlayShootAnimation;
            gunShootingService.OnReload -= PlayReloadAnimation;
            gunShootingService.OnStopReload -= StopReloadAnimation;
        }

        private void Update()
        {
            var isInShootState = revolverAnimator.GetCurrentAnimatorStateInfo(3).tagHash == ShootStateTagHash;


            if (isShooting)
            {
                // Shoot in not shooting state
                if (shootStateChangedThisFrame && !isInShootState)
                {
                    shootStateChangedThisFrame = false;
                }
                // Go in shooting state first time
                else if (!shootStateChangedThisFrame && isInShootState)
                {
                    isShooting = false;
                    revolverAnimator.SetBool(ShootHash, false);
                }
            }

            if (isReloading)
            {
                isReloading = false;
            }
            else if (revolverAnimator.GetBool(ReloadHash))
            {
                revolverAnimator.SetBool(ReloadHash, false);
            }
        }

        private void LateUpdate()
        { 
            if (gunShootingService.IsAiming)
            {
                revolverTransform.localPosition = Vector3.MoveTowards(revolverTransform.localPosition,
                                                gunAimPosition,
                                                interpolationPositionSpeed * Time.deltaTime);

                revolverTransform.localRotation = Quaternion.RotateTowards(revolverTransform.localRotation,
                                                Quaternion.Euler(gunAimRotation),
                                                interpolationRotationSpeed * Time.deltaTime);
            }
            else
            {
                revolverTransform.localPosition = Vector3.MoveTowards(revolverTransform.localPosition,
                                                gunDefaultPosition,
                                                interpolationPositionSpeed * Time.deltaTime);


                revolverTransform.localRotation = Quaternion.RotateTowards(revolverTransform.localRotation,
                                                Quaternion.Euler(gunDefaultRotation),
                                                interpolationRotationSpeed * Time.deltaTime);
            }
        }
        

        public void PlayShootAnimation()
        {
            isShooting = true;
            shootStateChangedThisFrame = true;
            revolverAnimator.SetBool(ShootHash, true);
            shootParticle.Play();
        }

        public void PlayRunAnimation()
        {
            revolverAnimator.SetBool(RunningHash, true);
        }

        public void StopRunAnimation()
        {
            revolverAnimator.SetBool(RunningHash, false);
        }

        public void PlayHoldingGunAnimation()
        {
            revolverAnimator.SetBool(GunEquipHash, true);
        }

        public void StopHoldingGunAnimation()
        {
            revolverAnimator.SetBool(GunEquipHash, false);
        }

        public void StartAimingAnimation()
        {
            revolverAnimator.SetFloat(AimingHash, 1);
        }

        public void StopAimingAnimation()
        {
            revolverAnimator.SetFloat(AimingHash, 0);
        }

        public void PlayReloadAnimation()
        {
            isReloading = true;
            revolverAnimator.SetBool(ReloadHash, true);
        }

        public void StopReloadAnimation()
        {
            isReloading = true;
            revolverAnimator.SetBool(ReloadHash, true);
        }
    }
}