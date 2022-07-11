using System;

namespace ArRetarget
{
	public class LogManager : Singleton<LogManager>
	{
		public enum Message
		{
			Notification,
			Warning,
			Error,
			Disable
		}
		public Message msg;

		public event Action<string, Message> m_Log;

		public void Log(string log, Message type)
		{
			m_Log(log, type);
		}

		public void OnEnable()
		{
			m_Log += logReceived;
		}

		//this should never happen
		public void OnDisable()
		{
			if (this.gameObject)
				m_Log -= logReceived;
		}

		public void logReceived(string log, Message type)
		{
			msg = type;
		}
	}
}
