using Pool;
using UnityEngine;

public class FireRingPool : MonoPool<FireRing>
{
    //private static FireRingPool _instance;

    public static FireRingPool Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
        {
            Debug.LogError("Multiple FireRingPool instances detected!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log($"{GetType().Name} initialized successfully.");
    }
}
