using Dreamteck.Splines;
using System;
using UnityEngine;

namespace Assets.Code.Shooting
{
    public class GunProceduralAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerGunShootingService gunShootingService;
        [SerializeField] private ChildPosition gunLookAtMuzzle;

        [Header("Targets")]
        [SerializeField] private Transform gunAimTarget;
        [SerializeField] private Transform handTarget;
        [SerializeField] private Transform aimingPointTargetMuzzle;
        [SerializeField] private Transform aimingPointTargetHandTarget;
        [SerializeField] private Transform character;
        [SerializeField] private Transform characterCamera;
        [SerializeField] private Transform characterDampingSideTarget;
        [SerializeField] private Transform characterDampingUpDownTarget;

        [SerializeField] private RectTransform uiMouseMove;


        [Header("Target movement settings")]
        [SerializeField] private float gunAimPositionDamping;
        [SerializeField] private float sideMovementMinOffset;
        [SerializeField] private float sideMovementMaxOffset;
        [SerializeField] private float verticalMovementMinOffset;
        [SerializeField] private float verticalMovementMaxOffset;
        [SerializeField] private float upDownMovementMinOffset;
        [SerializeField] private float upDownMovementMaxOffset;
        [SerializeField] private float interpolationSpeed;
        [SerializeField] private float interpolationAimSpeed;

        [SerializeField] private float mouseMoveStrenght;
        [SerializeField] private float mousePushStrenght;


        [Header("Splines settings")]
        [SerializeField] private float shootDuration = 1;
        [SerializeField] private float idleDuration = 4;
        [SerializeField] private float runDuration = 1;
        [SerializeField] private float interpolationSplinesSpeed;



        [Header("Spline Followers")]
        [SerializeField] protected SplineFollower gunAimFollower;
        [SerializeField] protected SplineFollower handFollower;

        [Header("Gun point Paths")]
        [SerializeField] private SplineComputer gunAimIdlePath;
        [SerializeField] private SplineComputer gunAimRunPath;
        [SerializeField] private SplineComputer gunAimShootPath;

        [Header("Hand point Paths")]
        [SerializeField] private SplineComputer handIdlePath;
        [SerializeField] private SplineComputer handRunPath;
        [SerializeField] private SplineComputer handShootPath;


        public event Action OnShoot;
        public event Action OnReload;

        private float horizontalOffset;
        private float verticalOffset;
        private float upDownOffset;


        private Vector3 handStartPosition;
        private Vector3 gunDampPosition;
        private Vector2 initialPosition;
        private Vector2 mouseMoveDirection;
        private Vector2 mouseMovedPosition;

        private const float horizontalMaxLerped = 3.5f;
        private const float verticalMaxLerped = 3.5f;
        private const float upDownMaxLerped = 3.5f;

        private bool isShooting;
        private bool isRunning;
        private bool isIdling;


        private void Start()
        {
            handStartPosition = handTarget.localPosition;
            //gunAimFollower.onEndReached += FinishShooting;
        }

        private void Update()
        {
            var lookDir = character.position - characterDampingSideTarget.position;
            float horizontalMovement = Vector3.Dot(lookDir, characterCamera.right);
            float verticalMovement = Vector3.Dot(lookDir, characterCamera.forward);

            // ---------- Horizontal Damping ------------
            var offsetPercent = Mathf.InverseLerp(0, horizontalMaxLerped, Mathf.Abs(horizontalMovement));
            horizontalOffset = Mathf.Lerp(sideMovementMinOffset, sideMovementMaxOffset, offsetPercent);
            horizontalOffset = horizontalOffset * Mathf.Sign(horizontalMovement);

            // ---------- Vertical Damping ------------
            offsetPercent = Mathf.InverseLerp(0, verticalMaxLerped, Mathf.Abs(verticalMovement));
            verticalOffset = Mathf.Lerp(verticalMovementMinOffset, verticalMovementMaxOffset, offsetPercent);
            verticalOffset = verticalOffset * Mathf.Sign(verticalMovement);

            // ---------- UpDown Damping ------------
            lookDir = character.position - characterDampingUpDownTarget.position;
            float upDownMovement = Vector3.Dot(lookDir, Vector3.up);
            offsetPercent = Mathf.InverseLerp(0, upDownMaxLerped, Mathf.Abs(upDownMovement));
            upDownOffset = Mathf.Lerp(upDownMovementMinOffset, upDownMovementMaxOffset, offsetPercent);
            upDownOffset = upDownOffset * Mathf.Sign(upDownMovement);

            // ---------- Mouse Rotation Damping ------------
            var currentPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            mouseMoveDirection = -(currentPosition - initialPosition);
            initialPosition = currentPosition;
        }

