using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SizeAdjustOnSelectUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float scaleFactor;
    
    [SerializeField] private float tweenTime;
    
    private int _currentTweenId;
    public void OnSelect(BaseEventData eventData)
    {
        LeanTween.cancel(_currentTweenId);
        
        _currentTweenId = gameObject.transform.LeanScale(Vector3.one * scaleFactor, tweenTime).setEaseOutCirc().id;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        LeanTween.cancel(_currentTweenId);
        
        _currentTweenId = gameObject.transform.LeanScale(Vector3.one, tweenTime).setEaseOutCirc().id;
    }
}
