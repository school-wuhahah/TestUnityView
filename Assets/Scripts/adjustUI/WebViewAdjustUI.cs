using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebViewAdjustUI : MonoBehaviour
{

    public Button showBD;
    public GameObject webLayerPb;
    public Transform canvsTf;
    GameObject webLayerObj;

    const string url = @"http://www.baidu.com";

    // Start is called before the first frame update
    void Start()
    {
        showBD.onClick.AddListener(ShowWebLayer);
    }

    private void ShowWebLayer()
    {
        webLayerObj = Instantiate(webLayerPb, canvsTf);
        Button closeBtn = webLayerObj.GetComponentInChildren<Button>();
        closeBtn.onClick.AddListener(() => {
            Destroy(webLayerObj);
        });
        Vector2 btnSize = closeBtn.GetComponent<RectTransform>().sizeDelta; 
        WebViewObject wvobj = webLayerObj.GetComponentInChildren<WebViewObject>();
        wvobj.Init();
        
        int realheight = GetWidgetRealPixel(canvsTf, btnSize.y);
        wvobj.SetMargins(0, 0, 0, realheight);
        wvobj.LoadURL(url.Replace(" ", "%20"));
        wvobj.SetVisibility(true);
    }

    /// <summary>
    /// 获取真实控件像素
    /// </summary>
    /// <param name="canvastf"></param>
    /// <param name="wgtlen"></param>
    /// <returns></returns>
    private int GetWidgetRealPixel(Transform canvastf, float wgtlen)
    {
        CanvasScaler canvasScaler = canvastf.GetComponent<CanvasScaler>();
        Vector2 resSize = canvasScaler.referenceResolution;
        if (canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
        {
            float kLogBase = 2.0f;
            float logwidth = Mathf.Log(Screen.width / resSize.x, kLogBase);
            float logheight = Mathf.Log(Screen.height / resSize.y, kLogBase);
            float logAverage = Mathf.Lerp(logwidth, logheight, canvasScaler.matchWidthOrHeight);
            float factor = Mathf.Pow(kLogBase, logAverage);
            return (int)(factor * wgtlen + 0.5f);
        }
        return 0;
    }
}
