using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Managers;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    private Button button;
    [SerializeField] private AudioFieldEnum audioOnClick;

    private void Awake()
    {
        button = GetComponent<Button>();
        button?.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        SoundManager.instance.PlaySound(audioOnClick);
    }
}
