using System;
using System.Collections.Generic;
using System.Threading;
using Core.Exceptions;

namespace Core.Services
{
	/// <summary>
	/// �����, ���������� �� ��������� �����. 
	/// ������������ ������, ������� ������������ ������ �� �������.
	/// </summary>
	public class TaskProcessor
	{
		#region ������������
		/// <summary>
		/// ������� � �������������� ��������� �����.
		/// </summary>
		/// <param name="threadCount">���������� ������������ �����.</param>
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

		#region ����
		/// <summary>
		/// ������� �������������, ������� ���������������, ����� � ������� ���������� ������, 
		/// � ������������, ����� ������ ���������� ������.
		/// </summary>
		private ManualResetEvent m_ActivationEvent;
		/// <summary>
		/// ������� �� ������ ���������.
		/// </summary>
		private bool m_IsActive;
		/// <summary>
		/// ������ �������������� �������.
		/// </summary>
		private Thread[] m_Processors;
		/// <summary>
		/// ���������� �������������� �������.
		/// </summary>
		private int m_ThreadCount;
		/// <summary>
		/// ������� � ��������.
		/// </summary>
		private Queue<IRunnable> m_TaskQueue;
		/// <summary>
		/// ����� �����, ������������ ��� ����, ����� ������, ������������� ������, 
		/// �� ���������� � ������� �� ��������� ��������� ������ ������.
		/// </summary>
		private Dictionary<IRunnable, bool> m_TaskMap;

		#endregion

		#region ������
		/// <summary>
		/// �������� ������ � ������� �����. ���� � ������� ��� � ��������� 
		/// ��� ������������ ������������� �� ������, �� ����� ������ �� ���������� � �������.
		/// </summary>
		/// <param name="task">������ ������</param>
		public void EnqueueTask( IRunnable task )
		{
			lock(m_TaskMap)
			{
				// ���� ������ ��� �� ��������� � ���������
				if(!m_TaskMap.ContainsKey( task ))
				{
					// �������� ������, ��� ����������� � ���������
					m_TaskMap[task] = true;

					lock(m_TaskQueue)
					{
						// �������� ������ � ������� �����
						m_TaskQueue.Enqueue( task );

						// ���� ��������� ������ ������, �� ��������� �������, "�������" ������
						if(m_TaskQueue.Count == 1)
						{
							m_ActivationEvent.Set();
						}
					}
				}
			}
		}

		/// <summary>
		/// ��������� ������ �� ������� �����.
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
		/// ���������� true, ���� ��������� �������, ����� false.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return m_IsActive;
			}
		}

		/// <summary>
		/// ������� ������ ��� ��������� ����� � ��������� ��.
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
		/// ������������� ������.
		/// </summary>
		public void Stop()
		{
			m_IsActive = false;
			// ����� ������, ����� ��� ��������� ������
			m_ActivationEvent.Set();
		}

		/// <summary>
		/// �����, ������� ���������� ��������.
		/// </summary>
		private void DoTask( object data )
		{
			while(m_IsActive)
			{
				// �������� �� ������� ������
				IRunnable task = DequeueTask();
				if(task == null)
				{
					// ���� ������� �����, ������������� ���������� ������.
					m_ActivationEvent.WaitOne();
					continue;
				}

				try
				{
					// ��������� ������
					task.Run();
				}
				catch(Exception ex)
				{
					Logger.Log.Fatal( "������ ��������� ������", ex );
				}
				finally
				{
					lock(m_TaskMap)
					{
						// �������� ������ ��� ��������� ��������� (������� �� �� �������� �����)
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
