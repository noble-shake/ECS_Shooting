using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public enum CanvasType
{ 
    LogoCanvas,
    TitleCanvas,
    MenuCanvas,
    CharacterSelectCanvas,
    LoadingCanvas,
    ModeSelectCanvas,
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] public CanvasEffect LogoCanvas;
    [SerializeField] public CanvasEffect TitleCanvas;
    [SerializeField] public CanvasEffect MenuCanvas;
    [SerializeField] public CanvasEffect ModeSelectCanvas;
    [SerializeField] public CanvasEffect CharacterSelectCanvas;
    [SerializeField] public CanvasEffect LoadingCanvas;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CanvasEffect SwitchingMenu(CanvasType _type)
    {
        switch (_type)
        {
            case CanvasType.LogoCanvas:
                return LogoCanvas;
            default:
            case CanvasType.TitleCanvas:
                return TitleCanvas;
            case CanvasType.MenuCanvas:
                return MenuCanvas;
            case CanvasType.ModeSelectCanvas:
                return ModeSelectCanvas;
            case CanvasType.CharacterSelectCanvas:
                return CharacterSelectCanvas;
            case CanvasType.LoadingCanvas:
                return LoadingCanvas;
        }
    }


}