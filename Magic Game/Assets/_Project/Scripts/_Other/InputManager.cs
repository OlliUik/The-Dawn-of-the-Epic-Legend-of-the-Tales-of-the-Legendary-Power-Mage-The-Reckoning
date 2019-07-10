using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public string controllerType;
    public int controllerId;            //1 = Xbox, 2 = PlayStation

    void Update()
    {
        GetControllerType();
        GetInputMap(controllerId);
    }

    void GetControllerType()
    {
        string[] controllers = Input.GetJoystickNames();

        if (controllers != null)
        {
            foreach (string name in controllers)
            {
                if (name.Contains("Xbox"))
                {
                    controllerType = name;
                    controllerId = 1;
                }

                if (name.Contains("PlayStation") || name.Contains("Wireless Controller"))
                {
                    controllerType = name;
                    controllerId = 2;
                }
            }
        }
    }

    void GetInputMap(int id)
    {
        switch (id)
        {
            case 1:
                return;
            case 2:
                return;
            default:
                break;
        }
    }
}

/*
---XBOX---

            LS X axis = X axis
            LS Y axis = Y axis
            RS X axis = 4th axis
            RS Y axis = 5th axis
            D-pad X axis = 6th axis
            D-pad Y axis = 7th axis
            Triggers? = 3rd axis
            LT = 9th axis
            RT = 10th axis
            A button = Button 0
            B button = Button 1
            X button = Button 2
            Y button = Button 3
            LB = Button 4
            RB = Button 5
            Back button = Button 6
            Start button = Button 7
            LS = Button 8
            RS = Button 9

---PS4---

            LS X axis = X axis
            LS Y axis = Y axis
            RS X axis = 3rd axis
            RS Y axis = 6th axis
            D-pad X axis = 7th axis
            D-pad Y axis = 8th axis
            L2 = 4th axis (-1.0f to 1.0f range, unpressed is -1.0f)
            R2 = 5th axis (-1.0f to 1.0f range, unpressed is -1.0f)
            Square = Button 0
            X = Button 1
            Circle = Button 2
            Triangle = Button 3
            L1 = Button 4
            R1 = Button 5
            L2 = Button 6
            R2 = Button 7
            Share = Button 8
            Options = Button 9
            L3 = Button 10
            R3 = Button 11
            PS = Button 12
            PadPress = Button 13
*/