        private void LateUpdate()
        {
            float interpolation;

            if (!gunShootingService.IsAiming)
            {
                interpolation = interpolationSpeed;

                handTarget.localPosition = Vector3.MoveTowards(handTarget.localPosition,
                                                new Vector3(handFollower.transform.localPosition.x + horizontalOffset,
                                                            handFollower.transform.localPosition.y + upDownOffset,
                                                            handFollower.transform.localPosition.z + verticalOffset),
                                                interpolation * Time.deltaTime);

                upDownOffset = handFollower.transform.localPosition.y;
                horizontalOffset = handFollower.transform.localPosition.x;
                verticalOffset = handFollower.transform.localPosition.z;

                gunDampPosition = gunAimFollower.transform.position;

                gunAimTarget.localPosition = Vector3.MoveTowards(gunAimTarget.localPosition,
                                                    gunDampPosition,
                                                    interpolationSplinesSpeed * Time.deltaTime);
            }
            else
            {
                interpolation = interpolationAimSpeed;
            
                handTarget.localPosition = Vector3.MoveTowards(handTarget.localPosition,
                                                    handFollower.transform.localPosition,
                                                    interpolation * Time.deltaTime);
            
                upDownOffset = handFollower.transform.localPosition.y;
                horizontalOffset = handFollower.transform.localPosition.x;
                verticalOffset = handFollower.transform.localPosition.z;
            
                gunDampPosition = gunAimFollower.transform.position;
            
                gunAimTarget.localPosition = Vector3.MoveTowards(gunAimTarget.localPosition,
                                                    gunDampPosition,
                                                    interpolationSplinesSpeed * Time.deltaTime);
            }


            //else
            //{
            //    //gunLookAtMuzzle.IsChild = false;

            //    handTarget.localPosition = Vector3.MoveTowards(handTarget.localPosition,
            //                                        new Vector3(handFollower.transform.localPosition.x + horizontalOffset + aimingPointTargetHandTarget.localPosition.x,
            //                                                    handFollower.transform.localPosition.y + upDownOffset + aimingPointTargetHandTarget.localPosition.y,
            //                                                    handFollower.transform.localPosition.z + verticalOffset + aimingPointTargetHandTarget.localPosition.z),
            //                                        interpolationSpeed * Time.deltaTime);

            //    upDownOffset = handFollower.transform.localPosition.y;
            //    horizontalOffset = handFollower.transform.localPosition.x;
            //    verticalOffset = handFollower.transform.localPosition.z;

            //    gunDampPosition = gunAimFollower.transform.position;

            //    gunAimTarget.localPosition = Vector3.MoveTowards(gunAimTarget.localPosition,
            //                                        gunDampPosition + aimingPointTargetMuzzle.localPosition,
            //                                        interpolationSplinesSpeed * Time.deltaTime);
            //}
            //var magn = mouseMoveDirection.magnitude;
            //mouseMoveDirection = mouseMoveDirection + mouseMoveDirection.normalized * mouseMoveStrenght;
            //mouseMovedPosition = mouseMovedPosition + mouseMoveDirection;
            //mouseMovedPosition = Vector2.Lerp(mouseMovedPosition, Vector2.zero, magn * mousePushStrenght * Time.deltaTime);

            //gunDampPosition += (Vector3)mouseMovedPosition;
            //Debug.Log($"mouse move - {mouseMovedPosition}");

            //gunAimTarget.localPosition = Vector3.Lerp(gunAimTarget.localPosition,
            //                                    gunAimTarget.localPosition + (Vector3)mouseMoveDirection,
            //                                    1f - Mathf.Exp(-interpolationSpeed * Time.deltaTime));
        }


        public void Move() { }

        public void Idle()
        {
            gunAimFollower.spline = gunAimIdlePath;
            handFollower.spline = handIdlePath;

            gunAimFollower.wrapMode = SplineFollower.Wrap.Loop;
            handFollower.wrapMode = SplineFollower.Wrap.Loop;

            gunAimFollower.followDuration = idleDuration;
            handFollower.followDuration = idleDuration;

            gunAimFollower.follow = true;
            handFollower.follow = true;

            isIdling = true;
            isRunning = false;
        }

        public void Run()
        {
            gunAimFollower.spline = gunAimRunPath;
            handFollower.spline = handRunPath;

            gunAimFollower.wrapMode = SplineFollower.Wrap.Loop;
            handFollower.wrapMode = SplineFollower.Wrap.Loop;

            gunAimFollower.followDuration = runDuration;
            handFollower.followDuration = runDuration;

            gunAimFollower.follow = true;
            handFollower.follow = true;


            isRunning = true;
            isIdling = false;
        }

        public void Shoot()
        {
            //interpolationSplinesSpeed = 1;

            gunAimFollower.Restart();
            gunAimFollower.spline = gunAimShootPath;
            handFollower.spline = handShootPath;

            gunAimFollower.wrapMode = SplineFollower.Wrap.Default;
            handFollower.wrapMode = SplineFollower.Wrap.Default;

            gunAimFollower.followDuration = shootDuration;
            handFollower.followDuration = shootDuration;

            gunAimFollower.follow = true;
            handFollower.follow = true;

            isShooting = true;
            isIdling = false;
            isRunning = false;
        }

        private void FinishShooting(double obj)
        {
            if (isShooting)
            {
                isShooting = false;
                //interpolationSplinesSpeed = 0.3f;
            }
        }
    }
}