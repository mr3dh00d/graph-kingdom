using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

using System.Linq;

using UnityEngine.UI;

public class PredictorIIFerNet : MonoBehaviour
{
    const int IMAGE_SIZE = 48;
    const string INPUT_NAME = "conv2d_input";
    const string OUTPUT_NAME = "dense_1";
    //const string INPUT_NAME = "imagenes";
    //const string OUTPUT_NAME = "emociones";
    //const string INPUT_NAME = "input";
    //const string OUTPUT_NAME = "output";

    readonly List<string> OutputLabels = new List<string>() { "angry", "disgust", "fear", "happy", "neutral", "sad", "surprise" };

    public DisplayWebcam CameraView;//
    public PreprocessIIFerNet preprocessII;//
    public NNModel modelFile;
    public Text uiText;

    public float emotionCooldown = 20f;

    IWorker worker;

    void Start()
    {
        var model = ModelLoader.Load(modelFile);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    void Update()
    {

        WebCamTexture webCamTexture = CameraView.GetCamImage();
        //Debug.Log("webcam height " + webCamTexture.height + ", webcam width " + webCamTexture.width);

        if (webCamTexture.didUpdateThisFrame && webCamTexture.width > 100)
        {
            preprocessII.ScaleAndCropImage(webCamTexture, IMAGE_SIZE, RunModel);
        }

    }

    private void OnDestroy()
    {
        worker.Dispose();
    }


    void RunModel(byte[] pixels)
    {
        StartCoroutine(RunModelRoutine(pixels));//<-----------
    }

    IEnumerator RunModelRoutine(byte[] pixels)
    {
        Tensor tensor = TransformInput(pixels);

        var inputs = new Dictionary<string, Tensor> {
            { INPUT_NAME, tensor }
        };

        worker.Execute(inputs);//<--------
        Tensor outputTensor = worker.PeekOutput(OUTPUT_NAME);

        //get largest output
        List<float> temp = outputTensor.ToReadOnlyArray().ToList();
        float max = temp.Max();
        int index = temp.IndexOf(max);

        //set UI text
        uiText.text = OutputLabels[index];
        // Debug.Log("La emocion de FerNet " + OutputLabels[index]);

        //dispose tensors
        tensor.Dispose();
        outputTensor.Dispose();

        yield return null;
    }

    //This model requires a greyscale image
    Tensor TransformInput(byte[] pixels)
    {
        float[] singleChannel = new float[IMAGE_SIZE * IMAGE_SIZE];
        //Debug.Log("Tama�o del tensor " + singleChannel.Length);
        for (int i = 0; i < singleChannel.Length; i++)
        {
            Color color = new Color32(pixels[i * 3 + 0], pixels[i * 3 + 1], pixels[i * 3 + 2], 255);
            singleChannel[i] = color.grayscale * 255;
        }
        return new Tensor(1, IMAGE_SIZE, IMAGE_SIZE, 1, singleChannel);
    }

}
