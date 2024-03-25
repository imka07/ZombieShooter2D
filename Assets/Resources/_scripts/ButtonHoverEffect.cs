using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScaleMultiplier = 1.2f; // Множитель для изменения размера при наведении
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Увеличиваем размер кнопки при наведении
        transform.localScale = originalScale * hoverScaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Возвращаем размер кнопки к исходному значению при уходе курсора с кнопки
        transform.localScale = originalScale;
    }
}
