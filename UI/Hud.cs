using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Hud : MonoBehaviour
{

    [SerializeField] private Image _imagePrefab;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _staminaBar;
    [SerializeField] private Color _originalColorHpBar;
    [SerializeField] private Color _originalColorStaminaBar;

    private List<Image> _images = new List<Image>();

    public void SetStatus(string name) => _name.text = name;

    public void UpdateStamina(StaminaChangedEvent data) => _staminaBar.fillAmount = data.CurrentStamina / data.MaxStamina;

    public void UpdateHealth(HealthChangedEvent data) => _hpBar.fillAmount = data.CurrentHealth / data.MaxHealth;

    public void SetColorHpBar(Color color) => _hpBar.color = color;
    public void ResetColorHpBar() => _hpBar.color = _originalColorHpBar;

    public void SetColorStaminaBar(Color color) => _staminaBar.color = color;
    public void ResetColorStaminaBar() => _staminaBar.color = _originalColorStaminaBar;

    public void CreateImage(Sprite sprite)
    {
        Image image = Instantiate(_imagePrefab, _imagePrefab.transform.parent);

        _images.Add(image);

        image.transform.localPosition = new Vector2(_images.Count * 20f, 0);

        image.sprite = sprite;

        image.gameObject.SetActive(true);
    }

    public void DestroyAllImages()
    {
        foreach (var image in _images)
        {
            Destroy(image.gameObject);
        }

        _images.Clear();
    }

}