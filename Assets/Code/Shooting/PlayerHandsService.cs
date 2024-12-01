using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shooting
{
    public class PlayerHandsService : MonoBehaviour
    {
        public GameObject hintOuijiEquip;
        public GameObject hintGunEquip;


        public Game Game;
        public OuijiVisuals Ouija;

        public PlayerGunShootingService gunShootingService;
        //private Dictionary<GunSO, ItemStack> guns = new();
        [ShowInInspector, ReadOnly]
        private List<GunSO> gunsList = new();
        [ShowInInspector, ReadOnly]
        private HandsItemTypes currentHandItem;

        [ShowInInspector, ReadOnly]
        private GunSO currentGun;

        private bool ouijiIsCollected;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (gunsList.Count == 0) return;
                if (gunsList[0] != currentGun)
                    EquipGun(gunsList[0]);

                if (currentHandItem != HandsItemTypes.Gun)
                    GetOutEquipedGun();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!ouijiIsCollected) return;
                if (currentHandItem == HandsItemTypes.Gun)
                    TakeAwayAllItemsFromHands();

                EquipOuijaToLeftHand();
            }
        }

        public void CollectOuiji()
        {
            ouijiIsCollected = true;
            hintOuijiEquip.SetActive(true);
        }

        [Button]
        public bool TryCollectGun(GunSO gun, ItemStack ammoWithGun = null)
        {
            if (gunsList.Contains(gun)) return false;

            //guns.Add(gun, ammoWithGun);
            gunsList.Add(gun);

            if (ammoWithGun == null) return true;

            Game.PlayerInventoryService.TryAddItemToInventory(ammoWithGun.Item, ammoWithGun.Amount);
            Game.GatherPopupService.CreateGatherPopup(ammoWithGun.Item, ammoWithGun.Amount);

            hintGunEquip.SetActive(true);
            return true;
        }

        [Button]
        public void EquipGun(GunSO gun)
        {
            if (!gunsList.Contains(gun)) return;

            currentGun = gun;

            gunShootingService.Initialize(currentGun);
            GetOutEquipedGun();
        }

        [Button]
        public void GetOutEquipedGun()
        {
            TakeAwayAllItemsFromHands();
            currentHandItem = HandsItemTypes.Gun;

            gunShootingService.GetGunFromHolster();
        }

        [Button]
        public void TakeAwayAllItemsFromHands()
        {
            if (currentHandItem == HandsItemTypes.Gun)
            {
                gunShootingService.PutGunInHolster();
            }
            else if (currentHandItem == HandsItemTypes.Ouija)
            {
                Ouija.HideOuiji();
            }

            currentHandItem = HandsItemTypes.None;
        }

        [Button]
        public void EquipOuijaToLeftHand()
        {
            TakeAwayAllItemsFromHands();

            Ouija.GetOutOuiji();
            currentHandItem = HandsItemTypes.Ouija;
        }
    }

    public enum HandsItemTypes
    {
        None,
        Gun,
        Ouija,
        Letter
    }
}