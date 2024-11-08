using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    public GameObject adKeysPanel;
    public GameObject mouseKeysPanel;
    public Image shiftKeyImage;
    public Image spaceKeyImage;

    private int step = 0;

    // Diccionarios para rastrear el estado de teclas y clics
    private Dictionary<KeyCode, bool> moveKeysPressed = new Dictionary<KeyCode, bool> {
        { KeyCode.A, false },
        { KeyCode.D, false },
        { KeyCode.LeftArrow, false },
        { KeyCode.RightArrow, false }
    };

    private Dictionary<int, bool> mouseClicksPressed = new Dictionary<int, bool> {
        { 0, false },  // Clic izquierdo
        { 1, false }   // Clic derecho
    };

    public void StartTutorial()
    {
        tutorialPanel.SetActive(true);
        ShowNextStep();
    }

    void ShowNextStep()
    {
        switch (step)
        {
            case 0:
                tutorialText.text = "Usa las teclas AD o las flechas para moverte";
                ResetUI();
                adKeysPanel.SetActive(true);
                break;
            case 1:
                tutorialText.text = "Presiona SPACE para saltar";
                ResetUI();
                spaceKeyImage.gameObject.SetActive(true);
                break;
            case 2:
                tutorialText.text = "Presiona SHIFT para hacer un dash";
                ResetUI();
                shiftKeyImage.gameObject.SetActive(true);
                break;
            case 3:
                tutorialText.text = "Presiona clic izquierdo para atacar y clic derecho para bloquear";
                ResetUI();
                mouseKeysPanel.SetActive(true);
                break;
            case 4:
                tutorialText.text = "¡Buen trabajo! Has completado el tutorial.";
                ResetUI();
                StartCoroutine(HideTutorialPanelAfterDelay(3f));
                break;
        }
    }

    IEnumerator HideTutorialPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialPanel.SetActive(false);
    }

    void Update()
    {
        if (!tutorialPanel.activeInHierarchy) return;

        switch (step)
        {
            case 0:
                CheckKeysPressed(new List<KeyCode> { KeyCode.A, KeyCode.D, KeyCode.LeftArrow, KeyCode.RightArrow }, moveKeysPressed, () => {
                    step++;
                    ShowNextStep();
                });
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    step++;
                    ShowNextStep();
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    step++;
                    ShowNextStep();
                }
                break;
            case 3:
                CheckMouseClicks(new List<int> { 0, 1 }, mouseClicksPressed, () => {
                    step++;
                    ShowNextStep();
                });
                break;
        }
    }

    // Función para verificar si ambas teclas en la lista fueron presionadas
    private void CheckKeysPressed(List<KeyCode> keys, Dictionary<KeyCode, bool> keyStatus, System.Action onComplete)
    {
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
                keyStatus[key] = true;
        }

        if (keyStatus[KeyCode.A] && keyStatus[KeyCode.D] || keyStatus[KeyCode.LeftArrow] && keyStatus[KeyCode.RightArrow])
        {
            ResetKeyPresses(keyStatus);
            onComplete.Invoke();
        }
    }

    // Función para verificar si ambos clics fueron presionados
    private void CheckMouseClicks(List<int> buttons, Dictionary<int, bool> clickStatus, System.Action onComplete)
    {
        foreach (var button in buttons)
        {
            if (Input.GetMouseButtonDown(button))
                clickStatus[button] = true;
        }

        if (clickStatus[0] && clickStatus[1])
        {
            ResetKeyPresses(clickStatus);
            onComplete.Invoke();
        }
    }

    // Función para restablecer el estado de teclas o clics
    private void ResetKeyPresses<T>(Dictionary<T, bool> statusDictionary)
    {
        var keys = new List<T>(statusDictionary.Keys);
        foreach (var key in keys)
        {
            statusDictionary[key] = false;
        }
    }

    // Función para restablecer el estado de la interfaz de usuario
    private void ResetUI()
    {
        adKeysPanel.SetActive(false);
        mouseKeysPanel.SetActive(false);
        shiftKeyImage.gameObject.SetActive(false);
        spaceKeyImage.gameObject.SetActive(false);
    }
}
