using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class able to layout a list of gameObject in a circle shape defined by serialize attributes.
/// </summary>
public class CircularLayoutUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjectsToLayout;
    
    /// <summary>
    /// Distance from the center of the circle, radius.
    /// </summary>
    [SerializeField] private float distanceFromCenter = 320f;
    
    /// <summary>
    /// Angle of which the first object will be placed.
    /// </summary>
    [SerializeField] private float startAngle = 0f;
    
    /// <summary>
    /// The minimum value of the angle of an object on the edge of the circle.
    /// </summary>
    [SerializeField] private float minAngle = 0f;
    
    /// <summary>
    /// The maximum value of the angle of an object on the edge of the circle.
    /// </summary>
    [SerializeField] private float maxAngle = 360f;

    /// <summary>
    /// Interval of the value of the angle separating each object.
    /// </summary>
    private float _placementAngleInterval;

    private RectTransform _rectTransform;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        _placementAngleInterval = (maxAngle - minAngle) / gameObjectsToLayout.Count;
        _placementAngleInterval = Mathf.Deg2Rad * _placementAngleInterval;
    }

    private void Start()
    {
        PlaceObjectsAroundCircle();
    }

    private void PlaceObjectsAroundCircle()
    {
        float angleToLayout = Mathf.Deg2Rad * startAngle;

        foreach (GameObject toLayout in gameObjectsToLayout)
        {
            toLayout.GetComponent<RectTransform>().localPosition = new Vector3(
                distanceFromCenter * Mathf.Sin(angleToLayout),
                distanceFromCenter * Mathf.Cos(angleToLayout),
                0f
            );

            angleToLayout += _placementAngleInterval;
        }
    }
}
