//****************//
//Global Variables//
//****************//

public static class Globals
{
    public static string BuildString(params string[] buttons) {
        string s = "";
        for (int i = 0; i < buttons.Length; i += 2) {
            s = s + buttons[i] + ": " + buttons[i + 1] + "\n";
        }
        return s;
    }

    public static string WORLDPATH;
    public static string BACKUPS;
    public static string TODPATH;
    public static string LEGACYPATH;

    public static string FAVPATH;
    public static string BUNDLEPATH;
    public static string TEMPFOLDER;

    public static string CAMERA_CONTROL = "Up/Down: Adjust Zoom\nLeft/Right: Adjust Rotation";
    public static string SELECT_CONTROL = "Action: Pickup Selected\n" +
                                                "Back: Ignore Selected\n" +
                                                "Up/Down: Not Used\n" +
                                                "Left/Right: Coming Soon\n" +
                                                "Shortcut: Activate Object";
    public static string CYCLE_CONTROL = "Action: Pickup Selected\n" +
                                                "Back: Exit Cycle Select\n" +
                                                "Up/Down: Not Used\n" +
                                                "Left/Right: Cycle Selected\n" +
                                                "Shortcut: Activate Object";
    public static string PLACE_CONTROL = "Action: Stamp Copy\n" +
                                                "Back: Delete Object\n" +
                                                "Up/Down: Scale Object\n" +
                                                "Left/Right: Rotate Object\n" +
                                                "Shortcut: Drop Object";
    public static string PLACE_CONTROL_2 = "Action: Pose Detail\n" +
                                                "Back: Delete Object\n" +
                                                "Up/Down: Scale Object\n" +
                                                "Left/Right: Change Start Frame\n" +
                                                "Shortcut: Drop Object";
    public static string POSE_CONTROL = "Action: Toggle Joint Rotation\n" +
                                                "Back: Exit Pose Detail\n" +
                                                "Up/Down: Coming Soon\n" +
                                                "Left/Right: Coming Soon\n" +
                                                "Shortcut: Pickup Object (Left Hand)";

    
}
