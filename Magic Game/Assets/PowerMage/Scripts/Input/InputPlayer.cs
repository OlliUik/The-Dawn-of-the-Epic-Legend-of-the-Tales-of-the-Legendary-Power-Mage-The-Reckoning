using UnityEngine;

namespace PowerMage
{
    public class InputPlayer : MonoBehaviour, IInput
    {
        #region VARIABLES

        [SerializeField] private string[] inputMoveHorizontal, inputMoveVertical, inputLookHorizontal, inputLookVertical, inputJump, inputDash, inputAttack = null;

        #endregion

        #region INTERFACE_IMPLEMENTATION

        public InputContainer GetInput()
        {
            InputContainer input = new InputContainer(
                new Vector2(GetFloat(inputMoveHorizontal), GetFloat(inputMoveVertical)),
                new Vector2(GetFloat(inputLookHorizontal), GetFloat(inputLookVertical)),
                GetButton(inputJump, false),
                GetButton(inputDash, false),
                GetButton(inputAttack, false)
                );
            return input;
        }

        #endregion
        
        #region CUSTOM_METHODS

        private bool GetButton(string[] input, bool continuous)
        {
            bool output = false;
            foreach (string s in input)
            {
                bool i = continuous ? Input.GetButton(s) : Input.GetButtonDown(s);
                if (i)
                {
                    output = i;
                    continue;
                }
            }
            return output;
        }

        private float GetFloat(string[] input)
        {
            float output = 0.0f;
            foreach (string s in input)
            {
                float i = Input.GetAxis(s);
                if (!Mathf.Approximately(i, 0.0f))
                {
                    output = i;
                    continue;
                }
            }
            return output;
        }
        
        #endregion
    }
}
