using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickableElement : MonoBehaviour
{
    public bool isActive = true;
    public Action OnClickElement;

    private void OnEnable()
    {
        TimerManager.OnEndTimer += OnTimerEnd;
    }

    private void OnDisable()
    {
        TimerManager.OnEndTimer -= OnTimerEnd;
    }

    private void OnTimerEnd()
    {
        isActive = false;
    }

    private void Update()
    {
        if (!isActive)
            return;
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if(hit.collider != null && hit.collider.gameObject==this.gameObject)
            {
                OnClickElement?.Invoke();
            }
        }
    }
}
