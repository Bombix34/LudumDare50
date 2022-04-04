using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tools.Managers;

public class PowerPlant : MonoBehaviour
{
    private Vector2 basePosition;
    private bool isPolluting=false;
    public bool IsDead { get; set; } = false;
    [SerializeField] private float timerBeforePolluting = 1f;
    private float currentChrono;
    [SerializeField] private GameObject pollutionParticles;
    [SerializeField] private SpriteRenderer faceRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private Sprite deadSprite;

    private void Start()
    {
        basePosition = transform.position;
        GetComponent<ClickableElement>().OnClickElement += OnClick;
        currentChrono = timerBeforePolluting;
        transform.DOScale(2f, 0.5f).SetEase(Ease.InOutCubic);
        faceRenderer.color = ColorPicker.Instance.currentColor;
        bodyRenderer.color = ColorPicker.Instance.currentColor;
        faceRenderer.flipX = Random.Range(0f, 1f) < 0.5f;
    }

    private void OnDisable()
    {
        GetComponent<ClickableElement>().OnClickElement -= OnClick;
    }

    private void Update()
    {
        if (IsPolluting || IsDead)
            return;
        currentChrono -= Time.deltaTime;
        if(currentChrono <= (timerBeforePolluting*0.4f))
        {
            this.transform.localPosition = basePosition + (Vector2.right * Random.Range(-0.1f, 0.1f));
        }
        if(currentChrono <= 0f)
        {
            IsPolluting = true;
            pollutionParticles?.SetActive(true);
        }
    }

    public void OnClick()
    {
        if (IsDead)
            return;
        IsDead = true;
        SoundManager.instance.PlaySound(AudioFieldEnum.SFX01_BOUP);
        GetComponent<ClickableElement>().enabled = false; 
        faceRenderer.sprite = deadSprite;
        pollutionParticles.GetComponent<ParticleSystem>().Stop();
        transform.DOPunchScale(Vector2.one * 0.2f, 0.3f)
            .OnComplete(() => transform.DOScale(0f, 1f).SetEase(Ease.InOutCubic));
    }

    public bool IsPolluting
    {
        get => isPolluting;
        set
        {
            isPolluting = value;
            pollutionParticles?.SetActive(isPolluting);
        }
    } 
}
