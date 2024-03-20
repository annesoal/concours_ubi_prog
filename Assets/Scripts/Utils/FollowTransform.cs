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

    public void SetFollowParameters(GameObject toFollow, float distance, DirectionOfFollow directionOfFollow)
    {
        toFollow = toFollow;
        distance = distance;
        directionOfFollow = directionOfFollow;
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
