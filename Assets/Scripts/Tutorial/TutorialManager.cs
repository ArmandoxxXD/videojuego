using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
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

    public CinemachineVirtualCamera cinematicCamera; // Cámara virtual para la cinemática
    public CinemachineVirtualCamera playerCamera; // Cámara virtual del jugador

    private int step = 0;
    public bool tutorialCompleted = false;

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
                tutorialText.text = "Presiona SHIFT para hacer un dash.\nPodrás pasar por lugares estrechos.";
                ResetUI();
                shiftKeyImage.gameObject.SetActive(true);
                break;
            case 3:
                tutorialText.text = "Presiona clic izquierdo para atacar y clic derecho para bloquear";
                ResetUI();
                mouseKeysPanel.SetActive(true);
                break;
            case 4:
                StartCoroutine(PlayCinematicAndProceed());
                break;
            case 5:
                tutorialText.text = "¡Buen trabajo! Has completado el tutorial.\nVe hacia la bandera para continuar.";
                ResetUI();
                StartCoroutine(CompleteTutorial());
                break;
        }
    }

    IEnumerator PlayCinematicAndProceed()
    {
        tutorialPanel.SetActive(false); // Ocultar el panel temporalmente
        yield return new WaitForSeconds(1f); // Pequeña pausa antes de la cinemática

        // Activar la cinemática
        if (cinematicCamera != null)
        {
            cinematicCamera.Priority = 10; // Aumentar prioridad para hacerla activa
            playerCamera.Priority = 5;    // Reducir la prioridad de la cámara del jugador
            yield return new WaitForSeconds(3f); // Duración de la cinemática
            cinematicCamera.Priority = 5; // Restaurar prioridad original
            playerCamera.Priority = 10;   // Reactivar la cámara del jugador
        }

        tutorialPanel.SetActive(true); // Mostrar el panel nuevamente
        step++; // Avanzar al siguiente paso
        ShowNextStep(); // Mostrar el texto del siguiente paso
    }

    IEnumerator CompleteTutorial()
    {
        yield return new WaitForSeconds(3f);
        tutorialPanel.SetActive(false);
        tutorialCompleted = true;
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

    private void ResetKeyPresses<T>(Dictionary<T, bool> statusDictionary)
    {
        var keys = new List<T>(statusDictionary.Keys);
        foreach (var key in keys)
        {
            statusDictionary[key] = false;
        }
    }

    private void ResetUI()
    {
        adKeysPanel.SetActive(false);
        mouseKeysPanel.SetActive(false);
        shiftKeyImage.gameObject.SetActive(false);
        spaceKeyImage.gameObject.SetActive(false);
    }
}
