using Pool;
using UnityEngine;
namespace Pool
{

    public class SmallFireRingPool : MonoPool<SmallFireRing>
    {
        public static SmallFireRingPool Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (Instance != null)
            {
                Debug.LogError("Multiple SmallFireRingPool instances detected!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            //Debug.Log($"{GetType().Name} initialized successfully.");
        }
    }

}


