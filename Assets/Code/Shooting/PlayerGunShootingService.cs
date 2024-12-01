using Sirenix.OdinInspector;
using System;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.Shooting
{
    public class RecoilFactor
    {
    }

    public class PlayerGunShootingService : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Game game;
        [SerializeField] private GunInventory gunInventory;
        [SerializeField] private Camera cameraMain;
        [SerializeField] private Transform aimObject;
        [SerializeField] private Rigidbody cone;
        [SerializeField] private Collider coneCollider;

        [SerializeField] private Cone ConeCast;
        [SerializeField] private Cone ConeCastPreview;

        public int LoadedPatronsAmount => currentLoadedAmmoAmount;
        public int AmountOfPatronsInSlot => currentActivePatronsAmount;
        public Patron ActivePatronItem => currentPatron;
        public event Action OnPatronChanged;
        // Tranperent AC when shot in aimed 
        // Move HitCursor with gun aim after shot 

        [Title("UI References")]
        public bool ShowUIReferences;

        [SerializeField] private GameObject UIAimGroup;

        [ShowIf(nameof(ShowUIReferences))]
        [BoxGroup("Accuracy crosshair")]
        [SerializeField] private RectTransform UIElementLeft, UIElementRight, UIElementUp, UIElementDown;

        [ShowIf(nameof(ShowUIReferences))]
        [BoxGroup("Aim assist crosshair")]
        [SerializeField] private RectTransform UIElementAALeft, UIElementAARight, UIElementAAUp, UIElementAADown;

        [ShowIf(nameof(ShowUIReferences))]
        [BoxGroup("Target point")]
        [SerializeField] private RectTransform UIElementTarget;

        [Title("Settings")]
        [SerializeField] private GunSO gunData;

        public event Action OnPatronAmountChange;
        public event Action OnPatronItemChange;
        public event Action<Patron, int> OnReturnAmmo;
        public event Action OnGunGetOut;
        public event Action OnGunTakeAway;
        public event Action OnGunShoot;
        public event Action OnReload;
        public event Action OnStopReload;
        public event Action OnStartRunning;
        public event Action OnStopRunning;

        public bool IsAiming => currentState == GunStates.Aiming;

        private LayerMask targetMask;

        [ShowInInspector, ReadOnly]
        private Patron currentPatron;
        [ShowInInspector, ReadOnly]
        private int maxAmmoAmount;
        [ShowInInspector, ReadOnly]
        private int currentLoadedAmmoAmount = 0;
        private int currentActivePatronsAmount = 6;

        private int damageAmount;
        private float minDamageOnMaxDistanceFactor;
        private float minRangeDamageFalloff;
        private float projectileAccuracy;
        private float range, aimAssistDefault, accuracyDefault, stability;
        private float minAimAssist, maxAccuracy, shootingAimAssist, shootingAccuracy;

        private float aimAssistRecoilFactor, accuracyRecoilFactor;

        private float recoilStrenght;
        private bool isCameraShakeingWhenShootNotAiming;
        private float conesOneUnitChangeTime;
        private float recoilACNormalizeOneUnitChangeTime, recoilAANormalizeOneUnitChangeTime;

        Transform cameraTrs;
        RaycastHit[] hits;

        [ShowInInspector, ReadOnly]
        private float currentAimAssist, currentAccuracy, targetAimAssist, targetAccuracy, recoilAccuracyAmount, recoilAimAssistAmount;
        //private Vector2 crosshairCenter;

        private float accuracyChangeSpeed, aimAssistChangeSpeed, recoilAccuracyChangeSpeed, recoilAimAssistChangeSpeed;
        private GunStates currentState = GunStates.NotAiming;
        //private GunActionStates currentGunActionState = GunActionStates.Idle;
        private const float minRecoil = 0.01f;

        private bool isAbleToShoot;
        private bool isGunInHolster;
        private float reloadedTimer;
        private bool isReloading;

        private enum GunStates
        {
            None,
            NotAiming,
            Aiming,
        }
        private enum GunActionStates
        {
            None,
            Idle,
            Running,
            Reloading
        }

        [Button]
        public void Initialize(GunSO gunData)
        {
            this.gunData = gunData;

            targetMask = gunData.TargetMask;
            maxAmmoAmount = gunData.MaxAmmoAmount;

            range = gunData.Range;
            aimAssistDefault = gunData.AimAssistDefault;
            accuracyDefault = gunData.AccuracyDefault;
            stability = gunData.Stability;
            minAimAssist = gunData.MinAimAssist;
            maxAccuracy = gunData.MaxAccuracy;
            shootingAimAssist = gunData.ShootingAimAssist;
            shootingAccuracy = gunData.ShootingAccuracy;

            recoilStrenght = gunData.RecoilStrenght;
            isCameraShakeingWhenShootNotAiming = gunData.IsCameraShakeingWhenShootNotAiming;
            conesOneUnitChangeTime = gunData.ConesOneUnitChangeTime;
            recoilAANormalizeOneUnitChangeTime = gunData.RecoilAANormalizeOneUnitChangeTime;
            recoilACNormalizeOneUnitChangeTime = gunData.RecoilACNormalizeOneUnitChangeTime;

            projectileAccuracy = gunData.ProjectileAccuracy;
            minRangeDamageFalloff = gunData.MinRangeDamageFalloff;
            minDamageOnMaxDistanceFactor = gunData.MinDamageOnMaxDistanceFactor;
            damageAmount = gunData.DamageAmount;
            aimAssistRecoilFactor = gunData.AimAssistRecoilFactor;
            accuracyRecoilFactor = gunData.AccuracyRecoilFactor;

            UIAimGroup.SetActive(true);
        }

        private void Start()
        {
            cameraTrs = cameraMain.transform;

            UpdateCone();
            UIAimGroup.SetActive(false);
        }

        [Button]
        public void GetGunFromHolster()
        {
            if (!PatronsLoaded())
            {
                TryStartReload();
            }

            UIAimGroup.SetActive(true);
            OnGunGetOut?.Invoke();
            isGunInHolster = false;
        }

        [Button]
        public void PutGunInHolster()
        {
            UIAimGroup.SetActive(false);
            OnGunTakeAway?.Invoke();
            isGunInHolster = true;
        }

        public bool TryStartReload()
        {
            if (gunInventory.PatronInSlot == null || gunInventory.AmountOfPatronsInSlot == 0) return false;

            if (currentActivePatronsAmount > 0 && currentLoadedAmmoAmount < maxAmmoAmount)
            {
                StartReloading();
                return true;
            }
            return false;
        }

        public void ReloadAnimationSuccess()
        {
            Debug.Log("Reload Success");
            if (reloadedTimer <= 0.01f && isReloading)
            {
                Reload(gunInventory.PatronInSlot, gunInventory.AmountOfPatronsInSlot);
                reloadedTimer = 2f;
                isReloading = false;
            }
        }

        public void Reload(Patron patron, int amount)
        {
            if (patron == null || amount <= 0) return;

            var amountToAdd = amount;

            // We get equiped patron type
            if (currentPatron && patron == currentPatron)
            {
                amountToAdd = maxAmmoAmount - currentLoadedAmmoAmount;
                amountToAdd = Mathf.Min(amount, amountToAdd);

                if (amountToAdd == 0) return;

                currentLoadedAmmoAmount += amountToAdd;
                gunInventory.RemoveItemFromInventory(patron, amountToAdd);

                OnPatronAmountChange?.Invoke();
                return;
            }
            else
            {
                // We get another patron type and we need to return previous type
                if (currentPatron && currentLoadedAmmoAmount != 0)
                {
                    OnReturnAmmo?.Invoke(currentPatron, currentLoadedAmmoAmount);

                }

                currentPatron = patron;

                amountToAdd = Mathf.Min(amount, maxAmmoAmount);
                currentLoadedAmmoAmount = amountToAdd;
                gunInventory.RemoveItemFromInventory(patron, amountToAdd);

                OnPatronChanged?.Invoke();
                OnPatronAmountChange?.Invoke();

                return;
            }
        }

        public void StopReloading()
        {
            isReloading = false;
        }

        public void LoadAmmoToCurrentGun(Patron patron, int amount)
        {

        }

        public bool TryGetPatronsFromInventory()
        {

            return false;
        }

        private bool PatronsLoaded()
        {
            if (currentLoadedAmmoAmount > 0)
                return true;
            else
                return false;
        }

        private void StartReloading()
        {
            OnReload?.Invoke();
            isReloading = true;
        }

        void Update()
        {
            reloadedTimer -= Time.deltaTime;

            if (!isAbleToShoot || gunData == null || isGunInHolster)
                return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                TryStartReload();
                return;
            }

            // Set UI
            var maxRangeCenter = cameraTrs.position + cameraTrs.forward * range;
            SetUIAimAccuracy(maxRangeCenter);
            //SetUIAimAimAssist(maxRangeCenter);

            if (Input.GetKey(KeyCode.Mouse1))
                currentState = GunStates.Aiming;
            else
                currentState = GunStates.NotAiming;

            SetTargetCones();

            // smoothly set accuracy and aim assist depending on amount of recoil
            var targetAccuracyWithRecoil = targetAccuracy + recoilAccuracyAmount;
            var targetAimAssistWithRecoil = targetAimAssist - recoilAimAssistAmount;
            currentAccuracy = Mathf.SmoothDamp(currentAccuracy, targetAccuracyWithRecoil, ref accuracyChangeSpeed, conesOneUnitChangeTime * Mathf.Abs((int)(currentAccuracy - targetAccuracyWithRecoil)));
            currentAimAssist = Mathf.SmoothDamp(currentAimAssist, targetAimAssistWithRecoil, ref aimAssistChangeSpeed, conesOneUnitChangeTime * Mathf.Abs((int)(currentAimAssist - targetAimAssistWithRecoil)));

            // Decrease recoil
            if (recoilAccuracyAmount > 0.01f)
                recoilAccuracyAmount = Mathf.SmoothDamp(recoilAccuracyAmount, 0, ref recoilAccuracyChangeSpeed, recoilACNormalizeOneUnitChangeTime * Mathf.Abs((int)recoilAccuracyAmount));
            if (recoilAimAssistAmount > minRecoil)
                recoilAimAssistAmount = Mathf.SmoothDamp(recoilAimAssistAmount, 0, ref recoilAimAssistChangeSpeed, recoilAANormalizeOneUnitChangeTime * Mathf.Abs((int)recoilAimAssistAmount));

            // Shoot 
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Check if have patrons 
                if (currentLoadedAmmoAmount > 0)
                {
                    currentLoadedAmmoAmount--;
                    OnGunShoot?.Invoke();
                    OnPatronAmountChange?.Invoke();

                    UpdateCone();
                    Shoot();
                    Recoil();
                    StopReloading();
                }
                else
                {
                    TryStartReload();
                }
            }
        }


        public void LockShooting()
        {
            PutGunInHolster();
            isAbleToShoot = false;
            UIAimGroup.SetActive(false);
        }

        public void UnlockShooting()
        {
            if (isGunInHolster && gunData)
            {
                GetGunFromHolster();
                //UIAimGroup.SetActive(true);
            }

            isAbleToShoot = true;
        }

        private void SetTargetCones()
        {
            if (currentState == GunStates.Aiming)
            {
                targetAccuracy = shootingAccuracy;
                targetAimAssist = shootingAimAssist;
            }
            else if (currentState == GunStates.NotAiming)
            {
                targetAccuracy = accuracyDefault;
                targetAimAssist = aimAssistDefault;
            }
        }

        private void Recoil()
        {
            //Crosshair follow gun aim if aiming
            if (currentState == GunStates.Aiming)
            {
                targetAccuracy = accuracyDefault;
                targetAimAssist = aimAssistDefault;
            }
            else if (currentState == GunStates.NotAiming)
            {

            }
            targetAccuracy = shootingAccuracy;
            targetAimAssist = shootingAimAssist;

            recoilAccuracyAmount = ((targetAccuracy + recoilAccuracyAmount) * (1 + accuracyRecoilFactor)) - targetAccuracy;
            recoilAimAssistAmount = targetAimAssist - Mathf.Abs((targetAimAssist - recoilAimAssistAmount) * (1 - aimAssistRecoilFactor));

            recoilAccuracyAmount = Mathf.Min(recoilAccuracyAmount, maxAccuracy - targetAccuracy);
        }

        private void Shoot()
        {
            var maxRangeCenter = cameraTrs.position + cameraTrs.forward * range;

            Vector2 randDir = UnityEngine.Random.insideUnitCircle;
            Vector3 target = maxRangeCenter + cameraTrs.right * (currentAccuracy / 2) * randDir.x;
            target = target + cameraTrs.up * (currentAccuracy / 2) * randDir.y;
            var targetScreenPosition = cameraMain.WorldToScreenPoint(target);

            UIElementTarget.position = targetScreenPosition;
            RaycastHit hit;
            bool hitSmt;

            if (currentAccuracy > currentAimAssist)
            {
                var aaToAC = currentAimAssist / currentAccuracy;
                if (math.abs(randDir.x) >= aaToAC || math.abs(randDir.y) >= aaToAC)
                {
                    // CHECK RAYCAST point because Aim assist is not helping there 

                    hitSmt = Physics.Raycast(cameraMain.ScreenPointToRay(targetScreenPosition), out hit, range + 100, targetMask);
                    if (hitSmt)
                    {
                        aimObject.position = hit.point;
                        TryDoDamage(hit.collider.gameObject, hit.point);
                    }
                    return;
                }
            }

            hitSmt = Physics.Raycast(cameraMain.ScreenPointToRay(targetScreenPosition), out hit, range + 100, targetMask);

            if (hitSmt)
            {
                aimObject.position = hit.point;
                if (TryDoDamage(hit.collider.gameObject, hit.point))
                    return;
            }

            coneCollider.enabled = true;
            hits = cone.SweepTestAll(-cameraTrs.forward, range);

            float minDistanceSq = 1000;
            float minDistanceToDamageable = 1000;
            int closestInd = -1;
            Vector3 closestHitPointMagnetizm = Vector3.zero;
            Vector3 closestDamageablePointMagnetizm = Vector3.zero;
            bool[] hitsDamageables = new bool[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.TryGetComponent(out IDamagable damagable))
                    hitsDamageables[i] = true;
            }

            for (int i = 0; i < hits.Length; i++)
            {
                var projectionPoint = GetPositionOnSegment(cameraTrs.position, maxRangeCenter, hits[i].transform.position);
                var closestPointToProjection = hits[i].collider.ClosestPoint(projectionPoint);

                var distSq = math.distancesq(closestPointToProjection, projectionPoint);
                if (distSq < minDistanceSq)
                {
                    minDistanceSq = distSq;
                    closestHitPointMagnetizm = closestPointToProjection;

                    if (hitsDamageables[i])
                    {
                        minDistanceToDamageable = distSq;
                        closestInd = i;
                        closestDamageablePointMagnetizm = closestPointToProjection;
                    }
                }
                else if (distSq < minDistanceToDamageable && hitsDamageables[i])
                {
                    minDistanceToDamageable = distSq;
                    closestInd = i;
                    closestDamageablePointMagnetizm = closestPointToProjection;
                }
            }

            if (hits.Length > 0 && closestInd != -1)
            {
                var pointOnScreen = cameraMain.WorldToScreenPoint(closestDamageablePointMagnetizm);
                hitSmt = Physics.Raycast(cameraMain.ScreenPointToRay(pointOnScreen), out hit, range + 100, targetMask);

                if (hitSmt)
                {
                    aimObject.position = hit.point;
                    if (TryDoDamage(hit.collider.gameObject, hit.point))
                    {
                        coneCollider.enabled = false;
                        return;
                    }
                }

                aimObject.position = closestHitPointMagnetizm;
                hitSmt = Physics.Raycast(cameraMain.ScreenPointToRay(closestHitPointMagnetizm), out hit, range + 100, targetMask);

                if (hitSmt)
                {
                    TryDoDamage(hit.collider.gameObject, closestHitPointMagnetizm);
                }
                //Debug.Log("Do damage at closest magnetize point");
            }
            else
            {
                hitSmt = Physics.Raycast(cameraMain.ScreenPointToRay(targetScreenPosition), out hit, range + 100, targetMask);
                //Debug.Log($"Hit not in cone but with AA in - {hit.collider.name}");
                Debug.DrawRay(cameraMain.ScreenPointToRay(targetScreenPosition).origin, cameraMain.ScreenPointToRay(targetScreenPosition).direction * (range + 100), Color.red, 4);

                if (hitSmt)
                {
                    aimObject.position = hit.point;
                    if (TryDoDamage(hit.collider.gameObject, hit.point))
                    {
                        coneCollider.enabled = false;
                        return;
                    }
                }
            }

            coneCollider.enabled = false;
        }

        private bool TryDoDamage(GameObject hitObject, Vector3 position)
        {
            if (hitObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TryDealDamage(damageAmount, position);

                return true;
            }

            return false;
        }

        private void SetUIAimAccuracy(Vector3 maxRangeCenter)
        {
            var leftPointer = maxRangeCenter - cameraTrs.right * (currentAccuracy / 2);
            var upperPointer = maxRangeCenter + cameraTrs.up * (currentAccuracy / 2);


            var screenCoordinatesLeftPointer = cameraMain.WorldToScreenPoint(leftPointer);
            var screenCoordinatesUpPointer = cameraMain.WorldToScreenPoint(upperPointer);

            UIElementLeft.position = screenCoordinatesLeftPointer;
            UIElementRight.position = new Vector3(Screen.width - screenCoordinatesLeftPointer.x, screenCoordinatesLeftPointer.y, screenCoordinatesLeftPointer.z);

            UIElementUp.position = screenCoordinatesUpPointer;
            UIElementDown.position = new Vector3(screenCoordinatesUpPointer.x, Screen.height - screenCoordinatesUpPointer.y, screenCoordinatesUpPointer.z);
        }
        private void SetUIAimAimAssist(Vector3 maxRangeCenter)
        {
            var leftPointer = maxRangeCenter - cameraTrs.right * (currentAimAssist / 2);
            var upperPointer = maxRangeCenter + cameraTrs.up * (currentAimAssist / 2);


            var screenCoordinatesLeftPointer = cameraMain.WorldToScreenPoint(leftPointer);
            var screenCoordinatesUpPointer = cameraMain.WorldToScreenPoint(upperPointer);

            var halfWidth = (Screen.width / 2 - screenCoordinatesLeftPointer.x) / 2;
            var halfHeight = (Screen.height / 2 - screenCoordinatesUpPointer.y) / 2;

            UIElementAALeft.position = new Vector3(screenCoordinatesLeftPointer.x,
                                                   Screen.height - (screenCoordinatesUpPointer.y),
                                                   screenCoordinatesLeftPointer.z);

            UIElementAARight.position = new Vector3(Screen.width - screenCoordinatesLeftPointer.x,
                                                    screenCoordinatesUpPointer.y,
                                                    screenCoordinatesLeftPointer.z);

            UIElementAAUp.position = new Vector3(screenCoordinatesLeftPointer.x,
                                                 screenCoordinatesUpPointer.y,
                                                 screenCoordinatesUpPointer.z);

            UIElementAADown.position = new Vector3(Screen.width - screenCoordinatesLeftPointer.x,
                                                   Screen.height - screenCoordinatesUpPointer.y,
                                                   screenCoordinatesUpPointer.z);

            //UIElementAALeft.position = new Vector3(screenCoordinatesLeftPointer.x + halfWidth, 
            //                                       Screen.height - (screenCoordinatesUpPointer.y + halfHeight),    
            //                                       screenCoordinatesLeftPointer.z);

            //UIElementAARight.position = new Vector3(Screen.width - screenCoordinatesLeftPointer.x - halfWidth,
            //                                        screenCoordinatesUpPointer.y + halfHeight, 
            //                                        screenCoordinatesLeftPointer.z);

            //UIElementAAUp.position = new Vector3(screenCoordinatesLeftPointer.x + halfWidth,
            //                                     screenCoordinatesUpPointer.y + halfHeight,
            //                                     screenCoordinatesUpPointer.z);
            //UIElementAADown.position = new Vector3(Screen.width - screenCoordinatesLeftPointer.x - halfWidth,
            //                                       Screen.height - screenCoordinatesUpPointer.y - halfHeight,
            //                                       screenCoordinatesUpPointer.z);
        }
        public void UpdateCone()
        {
            ConeCast.CreateNewMesh(currentAimAssist / 2, range);
            ConeCastPreview.CreateNewMesh(currentAimAssist / 2, range);

            ConeCast.transform.localPosition = new Vector3(0, 0, range * 2);
            ConeCastPreview.transform.localPosition = new Vector3(0, 0, range);
        }

        public Vector3 GetPositionOnSegment(Vector3 A, Vector3 B, Vector3 point)
        {
            Vector3 projection = Vector3.Project(point - A, B - A);
            return projection + A;
        }
    }
}