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
            { 0, "雏鸟" },
            { 1, "腾飞" },
            { 2, "飞升" },
            { 3, "宏愿" },
            {4,""},
            {5,"团圆"}
        };
        LevelManager.OnLevelUpInt += OnLevelUp;
        OnLevelUp(0);
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
            title.color = new Color(1, 1, 1, timer / enterTime);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        title.color = Color.white;
        yield return new WaitForSecondsRealtime(stopTime);
        timer = 0;
        while (timer < exitTime)
        {
            title.color = new Color(1, 1, 1, 1-timer / exitTime);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }

        title.color = new Color(1, 1, 1, 0);
        title.gameObject.SetActive(false);
    }
}
