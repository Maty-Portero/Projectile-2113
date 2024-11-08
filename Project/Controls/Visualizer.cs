using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Project.Controls;
using Project;
using System;

public class Visualizer
{
    private Texture2D _texture;
    private SpriteFont _font;
    private Vector2 _position;
    private string _currentText;
    private Color _color;
    public Visualizer(Texture2D texture, SpriteFont font, Vector2 position)
    {
        _texture = texture;
        _font = font;
        _position = position;
        _color = Color.White;
        _currentText = string.Empty;
    }

    /*public void Update(GameTime gameTime, Konami konami)
    {
        
        // Aquí podrías actualizar el estado del visualizer dependiendo del estado del juego.
        // Por ejemplo, si el Konami code se activa, podrías cambiar la información mostrada.
    }*/

    public void Draw(int estado, SpriteBatch spriteBatch, bool konamiS, Leaderboard leaderboard)
    {
        // Dibuja el visualizer (textura) y la información asociada
        spriteBatch.Draw(_texture, _position, _color);
       
        // Dibuja el texto actual
        SetVisualizerMode(estado, spriteBatch, konamiS, leaderboard);
    }

    public void SetText(string text)
    {
        _currentText = text;
    }

    public void SetColor(Color color)
    {
        _color = color;
    }

    public void SetVisualizerMode(int mode, SpriteBatch spritebatch, bool konamiS, Leaderboard leaderboard)
    {
        // Cambiar lo que se muestra en función del "mode"
        switch (mode)
        {
            case 0:
                spritebatch.DrawString(_font, "Versus solo prueba", new Vector2(1500 + 50, 900), Color.Red);
                if (konamiS)
                {
                    spritebatch.DrawString(_font, "Konami ON", new Vector2(1500 + 60, 800), Color.Green);
                    
                }
                else
                {
                    spritebatch.DrawString(_font, "Konami OFF", new Vector2(1500 + 60, 800), Color.White);
                }
                break;
            case 1:
                // Mostrar el leaderboard
                var topEntries = leaderboard.GetTopEntries();
                var lastEntries = leaderboard.GetLastEntries();
                float yOffset = 300;
                // Encabezado para las mejores puntuaciones
                spritebatch.DrawString(_font, "TOP SCORES", _position + new Vector2(20, yOffset - 30), Color.White);
                for (int i = 0; i < Math.Min(10, topEntries.Count); i++)
                {
                    var entry = topEntries[i];
                    spritebatch.DrawString(_font, $"{i + 1}. {entry.PlayerID} : {entry.Mode} - {entry.Score}", _position + new Vector2(20, yOffset), Color.White);
                    yOffset += 30;
                }

                // Encabezado para las puntuaciones más recientes
                yOffset += 60; // Espacio entre los dos bloques
                spritebatch.DrawString(_font, "LATEST SCORES", _position + new Vector2(20, yOffset - 30), Color.White);
                for (int i = 0; i < Math.Min(10, lastEntries.Count); i++)
                {
                    var entry = lastEntries[i];
                    spritebatch.DrawString(_font, $"{i + 1}. {entry.PlayerID} : {entry.Mode} - {entry.Score}", _position + new Vector2(20, yOffset), Color.White);
                    yOffset += 30;
                }
                break;
            case 2:
                _currentText = "Kills: 10\nScore: 500\nTime: 02:30";
                break;
            default:
                _currentText = "Default Info";
                break;
        }
    }
}
