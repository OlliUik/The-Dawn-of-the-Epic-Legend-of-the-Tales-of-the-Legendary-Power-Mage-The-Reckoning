using UnityEngine;

namespace PowerMage
{
    public class InputPlayer : MonoBehaviour, IInput
    {
        [SerializeField] private string[] inputMoveHorizontal, inputMoveVertical, inputLookHorizontal, inputLookVertical, inputJump, inputDash, inputAttack = null;
        
        public InputContainer GetInput()
        {
            InputContainer input = new InputContainer(
                new Vector2(GetFloat(inputMoveHorizontal), GetFloat(inputMoveVertical)),
                new Vector2(GetFloat(inputLookHorizontal), GetFloat(inputLookVertical)),
                GetBool(inputJump),
                GetBool(inputDash),
                GetBool(inputAttack)
                );
            return input;
        }

        private bool GetBool(string[] input)
        {
            bool output = false;
            foreach (string s in input)
            {
                bool i = Input.GetButton(s);
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
    }
}
