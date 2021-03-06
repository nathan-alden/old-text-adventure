﻿using System;
using System.Collections.Generic;

using TextAdventure.WindowsGame.Fmod;

namespace TextAdventure.WindowsGame.Managers
{
	public class SoundEffectManager : IDisposable
	{
		private readonly Dictionary<Guid, SoundManager> _soundManagersById = new Dictionary<Guid, SoundManager>();

		public void Dispose()
		{
			OnDispose(true);
			GC.SuppressFinalize(this);
		}

		public void Play(Guid id, byte[] data, SoundParameters parameters)
		{
			SoundManager manager;

			if (!_soundManagersById.TryGetValue(id, out manager))
			{
				manager = new SoundManager(data);
				_soundManagersById.Add(id, manager);
			}

			Sound sound = manager.Next();

			sound.Play(parameters);
		}

		public void Mute()
		{
			foreach (SoundManager soundManager in _soundManagersById.Values)
			{
				soundManager.MuteAll();
			}
		}

		public void Unmute()
		{
			foreach (SoundManager soundManager in _soundManagersById.Values)
			{
				soundManager.UnmuteAll();
			}
		}

		public void Reset()
		{
			DisposeAllSoundManagers();
			_soundManagersById.Clear();
		}

		protected virtual void OnDispose(bool disposing)
		{
			if (disposing)
			{
				DisposeAllSoundManagers();
			}
		}

		private void DisposeAllSoundManagers()
		{
			foreach (SoundManager manager in _soundManagersById.Values)
			{
				manager.Dispose();
			}
		}

		private class SoundManager : IDisposable
		{
			private const int InstanceCount = 16;
			private readonly Sound[] _sounds = new Sound[InstanceCount];
			private int _index = -1;

			public SoundManager(byte[] data)
			{
				for (int i = 0; i < InstanceCount; i++)
				{
					_sounds[i] = new Sound(SoundSystem.Instance, data);
				}
			}

			public void Dispose()
			{
				foreach (Sound sound in _sounds)
				{
					sound.Dispose();
				}
			}

			public Sound Next()
			{
				if (++_index == InstanceCount)
				{
					_index = 0;
				}

				Sound sound = _sounds[_index];

				sound.Stop();

				return sound;
			}

			public void MuteAll()
			{
				foreach (Sound sound in _sounds)
				{
					sound.Mute();
				}
			}

			public void UnmuteAll()
			{
				foreach (Sound sound in _sounds)
				{
					sound.Unmute();
				}
			}
		}
	}
}