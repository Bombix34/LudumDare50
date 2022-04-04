using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameColorElements : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> spritesToColor;

    private void Start()
    {
        ColorPicker.Instance.SetupNewColor(spritesToColor);
    }
}
