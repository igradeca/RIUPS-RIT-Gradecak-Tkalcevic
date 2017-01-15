using UnityEngine;
using System.Collections;

public class AchievementElementScript : AchievementScript {

    public string title;
    public string description;
    public bool status;
    public int points;

    public AchievementElementScript() {
        status = false;
    }

}
