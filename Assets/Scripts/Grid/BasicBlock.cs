using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class BasicBlock : MonoBehaviour, Block
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Awake est bien dans editor ou mettre en edit mode
    // Peut creer un lag 
    void Awake()
    {
        
    }


    public Vector3 getPosition()
    {
        return this.transform.localPosition;
    }
}
