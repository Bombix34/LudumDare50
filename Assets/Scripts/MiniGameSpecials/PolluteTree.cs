using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tools.Managers;

public class PolluteTree : MonoBehaviour
{
    public bool IsDead { get; set; } = false;
    [SerializeField] private SpriteRenderer faceRenderer, bodyRenderer;
    [SerializeField] private Sprite deadFaceSprite;

    private void Start()
    {
        GetComponent<ClickableElement>().OnClickElement += OnClick;
        transform.DOScale(2.1f, 0.5f).SetEase(Ease.InOutCubic);
        faceRenderer.flipX = Random.Range(0f, 1f) < 0.5f;
        faceRenderer.color = ColorPicker.Instance.currentColor;
        bodyRenderer.color = ColorPicker.Instance.currentColor;
    }
    private void OnClick()
    {
        if (IsDead)
            return;
        IsDead = true;
        SoundManager.instance.PlaySound(AudioFieldEnum.SFX03_BOUP_2);
        faceRenderer.sprite = deadFaceSprite;
        transform.DOScale(0f, 0.5f).SetEase(Ease.InOutCubic);
    }
}
