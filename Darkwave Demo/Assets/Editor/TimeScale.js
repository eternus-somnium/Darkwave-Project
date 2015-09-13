#pragma strict

class TimeScale extends EditorWindow {
	@MenuItem ("Window/Time Scale")
    static function ShowWindow () {
        EditorWindow.GetWindow (TimeScale);
    }
    function OnGUI () {
        Time.timeScale = EditorGUILayout.Slider(Time.timeScale,0, 100);
        if(GUILayout.Button("Reset")) Time.timeScale = 1;
    }
}