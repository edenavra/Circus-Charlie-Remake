using Pool;
using UnityEngine;

public class SmallFireRing : FireRing
{
    [SerializeField] private MoneySack moneySack;
    //private protected FireRingType 
    private static int creationCount = 0;
    private static int resetCount = 0;
    protected override void Start()
    {
        base.Start();
        creationCount++;
        if (moneySack == null)
        {
            moneySack = GetComponentInChildren<MoneySack>();
            if (moneySack == null)
            {
                Debug.LogError("MoneySack not found in children of SmallFireRing!");
            }
        }
        //this.ringType = FireRingType.WithMoneySack;
        ringType = FireRingType.WithMoneySack;
        //GameManager.Instance.AddSmallFireRing(this);
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        // Reset the money sack if it's assigned
        resetCount++;
        
        if (moneySack != null)
        {        
            Debug.Log("SmallFireRing Reset Count: " + resetCount);
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

    /*public override void OnEndOfScreen()
    {
        GameManager.Instance.RemoveSmallFireRing(this);
        Destroy(this);
    }*/
}
