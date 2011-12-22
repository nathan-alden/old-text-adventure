using System;
using System.Collections.Generic;

using Junior.Common;

using TextAdventure.Engine.Game.Commands;
using TextAdventure.Engine.Game.Events;
using TextAdventure.Engine.Objects;

namespace TextAdventure.Engine.Game.World
{
	public class WorldInstance
	{
		private readonly DelegateDictionary<Guid, Board> _boardsById = new DelegateDictionary<Guid, Board>();
		private readonly Dictionary<Board, CommandQueue> _commandQueuesByBoard = new Dictionary<Board, CommandQueue>();
		private readonly Player _player;
		private readonly PlayerInput _playerInput = new PlayerInput();
		private readonly Objects.World _world;
		private readonly IWorldObserver _worldObserver;
		private readonly IWorldTime _worldTime;

		public WorldInstance(Objects.World world, Player player, IWorldTime worldTime, IWorldObserver worldObserver)
		{
			world.ThrowIfNull("world");
			player.ThrowIfNull("player");
			worldTime.ThrowIfNull("worldTime");
			worldObserver.ThrowIfNull("worldObserver");

			_world = world;
			_player = player;
			_worldTime = worldTime;
			_worldObserver = worldObserver;

			PopulateCommandQueues(world, worldObserver);
		}

		public Player Player
		{
			get
			{
				return _player;
			}
		}

		public Objects.World World
		{
			get
			{
				return _world;
			}
		}

		public IWorldTime WorldTime
		{
			get
			{
				return _worldTime;
			}
		}

		public Board CurrentBoard
		{
			get
			{
				return _boardsById[_player.BoardId, boardId => _world.GetBoardById(boardId)];
			}
		}

		public CommandQueue CurrentCommandQueue
		{
			get
			{
				return _commandQueuesByBoard[CurrentBoard];
			}
		}

		public PlayerInput PlayerInput
		{
			get
			{
				return _playerInput;
			}
		}

		public void ProcessCommandQueue()
		{
			_commandQueuesByBoard[CurrentBoard].ProcessQueue();
		}

		public EventResult RaiseEvent<TEvent>(IEventHandler<TEvent> eventHandler, TEvent @event)
			where TEvent : Event
		{
			if (eventHandler == null)
			{
				return EventResult.Complete;
			}

			var eventContext = new EventContext(this, _commandQueuesByBoard[CurrentBoard]);

			eventHandler.HandleEvent(eventContext, @event);

			_worldObserver.EventHandled(eventHandler, @event, eventContext.Cancel ? EventResult.Canceled : EventResult.Complete);

			return eventContext.Cancel ? EventResult.Canceled : EventResult.Complete;
		}

		private void PopulateCommandQueues(Objects.World world, IWorldObserver worldObserver)
		{
			foreach (Board board in world.Boards)
			{
				_commandQueuesByBoard.Add(board, new CommandQueue(worldObserver, this));
			}
		}
	}
}