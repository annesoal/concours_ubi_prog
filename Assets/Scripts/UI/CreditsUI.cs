using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject firstPartCredit;
    [SerializeField] private GameObject lastPartCredit;

    [SerializeField] private float slideTime;
    [SerializeField] private float hideDelay;
    
    private Vector2 _initialFirstPartCreditPosition;
    private Vector2 _initialLastPartCreditPosition;

    private void Awake()
    {
        _initialFirstPartCreditPosition = firstPartCredit.GetComponent<RectTransform>().anchoredPosition;
        _initialLastPartCreditPosition = lastPartCredit.GetComponent<RectTransform>().anchoredPosition;
        
        BasicShowHide.Hide(gameObject);
    }

    private void Start()
    {
        InputManager.Instance.OnUserInterfaceCancelPerformed += InputManager_OnUserInterfaceCancelPerformed;
    }


    private int _currentFirstCreditId;
    private int _currentLastCreditId;
    private GameObject _toReselectOnClose;

    public void ShowCredits()
    {
        _toReselectOnClose = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        
        CancelTweens();

        firstPartCredit.GetComponent<RectTransform>().anchoredPosition =
            _initialFirstPartCreditPosition;
        lastPartCredit.GetComponent<RectTransform>().anchoredPosition =
            _initialLastPartCreditPosition;

        //_currentFirstCreditId =
            //LeanTween.moveY(firstPartCredit.GetComponent<RectTransform>(), Screen.height * 2, slideTime).id;
        _currentLastCreditId =
            LeanTween.moveY(lastPartCredit.GetComponent<RectTransform>(), 0, slideTime).setOnComplete(HideCreditsWithDelay).id;
        
        BasicShowHide.Show(gameObject);
    }

    private void HideCreditsWithDelay()
    {
        StartCoroutine(HideCreditsTimer());
    }

    private IEnumerator HideCreditsTimer()
    {
        yield return new WaitForSeconds(hideDelay);
        
        HideCredits();
    }
    
    private void HideCredits()
    {
        CancelTweens();
        
        BasicShowHide.Hide(gameObject);
        
        EventSystem.current.SetSelectedGameObject(_toReselectOnClose);
    }

    private void CancelTweens()
    {
        LeanTween.cancel(_currentFirstCreditId);
        LeanTween.cancel(_currentLastCreditId);
    }

    private void InputManager_OnUserInterfaceCancelPerformed(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            HideCredits();
        }
    }
    
}
