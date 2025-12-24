using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class VisuaManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countCoin;
    [SerializeField] TextMeshProUGUI countBridgeElement;

    [SerializeField] TextMeshProUGUI warningMessageTxt;
    [SerializeField] GameObject warningPanelObj;
    [SerializeField] Image frame;
    [SerializeField] Image currentIcoBridge;
    [SerializeField] ManagerMakeBridge makeManager;

    [SerializeField] LeanTweenType ease;
    string language;

    bool isAcriveWarningPanel = false;
    private void Start()
    {
        language = YG2.envir.language;
    }


    private void OnEnable()
    {
        GameManager.instance.OnUpdateCointCoin += Instance_OnUpdateCointCoin;
        GameEvents.OnSelectedBridge += OnSelectedBridge;
        GameEvents.OnMakeBridgeElement += OnMakeBridgeElement;
        GameEvents.OnDontHaveMany += OnDontHaveMany;
        makeManager.OnWarningMessgeDistance += MakeManager_OnWarningMessgeDistance;
    }

    private void MakeManager_OnWarningMessgeDistance(object sender, float e)
    {
       
        AnimateWarning();
        string msg = language == "ru" ? "Ты слишком далеко от стройки" : " You Daleko";
        string distance = e.ToString("F1");
        warningMessageTxt.text = $"{msg} ({distance}M)";
    }

    private void OnDisable()
    {
        GameManager.instance.OnUpdateCointCoin -= Instance_OnUpdateCointCoin;
        GameEvents.OnSelectedBridge -= OnSelectedBridge;
        GameEvents.OnMakeBridgeElement -= OnMakeBridgeElement;
        GameEvents.OnDontHaveMany -= OnDontHaveMany;
    }

    private void OnDontHaveMany(int obj)
    {
        if (isAcriveWarningPanel) return;
        AnimateWarning();
        string msg = language == "ru" ? "Тебе не хватает" : "Dont Have many";
        string countDontHave = obj.ToString();
        warningMessageTxt.text = $"{msg} {countDontHave}";

    }

    private void OnMakeBridgeElement(int obj)
    {  
        if (isAcriveWarningPanel) return;
        string coint = obj.ToString();
        string max = makeManager.maxMakeElementBridge.ToString();
        countBridgeElement.text = $"{coint}/{max}";
    }

    void AnimateWarning()
    {
        isAcriveWarningPanel = true;
        warningPanelObj.transform.localScale = Vector3.zero;
        warningPanelObj.SetActive(true);
        warningPanelObj.transform.LeanScale(Vector3.one * 1.3f, .2f).setEase(ease).setOnComplete(() =>
        {
            warningPanelObj.LeanScale(Vector3.one, .2f);
        });

        BlinkFrame(frame, 3, 0.25f);
    }

    void BlinkFrame(Image img, int blinkCount, float blinkTime)
    {
        Color red = Color.red;
        Color white = Color.white;

        int totalTweens = blinkCount * 2;

        LeanTween.value(img.gameObject, 0f, 1f, blinkTime)
            .setLoopPingPong(totalTweens)
            .setOnUpdate((float t) =>
            {
                img.color = Color.Lerp(white, red, t);
            })
            .setOnComplete(() =>
            {
                img.color = white;
                warningPanelObj.transform.LeanScale(Vector3.zero, .2f).setEase(ease).setOnComplete(() =>
                {
                    warningPanelObj.SetActive(false);
                    isAcriveWarningPanel = false;
                });
            });
    }

    private void OnSelectedBridge(int arg1, Sprite sprite)
    {
        currentIcoBridge.sprite = sprite;
    }

    private void Instance_OnUpdateCointCoin(object sender, int e)
    {
        countCoin.text = e.ToString();
    }

}
