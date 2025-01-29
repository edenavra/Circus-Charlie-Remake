using Pool;
using UnityEngine;

namespace FireRings
{
    /// <summary>
    /// Represents a smaller version of the FireRing that contains a Money Sack.
    /// </summary>
    public class SmallFireRing : FireRing
    {
        [SerializeField] private MoneySack moneySack;
        /// <summary>
        /// Initializes the SmallFireRing, ensuring the Money Sack is assigned.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (moneySack == null)
            {
                moneySack = GetComponentInChildren<MoneySack>();
            }
            RingType = FireRingType.WithMoneySack;
            Reset();
        }
        
        /// <summary>
        /// Resets the SmallFireRing and its associated Money Sack.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            // Reset the money sack if it's assigned
        
            if (moneySack != null)
            {        
                moneySack.ResetSack();
            }
            else
            {
                Debug.LogError("MoneySack is missing in SmallFireRing!");
            }
        }
        
        /// <summary>
        /// Returns this SmallFireRing instance back to its object pool.
        /// </summary>
        protected override void ReturnToPool()
        {
            SmallFireRingPool.Instance.Return(this);
        }
    
    }
}
