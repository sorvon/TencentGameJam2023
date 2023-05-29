using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;

public class StartFilmText : MonoBehaviour
{
    public float interval = 1f;

    private TMP_Text title;
    private GameManager gameManager;

    private void Start()
    {
        title = GetComponent<TMP_Text>();
        StartCoroutine(nameof(Flash));
        gameManager = ServiceLocator.Get<GameManager>();
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            StopAllCoroutines();
            gameManager.StartGame();
        }
    }

    IEnumerator Flash()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        while (true)
        {
            float timer = 0;
            while (timer < interval)
            {
                timer += Time.deltaTime;
                title.color = new Color(1, 1, 1, 1 - timer / interval);
                yield return new WaitForFixedUpdate();
            }

            timer = 0;
            while (timer < interval)
            {
                timer += Time.deltaTime;
                title.color = new Color(1, 1, 1, timer / interval);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}