using UnityEngine;
using UnityEngine.EventSystems;

namespace Cainos.PixelArtMonster_Dungeon
{
    public class MonsterInputMouseAndKeyboard : MonoBehaviour
    {
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;

        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode moveModifierKey = KeyCode.LeftShift;
        public KeyCode attackKey = KeyCode.Mouse0;

        private MonsterFlyingController controllerFlying;

        private Vector2 inputMove;
        private bool inputMoveModifier;
        private bool inputJump;
        private bool inputAttack;

        private void Awake()
        {
            // Este script no necesita referenciar MonsterController, solo el controlador de vuelo
            controllerFlying = GetComponent<MonsterFlyingController>();
        }

        private void Update()
        {
            bool pointerOverUI = EventSystem.current && EventSystem.current.IsPointerOverGameObject();
            if (!pointerOverUI)
            {
                inputMoveModifier = Input.GetKey(moveModifierKey);
                inputJump = Input.GetKey(jumpKey);
                inputAttack = Input.GetKeyDown(attackKey);

                // Si el controlador de vuelo está presente, procesa las entradas
                if (controllerFlying)
                {
                    controllerFlying.inputAttack = inputAttack;
                }
            }

            // Move horizontal
            if (Input.GetKey(leftKey) || Input.GetKey(KeyCode.LeftArrow))
            {
                inputMove.x = -1.0f;
            }
            else if (Input.GetKey(rightKey) || Input.GetKey(KeyCode.RightArrow))
            {
                inputMove.x = 1.0f;
            }
            else
            {
                inputMove.x = 0.0f;
            }

            // Move vertical
            if (Input.GetKey(downKey) || Input.GetKey(KeyCode.DownArrow))
            {
                inputMove.y = -1.0f;
            }
            else if (Input.GetKey(upKey) || Input.GetKey(KeyCode.UpArrow))
            {
                inputMove.y = 1.0f;
            }
            else
            {
                inputMove.y = 0.0f;
            }

            if (controllerFlying) controllerFlying.inputMove = inputMove;
        }
    }
}
