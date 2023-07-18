using UnityEngine;
using UnityEngine.InputSystem;

namespace Drone.Scripts.GamePlay
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        #region Variables

        private Vector2 _cyclic;
        private float _pedals;
        private float _throttle;

        public Vector2 Cyclic => _cyclic;
        public float Pedals => _pedals;
        public float Throttle => _throttle;

        /// <summary>
        /// 左轴输入
        /// </summary>
        public Vector2 leftAxisOffset;
        /// <summary>
        /// 右轴输入
        /// </summary>
        public Vector2 rightAxisOffset;

        #endregion

        #region Main Methods

        // Update is called once per frame
        void Update()
        {
            if (!Application.isFocused) return;
            if (Joystick.current != null)
            {
                leftAxisOffset.x = Input.GetAxis("Left_X");
                leftAxisOffset.y = Input.GetAxis("Left_Y");

                rightAxisOffset.x = Input.GetAxis("Right_X");
                rightAxisOffset.y = Input.GetAxis("Right_Y");

                byte[] mByte = (byte[])Joystick.current.ReadValueAsObject();
                if (mByte.Length == 8)
                {
                    float offset = Mathf.Clamp((mByte[5] - 120) * 0.01F, -1, 1);
                    if (Mathf.Abs(offset) < 0.05f)
                    {
                        offset = 0;
                    }
                    leftAxisOffset.x = offset;

                    offset = Mathf.Clamp((mByte[3] - 120) * 0.01F, -1, 1);
                    if (Mathf.Abs(offset) <= 0.05f)
                    {
                        offset = 0;
                    }
                    leftAxisOffset.y = offset;

                    offset = Mathf.Clamp((mByte[1] - 120) * 0.01F, -1, 1);
                    if (Mathf.Abs(offset) <= 0.05f)
                    {
                        offset = 0;
                    }
                    rightAxisOffset.x = offset;

                    offset = Mathf.Clamp((mByte[2] - 120) * 0.01F, -1, 1);
                    if (Mathf.Abs(offset) <= 0.05f)
                    {
                        offset = 0;
                    }
                    rightAxisOffset.y = offset;
                }

                _cyclic.x = rightAxisOffset.x;
                _cyclic.y = rightAxisOffset.y;

                _throttle = leftAxisOffset.y;

                _pedals = leftAxisOffset.x;
            }
        }

        #endregion

        #region Input Methods

        private void OnCyclic(InputValue value)
        {
            _cyclic = value.Get<Vector2>();

        }

        private void OnPedals(InputValue value)
        {
            _pedals = value.Get<float>();
        }

        private void OnThrottle(InputValue value)
        {
            _throttle = value.Get<float>();
        }
        #endregion
    }
}