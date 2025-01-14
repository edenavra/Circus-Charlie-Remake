using System;
using UnityEngine;

public class FitCameraToWorldDimensions : MonoBehaviour
{
    private const float RatioChangeThreshold = 0.01f;

    [SerializeField] private Camera cam;
    [Header("How many world Unity units fit into the screen width")]
    [SerializeField] private float width = 8f; 
    [SerializeField] private float height = 6f;
    private float _currRatio;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;
        
        if (!cam.orthographic) 
            Debug.LogWarning("Camera is not orthographic, this script is designed for orthographic cameras");
    }

    private void Start()
    {
        _currRatio = (float)Screen.width / Screen.height;
        FitCamera();
    }

    private void Update()
    {
        var newRatio = (float)Screen.width / Screen.height;
        if (Math.Abs(newRatio - _currRatio) > RatioChangeThreshold)
        {
            _currRatio = newRatio;
            FitCamera();
        }
    }

    private void FitCamera()
    {
        var currHeight = cam.orthographicSize * 2f;
        var currWidth = currHeight * _currRatio;
        var widthRatio = width / currWidth;
        var heightRatio = height / cam.orthographicSize;
        var ratioChange = Math.Min(widthRatio, heightRatio);
        cam.orthographicSize *= ratioChange;
    }
}