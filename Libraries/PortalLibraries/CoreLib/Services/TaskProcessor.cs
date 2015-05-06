using System;
using System.Collections.Generic;
using System.Threading;
using Core.Exceptions;

namespace Core.Services
{
	/// <summary>
	/// Класс, отвечающий за обработку задач. 
	/// Контролирует потоки, которые обрабатывают задачи из очереди.
	/// </summary>
	public class TaskProcessor
	{
		#region Конструкторы
		/// <summary>
		/// Создает и инициализирует процессор задач.
		/// </summary>
		/// <param name="threadCount">Количество обработчиков задач.</param>
		public TaskProcessor( int threadCount )
		{
			if(threadCount <= 0)
			{
                throw new CoreArgumentOutOfRangeException(Resources.ResourceManager.GetString("ThreadException"));
			}
			m_IsActive = false;
			m_ThreadCount = threadCount;
			m_ActivationEvent = new ManualResetEvent( false );
			m_TaskQueue = new Queue<IRunnable>();
			m_TaskMap = new Dictionary<IRunnable, bool>();
		}

		#endregion

		#region Поля
		/// <summary>
		/// Событие синхронизации, которое устанавливается, когда в очереди появляются задачи, 
		/// и сбрасывается, когда очереь становится пустой.
		/// </summary>
		private ManualResetEvent m_ActivationEvent;
		/// <summary>
		/// Активен ли сейчас процессор.
		/// </summary>
		private bool m_IsActive;
		/// <summary>
		/// Вектор обрабатывающих потоков.
		/// </summary>
		private Thread[] m_Processors;
		/// <summary>
		/// Количество обрабатывающих потоков.
		/// </summary>
		private int m_ThreadCount;
		/// <summary>
		/// Очередь с задачами.
		/// </summary>
		private Queue<IRunnable> m_TaskQueue;
		/// <summary>
		/// Карта задач, используется для того, чтобы задачи, эквивалентные данной, 
		/// не помещались в очередь до окончания обработки данной задачи.
		/// </summary>
		private Dictionary<IRunnable, bool> m_TaskMap;

		#endregion

		#region Методы
		/// <summary>
		/// Помещает задачу в очередь задач. Если в очереди или в обработке 
		/// уже присутствует эквивалентная ей задача, то новая задача не помещается в очередь.
		/// </summary>
		/// <param name="task">объект задачи</param>
		public void EnqueueTask( IRunnable task )
		{
			lock(m_TaskMap)
			{
				// если задача еще не находится в обработке
				if(!m_TaskMap.ContainsKey( task ))
				{
					// помечаем задачу, как находящуюся в обработке
					m_TaskMap[task] = true;

					lock(m_TaskQueue)
					{
						// помещаем задачу в очередь задач
						m_TaskQueue.Enqueue( task );

						// если добавлена ПЕРВАЯ задача, то поднимаем событие, "будящее" потоки
						if(m_TaskQueue.Count == 1)
						{
							m_ActivationEvent.Set();
						}
					}
				}
			}
		}

		/// <summary>
		/// Извлекает задачу из очереди задач.
		/// </summary>
		/// <returns></returns>
		public IRunnable DequeueTask()
		{
			IRunnable task = null;
			lock(m_TaskQueue)
			{
				if(m_TaskQueue.Count > 0)
				{
					task = m_TaskQueue.Dequeue();
				}
				if(m_TaskQueue.Count == 0)
				{
					m_ActivationEvent.Reset();
				}
			}
			return task;
		}

		/// <summary>
		/// Возвращает true, если процессор активен, иначе false.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return m_IsActive;
			}
		}

		/// <summary>
		/// Создает потоки для обработки задач и запускает их.
		/// </summary>
		public void Start()
		{
			if(!m_IsActive)
			{
				m_IsActive = true;

				m_Processors = new Thread[m_ThreadCount];

				for(int i = 0; i < m_Processors.Length; i++)
				{
					m_Processors[i] = new Thread( new ParameterizedThreadStart( DoTask ) );
					m_Processors[i].Start( i );
				}
			}
		}

		/// <summary>
		/// Останавливает потоки.
		/// </summary>
		public void Stop()
		{
			m_IsActive = false;
			// будим потоки, чтобы они завершили работу
			m_ActivationEvent.Set();
		}

		/// <summary>
		/// Метод, который вызывается потоками.
		/// </summary>
		private void DoTask( object data )
		{
			while(m_IsActive)
			{
				// получаем из очереди задачу
				IRunnable task = DequeueTask();
				if(task == null)
				{
					// если очередь пуста, останавливаем выполнение потока.
					m_ActivationEvent.WaitOne();
					continue;
				}

				try
				{
					// выполняем задачу
					task.Run();
				}
				catch(Exception ex)
				{
					Logger.Log.Fatal( "Ошибка обработки задачи", ex );
				}
				finally
				{
					lock(m_TaskMap)
					{
						// помечаем задачу как прошедшую обработку (удаляем ее из активных задач)
						if(m_TaskMap.ContainsKey( task ))
						{
							m_TaskMap.Remove( task );
						}
					}
				}
			}
		}

		#endregion
	}
}
