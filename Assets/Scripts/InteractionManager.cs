using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore;
using TMPro;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelInfo;
    public GameObject panelColors;
    public GameObject panelQuiz;
    

    [Header("Info Panel UI")]
    public TextMeshProUGUI infoTitleText;
    public TextMeshProUGUI infoDescText;
    public Button infoCloseButton;
    public Button infoChangeColorButton;
    public Button infoQuizButton;

    [Header("Color Panel UI")]
    public Button buttonRed;
    public Button buttonBlue;
    public Button buttonGreen;

    [Header("Quiz Panel UI")]
    public TextMeshProUGUI quizQuestionText;
    public Button quizAnswerButton1;
    public Button quizAnswerButton2;
    public TextMeshProUGUI quizFeedbackText;

    // Référence à l'objet 3D cliqué
    private GameObject currentTarget;

    void Start()
    {
        // Masquer tous les panels au démarrage
        panelInfo.SetActive(false);
        panelColors.SetActive(false);
        panelQuiz.SetActive(false);

        // Liaison des boutons UI à leurs méthodes
        infoCloseButton.onClick.AddListener(CloseInfoPanel);
        infoChangeColorButton.onClick.AddListener(() => ShowColorPanel(currentTarget));
        infoQuizButton.onClick.AddListener(ShowQuiz);


        buttonRed.onClick.AddListener(() => ChangeColor(Color.red));
        buttonBlue.onClick.AddListener(() => ChangeColor(Color.blue));
        buttonGreen.onClick.AddListener(() => ChangeColor(Color.green));

        quizAnswerButton1.onClick.AddListener(() => OnAnswerChosen(true));
        quizAnswerButton2.onClick.AddListener(() => OnAnswerChosen(false));
    }

    void Update()
{
    // Nouveau Input System pour le clic
    if (Mouse.current.leftButton.wasPressedThisFrame
        && !panelInfo.activeSelf
        && !panelColors.activeSelf
        && !panelQuiz.activeSelf)
    {
        // Récupère la position de la souris
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Crée le rayon à partir de la position
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            currentTarget = hit.collider.gameObject;
            if (currentTarget.name.Contains("SF90"))
                ShowQuiz();
            else
                ShowInfoPanel(currentTarget.name);
        }
    }
}

    // --- Méthodes Info ---
    void ShowInfoPanel(string targetName)
    {
        panelInfo.SetActive(true);

        // Selon le nom, on remplit titre + description
        switch (targetName)
        {
            case string s when s.Contains("E36"):
                infoTitleText.text = "BMW E36 M3";
                infoDescText.text  = "Année : 1995\nMoteur : 3.0L—286 ch\nPays : Allemagne";
                break;
            case string s when s.Contains("Mazda"):
                infoTitleText.text = "Mazda RX-7";
                infoDescText.text  = "Année : 1993\nMoteur : 1.3L Rotary—255 ch\nPays : Japon";
                break;
            default:
                infoTitleText.text = targetName;
                infoDescText.text  = "Infos non disponibles.";
                break;
        }
    }

    void CloseInfoPanel()
    {
        panelInfo.SetActive(false);
    }

    // --- Méthodes Couleur ---
    void ShowColorPanel(GameObject target)
    {
        if (target == null) return;
        panelColors.SetActive(true);
    }

    void ChangeColor(Color newColor)
{
    if (currentTarget == null) return;

    // Récupère tous les renderers sur l'objet et ses enfants
    var rends = currentTarget.GetComponentsInChildren<Renderer>();
    foreach (var r in rends)
    {
        // material instancié pour ne pas altérer le mat partagé
        r.material.color = newColor;
    }

    panelColors.SetActive(false);
}

    // --- Méthodes Quiz ---
    void ShowQuiz()
{
    panelQuiz.SetActive(true);
    quizFeedbackText.text = "";

    if (currentTarget.name.Contains("E36"))
    {
        quizQuestionText.text = "Quelle est la puissance de la E36 M3 ?";
        quizAnswerButton1.GetComponentInChildren<TextMeshProUGUI>().text = "286 ch";
        quizAnswerButton2.GetComponentInChildren<TextMeshProUGUI>().text = "200 ch";
    }
    else if (currentTarget.name.Contains("SF90"))
    {
        quizQuestionText.text = "Quelle est la puissance de la SF90 ?";
        quizAnswerButton1.GetComponentInChildren<TextMeshProUGUI>().text = "1000 ch";
        quizAnswerButton2.GetComponentInChildren<TextMeshProUGUI>().text = "500 ch";
    }
    else if (currentTarget.name.Contains("Mazda"))
    {
        quizQuestionText.text = "Quel moteur équipe la Mazda RX-7 ?";
        quizAnswerButton1.GetComponentInChildren<TextMeshProUGUI>().text = "1.3L Rotary";
        quizAnswerButton2.GetComponentInChildren<TextMeshProUGUI>().text = "2.0L 4-cylindres";
    }
}


    void OnAnswerChosen(bool isCorrect)
    {
        quizFeedbackText.text = isCorrect ? "Bravo !" : "Dommage…";
        // Après réponse, on peut désactiver le quiz ou proposer de recommencer.
        // Ici, on le ferme 2s plus tard.
        Invoke(nameof(CloseQuizPanel), 2f);
    }

    void CloseQuizPanel()
    {
        panelQuiz.SetActive(false);
    }
}
