using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveAndExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public TextMeshProUGUI buttonText;
    private Color originalColor;
    private Color glowColor = new Color32(8, 248, 255, 255);
    private float glowIntensity = 1.5f;
    public GameObject optionsMenu;
    public Interactor interactor;

    [Header("Start Menu")]
    public GameObject regularScreen;

    
    void Start() {
        if (buttonText == null) {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }
        originalColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        buttonText.color = glowColor * glowIntensity;
    }

    public void OnPointerExit(PointerEventData eventData) {
        buttonText.color = originalColor;
    }

    public void closeMenu() {
        optionsMenu.SetActive(false);
        regularScreen.SetActive(true);
    }
}
