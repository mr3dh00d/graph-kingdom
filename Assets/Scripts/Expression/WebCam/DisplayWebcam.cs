using UnityEngine;
using UnityEngine.UI;

public class DisplayWebcam : MonoBehaviour
{
    RawImage rawImage;
    AspectRatioFitter fitter;
    WebCamTexture webcamTexture;
    //bool ratioSet;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        //fitter = GetComponent<AspectRatioFitter>();
        InitWebCam();
    }

    void Update()
    {

        /*if (webcamTexture.width > 100 && !ratioSet)
        {
            ratioSet = true;
            SetAspectRatio();
        }*/
    }

    /*void SetAspectRatio()
    {
        fitter.aspectRatio = (float)webcamTexture.width / (float)webcamTexture.height;
        Debug.Log("from Display " + webcamTexture.height + ", " + webcamTexture.width);
    }*/

    void InitWebCam()
    {
        string camName = WebCamTexture.devices[0].name;
        webcamTexture = new WebCamTexture(camName);
        rawImage.texture = webcamTexture;
        webcamTexture.Play();
        // @todo activar logs
        // Debug.Log("from Display " + webcamTexture.height + ", " + webcamTexture.width);
        // Debug.Log("from Display " + Screen.height + ", " + Screen.width);
    }

    public WebCamTexture GetCamImage()
    {
        return webcamTexture;
    }
}
