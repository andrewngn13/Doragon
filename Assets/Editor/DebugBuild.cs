using UnityEngine;
using UnityEditor;
using Cysharp.Text;

public class DebugBuild : EditorWindow
{
    private string buildPath;
    [MenuItem("Doragon/Build Debug")]
    private static void BuildDebug()
    {
        EditorWindow window = GetWindow<DebugBuild>();
        window.position = new Rect(50f, 50f, 200f, 24f);
        window.titleContent = new GUIContent("Build Debug");
        window.Show();

        //string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        //string[] levels = new string[] { "Assets/Scene1.unity", "Assets/Scene2.unity" };
        //BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    private void OnGUI()
    {
        GenericMenu menu = new GenericMenu();
        GUILayout.Button("Build Windows Standalone");
    }

    // TODO: Configure debug settings of windows standalone
    private void WindowsStandalone()
    {

    }
}