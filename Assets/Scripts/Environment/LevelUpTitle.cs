using System.Collections;
using System.Collections.Generic;
using Services;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelUpTitle : MonoBehaviour
{
    [SerializeField, Label("淡入时间")] private float enterTime;
    [SerializeField, Label("停留时间")] private float stopTime;
    [SerializeField, Label("淡出时间")] private float exitTime;
    
    private TMP_Text title;

    private Dictionary<int, string> level2title;
    private LevelManager LevelManager;

    protected void Start()
    {
        LevelManager = ServiceLocator.Get<LevelManager>();
        title = GetComponentInChildren<TMP_Text>();
        title.gameObject.SetActive(false);
        level2title = new Dictionary<int, string>()
        {
            { 0, "勇气" },
            { 1, "雏鸟" },
            { 2, "展翼" },
            { 3, "飞升" },
            { 4, "遥愿" },
            { 5, "团圆" }
        };
        LevelManager.OnLevelUpInt += OnLevelUp;
    }

    public void OnLevelUp(int level)
    {
        title.gameObject.SetActive(true);
        title.text = level2title[level];
        title.color = new Color(1, 1, 1, 0);
        StartCoroutine(TextAppear());
    }

    private IEnumerator TextAppear()
    {
        float timer = 0;
        while (timer < enterTime)
        {
            timer += Time.deltaTime;
            title.color = new Color(1, 1, 1, timer / enterTime);
            yield return new WaitForFixedUpdate();
        }
        title.color = Color.white;
        yield return new WaitForSeconds(stopTime);
        timer = 0;
        while (timer < exitTime)
        {
            timer += Time.deltaTime;
            title.color = new Color(1, 1, 1, 1-timer / exitTime);
            yield return new WaitForFixedUpdate();
        }

        title.color = new Color(1, 1, 1, 0);
        title.gameObject.SetActive(false);
    }
}
