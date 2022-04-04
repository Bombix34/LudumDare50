using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortingOrder : MonoBehaviour
{
    [SerializeField] private GameObject objectToUseForPosition;
    [SerializeField] private int modificator;
    private ParticleSystem particleRenderer;

    private void Awake()
    {
        particleRenderer = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(new Vector3(0f, objectToUseForPosition.transform.position.y, 0f), new Vector3(0f, 50f, 0f));
        particleRenderer.GetComponent<ParticleSystemRenderer>().sortingOrder = ((int)(distance * 10) + modificator);
    }
}
