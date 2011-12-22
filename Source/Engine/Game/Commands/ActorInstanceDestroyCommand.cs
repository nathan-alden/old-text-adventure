using System.Collections.Generic;

using Junior.Common;

using TextAdventure.Engine.Game.Events;
using TextAdventure.Engine.Game.World;
using TextAdventure.Engine.Objects;

namespace TextAdventure.Engine.Game.Commands
{
	public class ActorInstanceDestroyCommand : Command
	{
		private readonly ActorInstance _actorInstance;

		public ActorInstanceDestroyCommand(ActorInstance actorInstance)
		{
			actorInstance.ThrowIfNull("actorInstance");

			_actorInstance = actorInstance;
		}

		public override IEnumerable<string> Details
		{
			get
			{
				yield return "Actor instance ID: " + _actorInstance.Id;
			}
		}

		protected override CommandResult OnExecute(CommandContext context)
		{
			EventResult result = context.RaiseEvent(_actorInstance.ActorInstanceDestroyedEventHandler, new ActorInstanceDestroyedEvent(_actorInstance));

			if (result == EventResult.Canceled || !context.CurrentBoard.ActorInstanceLayer.RemoveTile(_actorInstance))
			{
				return CommandResult.Failed;
			}

			context.CancelCommands(_actorInstance);

			return CommandResult.Succeeded;
		}
	}
}