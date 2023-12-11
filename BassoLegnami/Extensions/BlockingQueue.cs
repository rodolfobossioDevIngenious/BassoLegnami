using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BassoLegnami.Extensions
{
	public class BlockingQueue<T> where T : class
	{
		private bool closing;
		private readonly Queue<T> queue = new();

		public int Count
		{
			get
			{
				lock (queue)
				{
					return queue.Count;
				}
			}
		}

		public bool Any(Func<T, bool> predicate)
		{
			lock (queue)
			{
				return queue.Any(predicate);
			}
		}

		public BlockingQueue()
		{
			lock (queue)
			{
				closing = false;
				Monitor.PulseAll(queue);
			}
		}

		public bool Enqueue(T item)
		{
			lock (queue)
			{
				if (closing || item == null)
				{
					return false;
				}

				queue.Enqueue(item);

				if (queue.Count == 1)
				{
					Monitor.PulseAll(queue);
				}

				return true;
			}
		}

		public void Close()
		{
			lock (queue)
			{
				if (!closing)
				{
					closing = true;
					queue.Clear();
					Monitor.PulseAll(queue);
				}
			}
		}

		public bool TryDequeue(out T value, int timeout = Timeout.Infinite)
		{
			lock (queue)
			{
				while (queue.Count == 0)
				{
					if (closing || (timeout < Timeout.Infinite) || !Monitor.Wait(queue, timeout))
					{
						value = default(T);
						return false;
					}
				}

				value = queue.Dequeue();
				return true;
			}
		}

		public void Clear()
		{
			lock (queue)
			{
				queue.Clear();
				Monitor.Pulse(queue);
			}
		}
	}
}
