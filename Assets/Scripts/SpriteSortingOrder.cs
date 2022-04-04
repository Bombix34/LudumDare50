using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortingOrder : MonoBehaviour
{
    [SerializeField] private GameObject objectToUseForPosition;
    [SerializeField] private int modificator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(new Vector3(0f, objectToUseForPosition.transform.position.y, 0f), new Vector3(0f, 50f, 0f));
        spriteRenderer.sortingOrder = ((int)(distance * 10) + modificator);
    }
}
