using UnityEngine;

namespace PowerMage
{
    public struct InputContainer
    {
        public Vector2 moveVector, lookVector;
        public bool jump, dash, attack;

        public InputContainer(Vector2 move, Vector2 look, bool j, bool d, bool a)
        {
            moveVector = Vector2.ClampMagnitude(move, 1.0f);
            lookVector = Vector2.ClampMagnitude(look, 1.0f);
            jump = j;
            dash = d;
            attack = a;
        }
    }
}
