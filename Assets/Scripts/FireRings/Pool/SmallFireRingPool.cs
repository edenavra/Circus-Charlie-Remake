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
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }

}


