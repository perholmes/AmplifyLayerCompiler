using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AmplifyLayerCompiler : MonoBehaviour
{
    static void Compile(int numLayers)
    {
        var functions = new List<AmplifyShaderFunction>();
        const string templateSuffix = "Layer Template";
        const string generatedLayerName = "Layer";

        // Scan selection. Filename must end in "Layer Template" and be of type AmplifyShaderFunction.
        foreach (var obj in Selection.objects) {
            if (obj.GetType() == typeof(AmplifyShaderFunction) && obj.name.EndsWith(templateSuffix)) {
                functions.Add(obj as AmplifyShaderFunction);
            }
        }

        var projectFolder = Directory.GetParent(Application.dataPath); // Parent of Assets folder

        // Iterate selected files
        foreach (var obj in functions) {
            var objPath = AssetDatabase.GetAssetPath(obj);
            var objFolder = Directory.GetParent(objPath);
            var objFilename = Path.GetFileName(objPath);
            var objFilenameBase = objFilename.Substring(0, objFilename.IndexOf(templateSuffix)).Trim();

            var templateCode = System.IO.File.ReadAllText(objPath);

            // Iterate layers. Replace _LAYERNUM and write into separate files. Layer number 0 gets
            // replaced with nothing. This way, the first layer gets a clean version of all names,
            // and e.g. _BaseMap only becomes _BaseMap1 when it's used a second time. This makes
            // layered shaders more compatible with changing to other shaders, because at least
            // layer 0 has common names for things.
            for (var layer = 0; layer < 8; layer++) {
                var layerName = objFilenameBase + " " + generatedLayerName + " " + layer.ToString();
                var layerPath = objFolder + "/" + layerName + ".asset";

                if (layer < numLayers) {
                    var layerNum = (layer == 0) ? "" : layer.ToString();

                    var layerCode = templateCode;
                    layerCode = layerCode.Replace("_LAYERNUM", layerNum);
                    layerCode = layerCode.Replace(obj.name, layerName);

                    System.IO.File.WriteAllText(layerPath, layerCode);

                    Debug.Log($"Created {layerName} from {obj.name}");
                } else {
                    if (File.Exists(layerPath)) {
                        File.Delete(layerPath);
                    }
                }
            }
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Amplify Layer Compiler/Compile to 2 layers")]
    static void Compile1Layer()
    {
        Compile(2);
    }

    [MenuItem("Assets/Amplify Layer Compiler/Compile to 3 layers")]
    static void Compile2Layers()
    {
        Compile(3);
    }

    [MenuItem("Assets/Amplify Layer Compiler/Compile to 4 layers")]
    static void Compile3Layers()
    {
        Compile(4);
    }

    [MenuItem("Assets/Amplify Layer Compiler/Compile to 5 layers")]
    static void Compile4Layers()
    {
        Compile(5);
    }

    [MenuItem("Assets/Amplify Layer Compiler/Compile to 6 layers")]
    static void Compile5Layers()
    {
        Compile(6);
    }

    [MenuItem("Assets/Amplify Layer Compiler/Compile to 7 layers")]
    static void Compile6Layers()
    {
        Compile(7);
    }

    [MenuItem("Assets/Amplify Layer Compiler/Compile to 8 layers")]
    static void Compile7Layers()
    {
        Compile(8);
    }
}
