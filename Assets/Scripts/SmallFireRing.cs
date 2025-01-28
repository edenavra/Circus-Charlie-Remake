using Pool;
using UnityEngine;

public class SmallFireRing : FireRing
{
    [SerializeField] private MoneySack moneySack;
    protected override void Start()
    {
        base.Start();
        if (moneySack == null)
        {
            moneySack = GetComponentInChildren<MoneySack>();
            if (moneySack == null)
            {
                Debug.LogError("MoneySack not found in children of SmallFireRing!");
            }
        }
        ringType = FireRingType.WithMoneySack;
        Reset();
    }

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

    protected override void ReturnToPool()
    {
        SmallFireRingPool.Instance.Return(this);
    }
    
}
