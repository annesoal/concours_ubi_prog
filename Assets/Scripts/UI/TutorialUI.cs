using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialsSlide;

    private int _currentSlideIndex = 0;

    private GameObject _toSelectAfterHide;

    private void Start()
    {
        InputManager.Instance.OnUserInterfaceSelectPerformed += InputManager_OnUserInterfaceSelectPerformed;
        
        BasicShowHide.Hide(gameObject);
    }

    private void InputManager_OnUserInterfaceSelectPerformed(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            _currentSlideIndex += 1;
            ShowSlide();
        }
    }

    public void ShowFirstSlide()
    {
        _toSelectAfterHide = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        
        HideAllTutorialSlides();
        
        _currentSlideIndex = 0;
        
        BasicShowHide.Show(gameObject);
        ShowSlide();
    }
    
    public void ShowSlide()
    {
        if (tutorialsSlide.Count == _currentSlideIndex)
        {
            HideTutorialUI();
        }
        else
        {
            Debug.Log("Current slide index : " + _currentSlideIndex);
            BasicShowHide.Show(tutorialsSlide[_currentSlideIndex]);
        }
    }

    private void HideAllTutorialSlides()
    {
        foreach (GameObject slide in tutorialsSlide)
        {
            BasicShowHide.Hide(slide);
        }
    }

    private const float TIMER_ACTIVATE_NAV_EVENT = 0.1f;
    private void HideTutorialUI()
    {
        Debug.Log("Hide tutorial UI");
        
        StartCoroutine(ActivateNavigationEventAfterTime(TIMER_ACTIVATE_NAV_EVENT));
    }
    
    private IEnumerator ActivateNavigationEventAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        EventSystem.current.sendNavigationEvents = true;
        EventSystem.current.SetSelectedGameObject(_toSelectAfterHide);
        
        BasicShowHide.Hide(gameObject); 
    }
    
}
