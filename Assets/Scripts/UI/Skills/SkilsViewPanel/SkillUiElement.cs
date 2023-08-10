using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUiElement : MonoBehaviour
{
    [SerializeField] private Image colourImage;
    [SerializeField] private Image selectImage;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI label;

    public event Action OnClick;
    public int SkillId { get; private set; }

    private void Start()
    {
        button.onClick.AddListener(OnClickInvoke);
    }
    private void OnClickInvoke() => OnClick?.Invoke();

    public void Init(int skillId, Color color, string labelText)
    {
        SkillId = skillId;
        SetSelectedColor(color);
        label.text = labelText;
        ShowSelect(false);
    }

    public void SetSelectedColor(Color color)
    {
        colourImage.color = color;
    }

    public void ShowSelect(bool show) => selectImage.enabled = show;
}