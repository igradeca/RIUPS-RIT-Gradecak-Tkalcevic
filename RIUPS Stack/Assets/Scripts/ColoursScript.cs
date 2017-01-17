using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoursScript : MonoBehaviour {

    public ColourElementScript[] tileColours = new ColourElementScript[3];

    void Start() {

        tileColours[0] = new ColourElementScript();
        tileColours[0].coloursPallette[0] = new Color32(68, 0, 236, 0);
        tileColours[0].coloursPallette[1] = new Color32(68, 98, 236, 0);
        tileColours[0].coloursPallette[2] = new Color32(68, 171, 236, 0);
        tileColours[0].coloursPallette[3] = new Color32(68, 211, 205, 0);

        tileColours[1] = new ColourElementScript();
        tileColours[1].coloursPallette[0] = new Color32(236, 50, 0, 0);
        tileColours[1].coloursPallette[1] = new Color32(42, 218, 130, 0);
        tileColours[1].coloursPallette[2] = new Color32(82, 206, 134, 0);
        tileColours[1].coloursPallette[3] = new Color32(144, 233, 174, 0);

        tileColours[2] = new ColourElementScript();
        tileColours[2].coloursPallette[0] = new Color32(236, 0, 0, 0);
        tileColours[2].coloursPallette[1] = new Color32(242, 43, 43, 0);
        tileColours[2].coloursPallette[2] = new Color32(227, 85, 85, 0);
        tileColours[2].coloursPallette[3] = new Color32(223, 144, 144, 0);

    }

}
