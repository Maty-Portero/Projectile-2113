using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.States;
using System.Collections.Generic;

namespace Project.Controls
{
    public abstract class NavigableState : State
    {
        protected List<Component> _components;
        protected int _selectedIndex;
        protected int _limitIndex;
        private bool _keyPressed; // Para controlar si una tecla de navegación está presionada
        private KeyboardState _currentKeyboardState; // Estado actual del teclado
        private KeyboardState _previousKeyboardState; // Estado previo del teclado
        private GraphicsDeviceManager _graphics;
        public NavigableState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager)
            : base(game, graphicsDevice, content, deviceManager)
        {
            _graphics = deviceManager;

            _components = new List<Component>();
            _selectedIndex = 0;
            _limitIndex = 0;
            _currentKeyboardState = Keyboard.GetState();
            _previousKeyboardState = _currentKeyboardState;
        }

        protected void HandleNavigation()
        {
            // Obtener el estado del teclado actual
            _currentKeyboardState = Keyboard.GetState();

            // Navegación con teclas W/S o Arriba/Abajo
            if ((_currentKeyboardState.IsKeyDown(Keys.W) || _currentKeyboardState.IsKeyDown(Keys.Up)) && !_keyPressed)
            {
                _selectedIndex--;
                if (_selectedIndex < 0) _selectedIndex = _components.Count - (_limitIndex + 1);
                _keyPressed = true;
            }
            else if ((_currentKeyboardState.IsKeyDown(Keys.S) || _currentKeyboardState.IsKeyDown(Keys.Down)) && !_keyPressed)
            {
                _selectedIndex++;
                if (_selectedIndex >= _components.Count-_limitIndex) _selectedIndex = 0;
                _keyPressed = true;
            }
            else if (_currentKeyboardState.IsKeyUp(Keys.W) && _currentKeyboardState.IsKeyUp(Keys.S) &&
                     _currentKeyboardState.IsKeyUp(Keys.Up) && _currentKeyboardState.IsKeyUp(Keys.Down))
            {
                _keyPressed = false;
            }

            // Seleccionar con Enter solo si antes estaba liberada
            if (_currentKeyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                var selectedButton = _components[_selectedIndex] as Button;
                selectedButton?.PerformClick();
            }

            // Volver con B hacia atrás
            if (_currentKeyboardState.IsKeyDown(Keys.B) && _previousKeyboardState.IsKeyUp(Keys.B))
            {
                if (_game.estado == 1 || _game.estado == 5 || _game.estado == 6)
                {
                    _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
                }
            }

            // Actualizar el estado previo del teclado
            _previousKeyboardState = _currentKeyboardState;
        }

        protected void HighlightSelectedButton()
        {
            foreach (var component in _components)
            {
                if (component is Button)
                {
                    var button = component as Button;
                    if (_components.IndexOf(component) == _selectedIndex)
                        button.colour = Color.Yellow; // Resaltar
                    else
                        button.colour = Color.White;  // Normal
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleNavigation();

            foreach (var component in _components)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            HighlightSelectedButton();

            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
