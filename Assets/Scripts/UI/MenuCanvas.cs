using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : CanvasEffect
{
    [SerializeField] Button StartButton;
    [SerializeField] Button CreditButton;
    [SerializeField] Button ExitButton;
    [SerializeField] List<Button> ButtonList;


    public void OnStartButton()
    { 
        
    }

    public void OnCreditButton()
    { 
        
    }

    public void OnExitButton()
    { 
        Application.Quit();
    }

}