using System;
using System.Collections;
using System.Collections.Generic;
using Amulets;
using UnityEngine;

public class AmuletChoicesUI : MonoBehaviour
{
    [SerializeField] private SingleAmuletChoiceUI leftAmuletChoice;
    [SerializeField] private SingleAmuletChoiceUI centerAmuletChoice;
    [SerializeField] private SingleAmuletChoiceUI rightAmuletChoice;

    private void Start()
    {
        leftAmuletChoice.OnAmuletSelectButtonClicked += LeftAmuletChoice_OnAmuletSelectButtonClicked;
        centerAmuletChoice.OnAmuletSelectButtonClicked += CenterAmuletChoice_OnAmuletSelectButtonClicked;
        rightAmuletChoice.OnAmuletSelectButtonClicked += RightAmuletChoice_OnAmuletSelectButtonClicked;
    }

    public void SetVisuals(List<AmuletSO> amuletChoiceAtEnd)
    {
        Debug.Assert(amuletChoiceAtEnd.Count == 3);
        
        leftAmuletChoice.SetVisuals(amuletChoiceAtEnd[0]);
        centerAmuletChoice.SetVisuals(amuletChoiceAtEnd[1]);
        rightAmuletChoice.SetVisuals(amuletChoiceAtEnd[2]);
    }

    public event EventHandler<OnAmuletChosenEventArgs> OnAmuletChosen;
    public class OnAmuletChosenEventArgs : EventArgs { public AmuletSO AmuletChosen; }
    
    private void LeftAmuletChoice_OnAmuletSelectButtonClicked
        (object sender, SingleAmuletChoiceUI.OnAmuletSelectButtonClickedEventArgs e)
    {
        OnAmuletChosen?.Invoke(this, new OnAmuletChosenEventArgs
        {
            AmuletChosen = e.SelectedAmulet,
        });
    }
    
    private void CenterAmuletChoice_OnAmuletSelectButtonClicked
        (object sender, SingleAmuletChoiceUI.OnAmuletSelectButtonClickedEventArgs e)
    {
        OnAmuletChosen?.Invoke(this, new OnAmuletChosenEventArgs
        {
            AmuletChosen = e.SelectedAmulet,
        });
    }
    private void RightAmuletChoice_OnAmuletSelectButtonClicked
        (object sender, SingleAmuletChoiceUI.OnAmuletSelectButtonClickedEventArgs e)
    {
        OnAmuletChosen?.Invoke(this, new OnAmuletChosenEventArgs
        {
            AmuletChosen = e.SelectedAmulet,
        });
    }

}
