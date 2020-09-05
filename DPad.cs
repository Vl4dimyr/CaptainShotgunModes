using UnityEngine;

namespace CaptainShotgunModes
{
    enum DPadInput { Left, Right, Up, Down, Count }
    enum DPadAxis { Horizontal, Vertical, Count }

    class DPad
    {
        private static bool[] inputDownList = new bool [(int)DPadInput.Count];
        private static bool[] inputUpList = new bool [(int)DPadInput.Count];
        private static float[] axisValueList = new float [(int)DPadAxis.Count];

        public static void Update()
        {
            // TODO: this might only work with xbox 360 and xbox one contollers
            float horizontal = Input.GetAxis("Joy1Axis6");
            float vertical = Input.GetAxis("Joy1Axis7");

            inputDownList[(int)DPadInput.Left] = horizontal < 0f && axisValueList[(int)DPadAxis.Horizontal] == 0f;
            inputDownList[(int)DPadInput.Right] = horizontal > 0f && axisValueList[(int)DPadAxis.Horizontal] == 0f;
            inputDownList[(int)DPadInput.Up] = vertical > 0f && axisValueList[(int)DPadAxis.Vertical] == 0f;
            inputDownList[(int)DPadInput.Down] = vertical < 0f && axisValueList[(int)DPadAxis.Vertical] == 0f;

            inputUpList[(int)DPadInput.Left] = horizontal == 0f && axisValueList[(int)DPadAxis.Horizontal] < 0f;
            inputUpList[(int)DPadInput.Right] = horizontal == 0f && axisValueList[(int)DPadAxis.Horizontal] > 0f;
            inputUpList[(int)DPadInput.Up] = vertical == 0f && axisValueList[(int)DPadAxis.Vertical] > 0f;
            inputUpList[(int)DPadInput.Down] = vertical == 0f && axisValueList[(int)DPadAxis.Vertical] < 0f;

            axisValueList[(int)DPadAxis.Horizontal] = horizontal;
            axisValueList[(int)DPadAxis.Vertical] = vertical;
        }

        public static bool GetInputDown(DPadInput input)
        {
            if (input < 0 || input >= DPadInput.Count) {
                return false;
            }

            return inputDownList[(int)input];
        }

        public static bool GetInputUp(DPadInput input)
        {
            if (input < 0 || input >= DPadInput.Count) {
                return false;
            }

            return inputUpList[(int)input];
        }

        public static float GetAxis(DPadAxis axis)
        {
            if (axis < 0 || axis >= DPadAxis.Count) {
                return 0f;
            }

            return axisValueList[(int)axis];
        }
    }
}
