using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public GameObject toFollow;
    public Vector3 distance;
    public DirectionOfFollow directionOfFollow;

    public enum DirectionOfFollow
    {
        Front,
        Back,
        Right,
        Left,
        FrontRight,
    }

    public void SetFollowParameters(GameObject _toFollow, Vector3 _distance, DirectionOfFollow _directionOfFollow)
    {
        toFollow = _toFollow;
        distance = _distance;
        directionOfFollow = _directionOfFollow;
    }
    
    void Update()
    {
        if (toFollow != null)
        {
            if (directionOfFollow == DirectionOfFollow.FrontRight)
            {
                Transform toFollowTransform = toFollow.transform;
                transform.position =
                    toFollowTransform.position +
                    toFollowTransform.forward * distance.z +
                    toFollowTransform.right * distance.x;
                return;
            }
            
            if (directionOfFollow == DirectionOfFollow.Front)
            {
                Transform toFollowTransform = toFollow.transform;
                transform.position = toFollowTransform.position + toFollowTransform.forward * distance.z;
                return;
            }
        }
    }
}
