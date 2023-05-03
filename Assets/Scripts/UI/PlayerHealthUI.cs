using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour {
    private Text levelText;
    private Image healthSlider;
    private Image expSlider;

#region 生命周期函数
    private void Awake() {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    void Update() {
        // Bad Practice
        levelText.text = "Level " + GameManager.Instance.playerStats.CurrentLevel;
        UpdateHealth();
        UpdateExp();
    }

#endregion

    void UpdateHealth() {
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    void UpdateExp() {
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentExp / GameManager.Instance.playerStats.BaseExp;
        expSlider.fillAmount = sliderPercent;
    }
}
