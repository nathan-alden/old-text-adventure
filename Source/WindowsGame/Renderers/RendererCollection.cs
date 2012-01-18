﻿using System.Collections.ObjectModel;

using Junior.Common;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TextAdventure.WindowsGame.Xna;

namespace TextAdventure.WindowsGame.Renderers
{
	public class RendererCollection
	{
		private readonly Collection<IRenderer> _renderers = new Collection<IRenderer>();

		public void Add(IRenderer renderer)
		{
			renderer.ThrowIfNull("renderer");

			_renderers.Add(renderer);
		}

		public void Remove(IRenderer renderer)
		{
			renderer.ThrowIfNull("renderer");

			_renderers.Remove(renderer);
		}

		public void Render(SpriteBatch spriteBatch, XnaGameTime gameTime, FontContent fontContent, TextureContent textureContent)
		{
			spriteBatch.ThrowIfNull("spriteBatch");
			gameTime.ThrowIfNull("gameTime");
			fontContent.ThrowIfNull("fontContent");
			textureContent.ThrowIfNull("textureContent");

			foreach (IRenderer renderer in _renderers)
			{
				var parameters = new RenderParameters(gameTime, spriteBatch, fontContent, textureContent);

				renderer.Render(parameters);
			}
		}

		private class RenderParameters : IRendererParameters
		{
			private readonly FontContent _fontContent;
			private readonly XnaGameTime _gameTime;
			private readonly SpriteBatch _spriteBatch;
			private readonly TextureContent _textureContent;

			public RenderParameters(XnaGameTime gameTime, SpriteBatch spriteBatch, FontContent fontContent, TextureContent textureContent)
			{
				_gameTime = gameTime;
				_spriteBatch = spriteBatch;
				_fontContent = fontContent;
				_textureContent = textureContent;
				Origin = Vector2.Zero;
				TransformMatrix = Matrix.Identity;
			}

			public XnaGameTime GameTime
			{
				get
				{
					return _gameTime;
				}
			}

			public SpriteBatch SpriteBatch
			{
				get
				{
					return _spriteBatch;
				}
			}

			public FontContent FontContent
			{
				get
				{
					return _fontContent;
				}
			}

			public TextureContent TextureContent
			{
				get
				{
					return _textureContent;
				}
			}

			public Rectangle? ScissorRectangle
			{
				get;
				set;
			}

			public Vector2 Origin
			{
				get;
				set;
			}

			public Effect Effect
			{
				get;
				set;
			}

			public Matrix TransformMatrix
			{
				get;
				set;
			}
		}
	}
}