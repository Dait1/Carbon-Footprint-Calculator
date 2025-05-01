using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarbonCalculator : MonoBehaviour
{
    public GameObject home;
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;
    public GameObject page5;
    public GameObject page6;
    public TMP_InputField kilometersInput;
    public TMP_InputField electricityInput;
    public TMP_InputField meatKgInput;
    public TMP_InputField publicTransportInput;
    public TMP_InputField airTravelInput;
    public TMP_Text resultText;
    public TMP_Text earthsText;
    public TMP_Text recommendationsText;
    public Button startButton;
    public Button nextButton1;
    public Button backButton2;
    public Button nextButton2;
    public Button backButton3;
    public Button nextButton3;
    public Button backButton4;
    public Button nextButton4;
    public Button backButton5;
    public Button nextButton5;
    public Button calculateButton;

    private float kilometers;
    private float electricity;
    private float meatKg;
    private float publicTransport;
    private float airTravel;
    private float carCO2PerKm = 0.250f;
    private float electricityCO2PerkWh = 0.400f;
    private float meatCO2PerKg = 14.5f;
    private float publicTransportCO2PerKm = 0.100f;
    private float airTravelCO2PerKm = 0.200f;
    private const float sustainableCO2ePerWeek = 0.0404f;

    void Start()
    {
        home.GetComponent<CanvasGroup>().alpha = 1;
        page1.GetComponent<CanvasGroup>().alpha = 0;
        page2.GetComponent<CanvasGroup>().alpha = 0;
        page3.GetComponent<CanvasGroup>().alpha = 0;
        page4.GetComponent<CanvasGroup>().alpha = 0;
        page5.GetComponent<CanvasGroup>().alpha = 0;
        page6.GetComponent<CanvasGroup>().alpha = 0;
        home.SetActive(true);
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(false);
        page4.SetActive(false);
        page5.SetActive(false);
        page6.SetActive(false);

        startButton.onClick.AddListener(() => FadeTo(home, page1, GoToPage1FromHome));
        nextButton1.onClick.AddListener(() => FadeTo(page1, page2, GoToPage2));
        backButton2.onClick.AddListener(() => FadeTo(page2, page1, GoToPage1));
        nextButton2.onClick.AddListener(() => FadeTo(page2, page3, GoToPage3));
        backButton3.onClick.AddListener(() => FadeTo(page3, page2, BackToPage2));
        nextButton3.onClick.AddListener(() => FadeTo(page3, page4, GoToPage4));
        backButton4.onClick.AddListener(() => FadeTo(page4, page3, BackToPage3));
        nextButton4.onClick.AddListener(() => FadeTo(page4, page5, GoToPage5));
        backButton5.onClick.AddListener(() => FadeTo(page5, page4, BackToPage4));
        nextButton5.onClick.AddListener(() => FadeTo(page5, page6, GoToPage6));
        calculateButton.onClick.AddListener(CalculateFootprint);
    }

    void FadeTo(GameObject fromPage, GameObject toPage, System.Action onComplete)
    {
        StartCoroutine(FadeCoroutine(fromPage, toPage, onComplete));
    }

    System.Collections.IEnumerator FadeCoroutine(GameObject fromPage, GameObject toPage, System.Action onComplete)
    {
        CanvasGroup fromGroup = fromPage.GetComponent<CanvasGroup>();
        CanvasGroup toGroup = toPage.GetComponent<CanvasGroup>();
        toPage.SetActive(true);
        float time = 0;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            fromGroup.alpha = 1 - (time / 0.5f);
            toGroup.alpha = time / 0.5f;
            yield return null;
        }
        fromPage.SetActive(false);
        fromGroup.alpha = 0;
        toGroup.alpha = 1;
        onComplete();
    }

    void GoToPage1FromHome() { }
    void GoToPage2() { kilometers = float.TryParse(kilometersInput.text, out float km) ? km : 0; }
    void GoToPage1() { }
    void GoToPage3() { electricity = float.TryParse(electricityInput.text, out float e) ? e : 0; }
    void BackToPage2() { }
    void GoToPage4() { meatKg = float.TryParse(meatKgInput.text, out float mkg) ? mkg : 0; }
    void BackToPage3() { }
    void GoToPage5() { publicTransport = float.TryParse(publicTransportInput.text, out float pt) ? pt : 0; }
    void BackToPage4() { }
    void GoToPage6() { airTravel = float.TryParse(airTravelInput.text, out float at) ? at : 0; }

    void CalculateFootprint()
    {
        float transportCO2 = kilometers * carCO2PerKm;
        float energyCO2 = (electricity / 4) * electricityCO2PerkWh;
        float dietCO2 = meatKg * meatCO2PerKg;
        float publicTransportCO2 = publicTransport * publicTransportCO2PerKm;
        float airTravelCO2 = (airTravel / 52) * airTravelCO2PerKm;
        float totalCO2Kg = transportCO2 + energyCO2 + dietCO2 + publicTransportCO2 + airTravelCO2;
        float totalCO2Tons = totalCO2Kg / 1000;

        resultText.text = $"Result: {totalCO2Tons:F4} tons CO2e/week";

        float earthsNeeded = totalCO2Tons / sustainableCO2ePerWeek;
        earthsText.text = $"Earths Needed: {earthsNeeded:F2} (if everyone lived like you)";

        string recommendations = "Ways to Lower Your Footprint:\n";
        if (kilometers > 50) recommendations += "- Reduce car use; try walking or biking for short trips.\n";
        if (electricity > 200) recommendations += "- Switch to energy-efficient appliances or renewable energy.\n";
        if (meatKg > 0.5) recommendations += "- Cut back on meat; try plant-based meals a few days a week.\n";
        if (publicTransport < 10) recommendations += "- Use public transport more to lower car emissions.\n";
        if (airTravel > 1000) recommendations += "- Limit air travel or offset flights with carbon credits.\n";
        if (totalCO2Tons <= sustainableCO2ePerWeek) recommendations += "- Great job! Your footprint is sustainable.\n";
        recommendationsText.text = recommendations;

        StartCoroutine(RevealResult());
    }

    System.Collections.IEnumerator RevealResult()
    {
        CanvasGroup resultGroup = page6.GetComponent<CanvasGroup>();
        float time = 0;
        while (time < 0.3f)
        {
            time += Time.deltaTime;
            resultText.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, time / 0.3f);
            earthsText.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, time / 0.3f);
            recommendationsText.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, time / 0.3f);
            yield return null;
        }
    }
}
