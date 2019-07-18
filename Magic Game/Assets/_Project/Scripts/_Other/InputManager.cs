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

        if (Input.GetAxis("Mouse X") < -0.2f || Input.GetAxis("Mouse X") > 0.2f)
        {
            //if controller's mouse x is moved, sens is 5
            
        }
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
}

#region NEEDED_MAPPINGS
/*
---XBOX---
            RS = Camera movement (4th axis/5th axis)

            A = Jump (Button 0)

            LB/RB = AOE (Button 4/5)
            LT = Beam (9th axis)
            RT = Projectile (10th axis)

            Start = Menu (Button 7)

---PS4---
            RS = Camera movement (3rd axis/6th axis)

            X = Jump (Button 1)

            L1/R1 = AOE (Button 4/5)
            L2 = Beam (4th axis) (-1.0f to 1.0f range, unpressed is -1.0f)
            R2 = Projectile (5th axis) (-1.0f to 1.0f range, unpressed is -1.0f)

            Options = Menu (Button 9)
*/
#endregion

#region CONTROLLER_SPECIFIC_MAPPINGS
/*
---XBOX---

            LS X axis = X axis          LS Y axis = Y axis
            RS X axis = 4th axis        RS Y axis = 5th axis

            D-pad X axis = 6th axis     D-pad Y axis = 7th axis

            LT = 9th axis               RT = 10th axis
            Triggers? = 3rd axis

            A button = Button 0         B button = Button 1
            X button = Button 2         Y button = Button 3

            LB = Button 4               RB = Button 5
            LS = Button 8               RS = Button 9

            Back button = Button 6      Start button = Button 7

---PS4---

            LS X axis = X axis          LS Y axis = Y axis
            RS X axis = 3rd axis        RS Y axis = 6th axis

            D-pad X axis = 7th axis     D-pad Y axis = 8th axis

            L2 = 4th axis (-1.0f to 1.0f range, unpressed is -1.0f)
            R2 = 5th axis (-1.0f to 1.0f range, unpressed is -1.0f)

            Square = Button 0           X = Button 1
            Circle = Button 2           Triangle = Button 3

            L1 = Button 4               R1 = Button 5
            L2 = Button 6               R2 = Button 7
            L3 = Button 10              R3 = Button 11

            Share = Button 8            Options = Button 9
            PS = Button 12              PadPress = Button 13
*/
#endregion