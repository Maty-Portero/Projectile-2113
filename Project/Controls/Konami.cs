using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Project.Controls
{
    internal class Konami
    {
        private readonly List<Keys> konamiKeys = new List<Keys>
        {
            Keys.Up, Keys.Up, Keys.Down, Keys.Down,
            Keys.Left, Keys.Right, Keys.Left, Keys.Right,
            Keys.B, Keys.A
        };

        private int currentIndex = 0;
        bool success = false;
        public bool Success { get { return success; } set { success = value; } }

        private KeyboardState previousKeyState;

        public void Read()
        {
            if (Success)
                return; // Si el código ya fue ingresado correctamente, no hace falta verificar más

            KeyboardState currentKeyState = Keyboard.GetState();

            // Verifica si la tecla actual en la secuencia se presionó y solo cuenta el cambio de "no presionada" a "presionada"
            if (currentKeyState.IsKeyDown(konamiKeys[currentIndex]) && previousKeyState.IsKeyUp(konamiKeys[currentIndex]))
            {
                // Avanza al siguiente en la secuencia
                currentIndex++;

                // Si se completó la secuencia
                if (currentIndex == konamiKeys.Count)
                {
                    Success = true;
                    currentIndex = 0; // Reiniciar el índice para permitir múltiples activaciones si es necesario
                }
            }
            // Si se presiona cualquier tecla incorrecta, reinicia la secuencia
            else if (currentKeyState.GetPressedKeys().Length > 0 && currentKeyState != previousKeyState)
            {
                currentIndex = 0;
            }

            // Guarda el estado del teclado para la próxima verificación
            previousKeyState = currentKeyState;
        }

        public void Reset()
        {
            Success = false;
            currentIndex = 0;
        }
    }
}
