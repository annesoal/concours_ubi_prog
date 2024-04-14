using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsDisplayer : MonoBehaviour
{
    private Animator _animator;
    
    private static readonly int Victory = Animator.StringToHash("Victory");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.SetBool(Victory, true);
    }
}
