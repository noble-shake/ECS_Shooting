using TMPro;
using UnityEngine;

public class TitleCanvas : CanvasEffect
{
    [SerializeField] TMP_Text PressAnyKey;
    float flowTime = 0f;
    float DelayTime = 2f;

    public void PressAnyKeyEffect()
    {
        flowTime += Time.deltaTime * 0.1f;
        if (flowTime * Mathf.Rad2Deg > 360f) flowTime = 0f;

        PressAnyKey.alpha = (1 + Mathf.Sin(flowTime * Mathf.Rad2Deg)) / 2;
    }

    private void Update()
    {
        PressAnyKeyEffect();
        KeyDown();
        DelayTime -= Time.deltaTime;
        if (DelayTime < 0f) DelayTime = 0f;
    }

    public void KeyDown()
    {
        if (Input.anyKeyDown && DelayTime <= 0f)
        {
            DelayTime = 2f;
            CanvasLoad();
        }
    }

    protected override void CanvasLoad()
    {
        CanvasEffect MenuCanvas = MenuManager.Instance.SwitchingMenu(CanvasType.MenuCanvas);
        MenuCanvas.gameObject.SetActive(true);
        CloseAlphaEffect();
    }
}
