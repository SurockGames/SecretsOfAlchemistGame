using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

namespace Assets.Code
{
    [Serializable]
    public abstract class Item : SerializedScriptableObject, IItem
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        [SerializeField] private Sprite icon;

        public Sprite Icon => icon;
        public abstract ItemTypes ItemType { get; }

        public virtual Item CompareItem => this;
    }

    public abstract class ItemVariant : Item, IItemVariant
    {
        protected abstract Item originalItemVersion { get; set; }
        public override Item CompareItem => originalItemVersion;

        public Item GetOriginalItem()
        {
            return originalItemVersion;
        }
    }


    public interface IItem
    {
        Item CompareItem { get; }
        ItemTypes ItemType { get; }
    }
    public interface IItemVariant
    {
        Item GetOriginalItem();
    }


    public interface IGrindable { }

    public interface IService
    {
        void Initialize();
    }

    public class ServiceLocator : RegulatorSingleton<ServiceLocator>
    {
        public HashSet<IService> services = new HashSet<IService>();

        public void RegisterService(IService service)
        {
            services.Add(service);
        }

        public void UnregisterService(IService service)
        {
            services.Remove(service);
        }

        public IService GetService(IService service)
        {
            services.TryGetValue(service, out IService storedService);
            return storedService;
        }
    }
}