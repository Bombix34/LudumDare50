using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tools.Managers;

public class DinoMeteorite : MonoBehaviour
{
    [SerializeField] private GameObject meteorite, meteoriteShadow;
    private SpriteRenderer meteoriteSprite;
    [SerializeField] private float meteoriteSpeed=1f;

    [SerializeField] private Sprite crashedSprite;

    private float shadowScale = 0f;

    private bool isFalling = true;

    public GameObject player;

    public Vector2 rotationSpeedRandom;
    private float currentRotationSpeed;

    public Vector2 meteoriteSizeRandom;
    private float currentSize;

    private Sequence fallingSequence;

    private void Start()
    {
        meteoriteSprite = meteorite.GetComponent<SpriteRenderer>();
        meteoriteSprite.color = ColorPicker.Instance.currentColor;
        //meteoriteSprite.sortingOrder = (int)Vector2.Distance(Vector2.up * 500f, meteoriteShadow.transform.position);

        currentRotationSpeed = Random.Range(rotationSpeedRandom.x, rotationSpeedRandom.y);
        currentSize = Random.Range(meteoriteSizeRandom.x, meteoriteSizeRandom.y);

        meteorite.transform.localScale = currentSize * Vector2.one;

        meteorite.transform.position = meteoriteShadow.transform.position + (Vector3.up * 10f) + (Vector3.right*Random.Range(-2f,2f));
        meteoriteShadow.transform.DOScaleX(meteorite.transform.localScale.x, meteoriteSpeed);
        meteoriteShadow.transform.DOScaleY(meteorite.transform.localScale.x/2, meteoriteSpeed);

        fallingSequence = DOTween.Sequence();
        fallingSequence.Append(meteorite.transform.DOMove(meteoriteShadow.transform.position, meteoriteSpeed).SetEase(Ease.Linear));
        fallingSequence.Play();
    }

    private void Update()
    {
        if (!isFalling)
            return;
        meteorite.transform.Rotate(Vector3.forward * Time.deltaTime * currentRotationSpeed);
        if (Vector2.Distance(meteorite.transform.position, meteoriteShadow.transform.position) < 0.5f)
        {
            isFalling = false;
            fallingSequence.Kill();
            meteorite.transform.rotation = Quaternion.Euler(Vector3.zero);
            this.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f).SetEase(Ease.InBounce);
            if (player != null)
            {
                if (Vector2.Distance(player.transform.position, meteoriteShadow.transform.position) < 0.7f)
                {
                    meteoriteSprite.sprite = crashedSprite;
                    SoundManager.instance.PlaySound(AudioFieldEnum.SFX03_BOUP_2);
                    Camera.main.GetComponent<CameraShaker>().Shake(0.1f);
                    player.SetActive(false);
                    return;
                }
            }
            SoundManager.instance.PlaySound(AudioFieldEnum.SFX03_BOUP_2);
            Camera.main.GetComponent<CameraShaker>().Shake(0.1f);
            meteoriteSprite.sprite = crashedSprite;
            meteoriteShadow.GetComponent<SpriteRenderer>().DOFade(0f, 1f);
            meteoriteSprite.DOFade(0f, 1f)
                .OnComplete(() => Destroy(this.gameObject));
        }
        /*
        meteorite.transform.Translate(Vector3.down * Time.deltaTime * meteoriteSpeed);
        shadowScale += Time.deltaTime * meteoriteSpeed * 0.45f;
        meteoriteShadow.transform.localScale = new Vector2(shadowScale, shadowScale / 2);
        */
    }

    public float MeteoriteSpeed { get => meteoriteSpeed; set => meteoriteSpeed = value; }
}
