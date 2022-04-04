using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private RectTransform transitionPanel;

    private void Awake()
    {
        transitionPanel.DOAnchorPosY(Screen.height, 1f);
        button.onClick.AddListener(OnClickButtonPlay);
    }
    public void OnClickButtonPlay()
    {
        StartCoroutine(OnClickCoroutine());
    }

    private IEnumerator OnClickCoroutine()
    {
        button.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f);
        transitionPanel.DOAnchorPosY(0f, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameScene");

    }
}
