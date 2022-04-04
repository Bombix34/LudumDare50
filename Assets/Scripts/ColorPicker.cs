using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    private Camera mainCamera;

    public Color currentColor;

    [SerializeField] private float colorSpeed;

    [SerializeField] private List<Color> colors;

    private static ColorPicker instance;
    public static ColorPicker Instance { get => instance; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        instance = this;

        mainCamera = Camera.main;
    }

    public void SetupNewColor(List<SpriteRenderer> sprites)
    {
        currentColor = colors[Random.Range(0, colors.Count)];
        mainCamera.backgroundColor = currentColor;
        mainCamera.DOColor(currentColor, colorSpeed);
        foreach(var sprite in sprites)
        {
            sprite.DOColor(currentColor, colorSpeed);
        }
    }
}
