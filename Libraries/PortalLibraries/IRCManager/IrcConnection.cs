/**
 * $Id: IrcConnection.cs 77 2004-09-19 13:31:53Z meebey $
 * $URL: svn://svn.qnetp.net/smartirc/SmartIrc4net/tags/0.2.0/src/IrcConnection.cs $
 * $Rev: 77 $
 * $Author: meebey $
 * $Date: 2004-09-19 15:31:53 +0200 (Sun, 19 Sep 2004) $
 *
 * Copyright (c) 2003-2004 Mirco 'meebey' Bauer <mail@meebey.net> <http://www.meebey.net>
 * 
 * Full LGPL License: <http://www.gnu.org/licenses/lgpl.txt>
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Reflection;

using Meebey.SmartIrc4net.Delegates;

namespace Meebey.SmartIrc4net
{
	/// <summary>
	///
	/// </summary>
	public class IrcConnection
	{
		#region [ Fields ]

		private string _Version;
		private string _VersionString;

		private IList<string> _AddressList = new List<string> { "localhost" };

		private int _CurrentAddress = 0;
		private int _Port = 6667;
		private int _SendDelay = 200;

		private StreamReader _Reader;
		private StreamWriter _Writer;

		private ReadThread _ReadThread;
		private WriteThread _WriteThread;

		private IrcTcpClient _TcpClient;
		private Dictionary<Priority, Queue> _SendBuffer = new Dictionary<Priority, Queue>();

		private bool _Registered = false;
		private bool _Connected = false;
		private int _ConnectTries = 0;
		private bool _AutoRetry = false;
		private bool _AutoReconnect = false;
		private bool _ConnectionError = false;
		private Encoding _Encoding = Encoding.GetEncoding("ISO-8859-15");
		//private Encoding        _Encoding = Encoding.GetEncoding(1252);
		//private Encoding        _Encoding = Encoding.UTF8;

		public event ReadLineEventHandler OnReadLine;
		public event WriteLineEventHandler OnWriteLine;

		public event SimpleEventHandler OnConnect;
		public event SimpleEventHandler OnConnected;
		public event SimpleEventHandler OnDisconnect;
		public event SimpleEventHandler OnDisconnected;

		#endregion

		#region [ Properties ]

		protected bool ConnectionError
		{
			get
			{
				lock (this)
				{
					return _ConnectionError;
				}
			}
			set
			{
				lock (this)
				{
					_ConnectionError = value;
				}
			}
		}

		public string Address
		{
			get { return _AddressList[_CurrentAddress]; }
		}

		public IList<string> AddressList
		{
			get { return _AddressList; }
		}

		public int Port
		{
			get { return _Port; }
		}

		public bool AutoReconnect
		{
			get { return _AutoReconnect; }
			set
			{
#if LOG4NET
				if (value == true) {
					Logger.Connection.Info("AutoReconnect enabled");
				} else {
					Logger.Connection.Info("AutoReconnect disabled");
				}
#endif
				_AutoReconnect = value;
			}
		}

		public bool AutoRetry
		{
			get { return _AutoRetry; }
			set
			{
#if LOG4NET
				if (value == true) {
					Logger.Connection.Info("AutoRetry enabled");
				} else {
					Logger.Connection.Info("AutoRetry disabled");
				}
#endif
				_AutoRetry = value;
			}
		}

		public int SendDelay
		{
			get { return _SendDelay; }
			set { _SendDelay = value; }
		}

		public bool Registered
		{
			get { return _Registered; }
		}

		public bool Connected
		{
			get { return _Connected; }
		}

		public string Version
		{
			get { return _Version; }
		}

		public string VersionString
		{
			get { return _VersionString; }
		}

		public Encoding Encoding
		{
			get { return _Encoding; }
			set { _Encoding = value; }
		}

		#endregion

		#region [ Constructor ]

		public IrcConnection()
		{
#if LOG4NET        
			Logger.Init();
#endif
			_SendBuffer[Priority.High] = Queue.Synchronized(new Queue());
			_SendBuffer[Priority.AboveMedium] = Queue.Synchronized(new Queue());
			_SendBuffer[Priority.Medium] = Queue.Synchronized(new Queue());
			_SendBuffer[Priority.BelowMedium] = Queue.Synchronized(new Queue());
			_SendBuffer[Priority.Low] = Queue.Synchronized(new Queue());

			OnReadLine += new ReadLineEventHandler(simpleParser);

			_ReadThread = new ReadThread(this);
			_WriteThread = new WriteThread(this);

			Assembly assembly = Assembly.GetAssembly(this.GetType());
			AssemblyName assembly_name = assembly.GetName(false);

			AssemblyProductAttribute pr = (AssemblyProductAttribute)assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0];

			_Version = assembly_name.Version.ToString();
			_VersionString = pr.Product + " " + _Version;
		}

		#endregion

		#region [ Connect ]

		public bool Connect(string address, int port)
		{
			return Connect(new string[] { address }, port);
		}

		public bool Connect(IList<string> addresslist, int port)
		{
			if (_Connected != false)
				throw new Exception("already connected");

#if LOG4NET
			Logger.Connection.Info("connecting...");
#endif
			_ConnectTries++;

			string[] arr = new string[addresslist.Count];
			addresslist.CopyTo(arr, 0);
			_AddressList = arr.ToList<string>();

			_Port = port;

			if (OnConnect != null)
				OnConnect();

			try
			{
				//System.Net.IPAddress ip = System.Net.Dns.Resolve(Address).AddressList[0];
				System.Net.IPAddress ip = System.Net.Dns.GetHostEntry(Address).AddressList[0];

				_TcpClient = new IrcTcpClient();
				_TcpClient.Connect(ip, port);

				if (OnConnected != null)
					OnConnected();

				_Reader = new StreamReader(_TcpClient.GetStream(), Encoding);
				_Writer = new StreamWriter(_TcpClient.GetStream(), Encoding);

				// Connection was succeful, reseting the connect counter
				_ConnectTries = 0;

				// updating the connection id, so connecting is possible again
				lock (this)
				{
					ConnectionError = false;
				}
				_Connected = true;
#if LOG4NET
				Logger.Connection.Info("connected");
#endif

				// lets power up our threads
				_ReadThread.Start();
				_WriteThread.Start();

				return true;
			}
			catch (Exception)
			{
				if (_Reader != null)
					_Reader.Close();

				if (_Writer != null)
					_Writer.Close();

				if (_TcpClient != null)
					_TcpClient.Close();

				_Connected = false;
				ConnectionError = true;
#if LOG4NET
				Logger.Connection.Info("connection failed: "+e.Message);
#endif
				if (_AutoRetry == true && _ConnectTries <= 3)
				{
					nextAddress();
					if (Reconnect(false))
						return true;
				}

				return false;
			}
		}

		#endregion

		#region [ Reconnect ]

		public bool Reconnect()
		{
			return Reconnect(true);
		}

		// login parameter only for IrcClient needed
		public virtual bool Reconnect(bool login)
		{
#if LOG4NET
			Logger.Connection.Info("reconnecting...");
#endif
			Disconnect();
			return Connect(_AddressList, _Port);
		}

		#endregion

		#region [ Disconnect ]

		public bool Disconnect()
		{
			if (!Connected)
				return false;

#if LOG4NET
				Logger.Connection.Info("disconnecting...");
#endif
			if (OnDisconnect != null)
				OnDisconnect();

			_ReadThread.Stop();
			_WriteThread.Stop();
			_TcpClient.Close();
			_Connected = false;
			_Registered = false;

			if (OnDisconnected != null)
				OnDisconnected();

#if LOG4NET
				Logger.Connection.Info("disconnected");
#endif
			return true;
		}

		#endregion

		#region [ Listen ]

		public void Listen()
		{
			Listen(true);
		}

		public void Listen(bool blocking)
		{
			if (blocking)
			{
				while (Connected)
				{
					ReadLine(true);
					if (ConnectionError == true)
					{
						if (AutoReconnect)
							Reconnect();
						else
							Disconnect();
					}
				}
			}
			else
			{
				while (ReadLine(false) != String.Empty)
				{
					// loop as long as we receive messages
				}
			}
		}

		public void ListenOnce()
		{
			ListenOnce(true);
		}

		public void ListenOnce(bool blocking)
		{
			ReadLine(blocking);
		}

		#endregion

		public string ReadLine(bool blocking)
		{
			string data = string.Empty;

			if (blocking)
			{
				// block till the queue has data
				while (Connected && _ReadThread.Queue.Count == 0)
				{
					Thread.Sleep(10);
				}
			}

			if (Connected && _ReadThread.Queue.Count > 0)
				data = (string)_ReadThread.Queue.Dequeue();

			if (!string.IsNullOrEmpty(data))
			{
				#if LOG4NET
				Logger.Queue.Debug("read: \""+data+"\"");
				#endif

				if (OnReadLine != null)
					OnReadLine(data);
			}

			return data;
		}

		public void WriteLine(string data)
		{
			WriteLine(data, Priority.Medium);
		}

		public void WriteLine(string data, Priority priority)
		{
			if (priority == Priority.Critical)
				_WriteLine(data);
			else
				_SendBuffer[priority].Enqueue(data);
		}

		private bool _WriteLine(string data)
		{
			if (!Connected)
				return false;

			try
			{
				_Writer.Write(data + "\r\n");
				_Writer.Flush();
			}
			catch (IOException)
			{
				#if LOG4NET
				Logger.Socket.Warn("sending data failed, connection lost");
				#endif

				ConnectionError = true;
				return false;
			}

			#if LOG4NET
			Logger.Socket.Debug("sent: \""+data+"\"");
			#endif

			if (OnWriteLine != null)
				OnWriteLine(data);

			return true;
		}

		private void nextAddress()
		{
			_CurrentAddress++;
			if (_CurrentAddress < _AddressList.Count)
			{
				// nothing
			}
			else
				_CurrentAddress = 0;
#if LOG4NET
			Logger.Connection.Info("set server to: "+Address);
#endif
		}

		private void simpleParser(string rawline)
		{
			string messagecode = string.Empty;
			string[] rawlineex = rawline.Split(new Char[] { ' ' });

			if (rawline.Substring(0, 1) == ":")
			{
				messagecode = rawlineex[1];
				try
				{
					ReplyCode replycode = (ReplyCode)int.Parse(messagecode);
					switch (replycode)
					{
						case ReplyCode.RPL_WELCOME:
							_Registered = true;

#if LOG4NET
							Logger.Connection.Info("logged in");
#endif
							break;
					}
				}
				catch (FormatException) 
				{ 
					// nothing 
				}
			}
			else
			{
				messagecode = rawlineex[0];
				switch (messagecode)
				{
					case "ERROR":
						ConnectionError = true;
						break;
				}
			}
		}

		#region [ ReadThread class]
		
		private class ReadThread
		{
			private IrcConnection _Connection;
			private Thread _Thread;
			// syncronized queue (thread safe)
			private Queue _Queue = Queue.Synchronized(new Queue());

			public Queue Queue
			{
				get { return _Queue; }
			}

			public ReadThread(IrcConnection connection)
			{
				_Connection = connection;
			}

			public void Start()
			{
				_Thread = new Thread(new ThreadStart(_Worker));
				_Thread.Name = "ReadThread (" + _Connection.Address + ":" + _Connection.Port + ")";
				_Thread.IsBackground = true;
				_Thread.Start();
			}

			public void Stop()
			{
				_Thread.Abort();
				_Connection._Reader.Close();
			}

			private void _Worker()
			{
#if LOG4NET
				Logger.Socket.Debug("ReadThread started");
#endif
				try
				{
					string data = string.Empty;
					try
					{
						while (_Connection.Connected && ((data = _Connection._Reader.ReadLine()) != null))
						{
							_Queue.Enqueue(data);
#if LOG4NET
							Logger.Socket.Debug("received: \""+data+"\"");
#endif
						}
					}
					catch (IOException)
					{
#if LOG4NET
						Logger.Socket.Warn("IOException: "+e.Message);
#endif
					}

#if LOG4NET
					Logger.Socket.Warn("connection lost");
#endif
					_Connection.ConnectionError = true;
				}
				catch (ThreadAbortException)
				{
					Thread.ResetAbort();
#if LOG4NET
					Logger.Socket.Debug("ReadThread aborted");
#endif
				}
			}
		}
		
		#endregion

		#region [ WriteThread class]

		private class WriteThread
		{
			private IrcConnection _Connection;
			private Thread _Thread;

			private int _HighCount = 0;
			private int _AboveMediumCount = 0;
			private int _MediumCount = 0;
			private int _BelowMediumCount = 0;
			private int _LowCount = 0;
			private int _AboveMediumSentCount = 0;
			private int _MediumSentCount = 0;
			private int _BelowMediumSentCount = 0;
			private int _AboveMediumThresholdCount = 4;
			private int _MediumThresholdCount = 2;
			private int _BelowMediumThresholdCount = 1;
			private int _BurstCount = 0;

			public WriteThread(IrcConnection connection)
			{
				_Connection = connection;
			}

			public void Start()
			{
				_Thread = new Thread(new ThreadStart(_Worker));
				_Thread.Name = "WriteThread (" + _Connection.Address + ":" + _Connection.Port + ")";
				_Thread.IsBackground = true;
				_Thread.Start();
			}

			public void Stop()
			{
				_Thread.Abort();
				_Connection._Writer.Close();
			}

			private void _Worker()
			{
#if LOG4NET
				Logger.Socket.Debug("WriteThread started");
#endif
				try
				{
					while (_Connection.Connected)
					{
						_CheckBuffer();
						Thread.Sleep(_Connection._SendDelay);
					}

#if LOG4NET
					Logger.Socket.Warn("connection lost");
#endif
					_Connection.ConnectionError = true;
				}
				catch (ThreadAbortException)
				{
					Thread.ResetAbort();
#if LOG4NET
					Logger.Socket.Debug("WriteThread aborted");
#endif
				}
			}

			// WARNING: complex scheduler, don't even think about changing it!
			private void _CheckBuffer()
			{
				// only send data if we are succefully registered on the IRC network
				if (!_Connection._Registered)
					return;

				_HighCount = _Connection._SendBuffer[Priority.High].Count;
				_AboveMediumCount = _Connection._SendBuffer[Priority.AboveMedium].Count;
				_MediumCount = _Connection._SendBuffer[Priority.Medium].Count;
				_BelowMediumCount = _Connection._SendBuffer[Priority.BelowMedium].Count;
				_LowCount = _Connection._SendBuffer[Priority.Low].Count;

				if (checkHighBuffer() && checkAboveMediumBuffer()
					&& checkMediumBuffer() && checkBelowMediumBuffer() && checkLowBuffer())
				{
					// everything is sent, resetting all counters
					_AboveMediumSentCount = 0;
					_MediumSentCount = 0;
					_BelowMediumSentCount = 0;
					_BurstCount = 0;
				}

				if (_BurstCount < 3)
				{
					_BurstCount++;
					//_CheckBuffer();
				}
			}

			private bool checkHighBuffer()
			{
				if (_HighCount == 0)
					return true;

				string data = (string)_Connection._SendBuffer[Priority.High].Dequeue();
				if (!_Connection._WriteLine(data))
				{
					// putting the message back into the queue if sending was not successful
					_Connection._SendBuffer[Priority.High].Enqueue(data);
				}

				// there is more data to send
				if (_HighCount > 1)
					return false;

				return true;
			}

			private bool checkAboveMediumBuffer()
			{
				if (_AboveMediumCount > 0 && _AboveMediumSentCount < _AboveMediumThresholdCount)
				{
					string data = (string)_Connection._SendBuffer[Priority.AboveMedium].Dequeue();
					if (!_Connection._WriteLine(data))
						_Connection._SendBuffer[Priority.AboveMedium].Enqueue(data);

					_AboveMediumSentCount++;

					if (_AboveMediumSentCount < _AboveMediumThresholdCount)
						return false;
				}

				return true;
			}

			private bool checkMediumBuffer()
			{
				if (_MediumCount > 0 && _MediumSentCount < _MediumThresholdCount)
				{
					string data = (string)_Connection._SendBuffer[Priority.Medium].Dequeue();
					if (!_Connection._WriteLine(data))
						_Connection._SendBuffer[Priority.Medium].Enqueue(data);

					_MediumSentCount++;

					if (_MediumSentCount < _MediumThresholdCount)
						return false;
				}

				return true;
			}

			private bool checkBelowMediumBuffer()
			{
				if (_BelowMediumCount > 0 && _BelowMediumSentCount < _BelowMediumThresholdCount)
				{
					string data = (string)_Connection._SendBuffer[Priority.BelowMedium].Dequeue();
					if (!_Connection._WriteLine(data))
						((Queue)_Connection._SendBuffer[Priority.BelowMedium]).Enqueue(data);

					_BelowMediumSentCount++;

					if (_BelowMediumSentCount < _BelowMediumThresholdCount)
						return false;
				}

				return true;
			}

			private bool checkLowBuffer()
			{
				if (_LowCount > 0)
				{
					if (_HighCount > 0 || _AboveMediumCount > 0 || _MediumCount > 0 || _BelowMediumCount > 0)
						return true;

					string data = (string)_Connection._SendBuffer[Priority.Low].Dequeue();
					if (_Connection._WriteLine(data) == false)
						_Connection._SendBuffer[Priority.Low].Enqueue(data);

					if (_LowCount > 1)
						return false;
				}

				return true;
			}
		}

		#endregion
	}
}