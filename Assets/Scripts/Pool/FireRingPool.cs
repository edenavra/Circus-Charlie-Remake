using Pool;
using UnityEngine;

public class FireRingPool : MonoPool<FireRing>
{
    public static FireRingPool Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this; }
}
