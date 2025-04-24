using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasEffect : MonoBehaviour
{
    [SerializeField] public CanvasType canvasType;
    [HideInInspector] public CanvasGroup _Canvas;
    [HideInInspector] public Image _Image;
    [HideInInspector] public Material _Material;
    [HideInInspector, Range(0f, 1f)] public float _MaterialAlpha = 1f;
    [HideInInspector, Range(0f, 3f)] public float _MaterialFactor = 0f;
    protected IEnumerator alphaEffector;
    protected IEnumerator materialEffector;
    bool AlphaPlusFlag;
    bool MaterialPlusFlag;
    bool NoMaterial;

    private void Awake()
    {
        TryGetComponent<CanvasGroup>(out _Canvas);

        if (_Canvas == null)
        {
            Debug.LogWarning($"{gameObject.name} :: althrough, UI Object. does not have CanvasGroup, Automatically Added with AddComponent.");
            gameObject.AddComponent<CanvasGroup>();
        }

        TryGetComponent<Image>(out _Image);

        if (_Image != null)
        {
            _Material = Instantiate(_Image.material);
            _Material.mainTexture = _Image.mainTexture;
            _MaterialAlpha = 1f;
            _Material.SetFloat("_Alpha", _MaterialAlpha);
            _Image.material = _Material;
        }

    }

    // Objective
    // 1. Control Alpha
    // 2. Control RayCaster
    // 3. If Opened
    // 4. If Closed

    // Control Alpha 
    // Basically, When Canvas Enable/Disabled, Canvas Effect will be executed.

    private void OnEnable()
    {
        TryGetComponent<CanvasGroup>(out _Canvas);
        if (_Canvas == null)
        {
            Debug.LogWarning($"{gameObject.name} :: althrough, UI Object. does not have CanvasGroup, Automatically Added with AddComponent.");
            gameObject.AddComponent<CanvasGroup>();
        }

        if (_Material == null)
        {
            _Material = Instantiate(_Image.material);
            _Material.mainTexture = _Image.mainTexture;
            _MaterialAlpha = 1f;
            _Material.SetFloat("_Alpha", _MaterialAlpha);
            _Image.material = _Material;
        }

        StartAlphaEffect(true);
        StartMaterialEffect(true);
    }
    public void StartAlphaEffect(bool isOn = true, float speed = 1f)
    {
        alphaEffector = AlphaEffect(isOn, speed);
        StartCoroutine(alphaEffector);
    }

    public void StartMaterialEffect(bool isOn = true, float speed = 1f)
    {
        if (_Material == null) return;
        materialEffector = MaterialEffect(isOn, speed);
        StartCoroutine(materialEffector);
    }

    public IEnumerator AlphaEffect(bool isOn, float speed = 2f)
    {
        AlphaPlusFlag = isOn;
        _Canvas.blocksRaycasts = false;
        if (isOn)
        {
            while (_MaterialAlpha < 1f)
            {
                _MaterialAlpha += Time.deltaTime * speed;

                if (_MaterialAlpha > 1f) _MaterialAlpha = 1f;
                _Material.SetFloat("_Alpha", _MaterialAlpha);
                _Image.material = _Material;
                yield return null;
            }
            _Canvas.blocksRaycasts = true;
        }
        else
        {
            while (_MaterialAlpha > 0f)
            {
                _MaterialAlpha -= Time.deltaTime * speed;

                if (_MaterialAlpha <= 0f) _MaterialAlpha = 0f;
                _Material.SetFloat("_Alpha", _MaterialAlpha);
                _Image.material = _Material;
                yield return null;
            }
            gameObject.SetActive(false);
            StopEffect();
        }
        yield return null;

    }

    public IEnumerator MaterialEffect(bool isOn, float speed = 2f)
    {
        MaterialPlusFlag = isOn;
        if (isOn)
        {
            while (_MaterialFactor < 3f)
            {
                _MaterialFactor += Time.deltaTime * speed;

                if (_MaterialFactor > 3f) _MaterialFactor = 3f;
                _Material.SetFloat("_Factor", _MaterialFactor);
                yield return null;
            }
            _Canvas.blocksRaycasts = true;
        }
        else
        {
            while (_MaterialAlpha > 0f)
            {
                _MaterialFactor -= Time.deltaTime * speed;

                if (_MaterialFactor < 0f) _MaterialFactor = 0f;
                _Material.SetFloat("_Factor", _MaterialFactor);
                yield return null;
            }
            gameObject.SetActive(false);
        }
        yield return null;
    }

    public void SkipEffect()
    {
        if(alphaEffector != null) StopCoroutine(alphaEffector);
        if (AlphaPlusFlag)
        {
            _Canvas.alpha = 1f;
            _Canvas.blocksRaycasts = true;
        }
        else
        {
            _Canvas.alpha = 0f;
            _Canvas.blocksRaycasts = false;
        }

    }

    public void StopEffect()
    {
        if (alphaEffector != null) StopCoroutine(alphaEffector);
        if (materialEffector != null) StopCoroutine(materialEffector);

        _Canvas.alpha = 1f;
        _MaterialAlpha = 1f;
        _MaterialFactor = 0f;
        _Material.SetFloat("_Alpha", _MaterialAlpha);
        _Material.SetFloat("_Factor", _MaterialFactor);
        _Image.material = _Material;
    }

    // If Canvas Closed..
    public void CloseAlphaEffect()
    {
        StartAlphaEffect(false);
    }

    private void OnDisable()
    {
        StopEffect();
    }

    protected virtual void CanvasLoad()
    { 
        
    }
}