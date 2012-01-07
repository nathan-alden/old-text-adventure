﻿using System;
using System.Collections.Generic;

using TextAdventure.Engine.Common;
using TextAdventure.Engine.Objects;

namespace TextAdventure.Engine.Game.Messages
{
	public class MessageBuilder
	{
		private readonly Color _backgroundColor;
		private readonly Guid _id;
		private readonly List<IMessagePart> _parts = new List<IMessagePart>();

		public MessageBuilder(Color backgroundColor)
			: this(Guid.NewGuid(), backgroundColor)
		{
			_backgroundColor = backgroundColor;
			_id = Guid.NewGuid();
		}

		public MessageBuilder(Guid id, Color backgroundColor)
		{
			_id = id;
			_backgroundColor = backgroundColor;
		}

		public Message Message
		{
			get
			{
				return new Message(_id, _backgroundColor, _parts);
			}
		}

		public MessageBuilder Color(Color color)
		{
			_parts.Add(new MessageColor(color));

			return this;
		}

		public MessageBuilder Text(string text)
		{
			_parts.Add(new MessageText(text));

			return this;
		}

		public MessageBuilder LineBreak()
		{
			_parts.Add(new MessageLineBreak());

			return this;
		}

		public MessageBuilder Question(
			string prompt,
			Color questionForegroundColor,
			Color unselectedAnswerForegroundColor,
			Color unselectedAnswerBackgroundColor,
			Color selectedAnswerForegroundColor,
			Color selectedAnswerBackgroundColor,
			params MessageAnswer[] answers)
		{
			_parts.Add(new MessageQuestion(
			           	prompt,
			           	questionForegroundColor,
			           	unselectedAnswerForegroundColor,
			           	selectedAnswerForegroundColor,
			           	selectedAnswerBackgroundColor,
			           	answers));

			return this;
		}

		public static implicit operator Message(MessageBuilder builder)
		{
			return builder.Message;
		}
	}
}