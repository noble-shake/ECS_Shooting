using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas: CanvasEffect
{
    public PlayerType playerType;
    public Image DosaSymobl;
    public Image CraneSymbol;

    public void LoadingSymbolAction(PlayerType _type)
    {
        switch (_type)
        {
            case PlayerType.Dosa:
                DosaSymobl.gameObject.SetActive(true);
                CraneSymbol.gameObject.SetActive(false);
                break;
            case PlayerType.Crane:
                DosaSymobl.gameObject.SetActive(false);
                CraneSymbol.gameObject.SetActive(true);
                break;
        }
    }
}
