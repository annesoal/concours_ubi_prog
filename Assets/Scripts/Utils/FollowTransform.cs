using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public GameObject toFollow;
    public float distance;
    public DirectionOfFollow directionOfFollow;

    public enum DirectionOfFollow
    {
        Front,
        Back,
        Right,
        Left,
    }

    public void SetFollowParameters(GameObject _toFollow, float _distance, DirectionOfFollow _directionOfFollow)
    {
        toFollow = _toFollow;
        distance = _distance;
        directionOfFollow = _directionOfFollow;
    }
    
    void Update()
    {
        if (toFollow != null)
        {
            if (directionOfFollow == DirectionOfFollow.Front)
            {
                Transform toFollowTransform = toFollow.transform;
                transform.position = toFollowTransform.position + toFollowTransform.forward * distance;
            }
        }
    }
}
