using System;
using UnityEngine;
using UnityEngine.UI;

public class TestSaveLoad : MonoBehaviour
{
	[SerializeField] Text log;
	LogHandler logHandler;
	void Start()
	{
		logHandler = new LogHandler(log);
		log.text = "";
	}
	public void OnSave()
	{
		++SystemData.Instance.Chunk.saveCount;
		SystemData.Instance.Save();
	}
	public void OnLoad()
	{
		SystemData.Instance.Load();
		++SystemData.Instance.Chunk.loadCount;
	}
	class LogHandler : ILogHandler
	{
		ILogHandler m_DefaultLogHandler = Debug.unityLogger.logHandler;
		public Text log;

		public LogHandler(Text t)
		{
			log = t;
			string filePath = Application.persistentDataPath + "/MyLogs.txt";
			Debug.unityLogger.logHandler = this;
		}

		public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
		{
			log.text = String.Format(format, args) + "\n" + log.text;
			m_DefaultLogHandler.LogFormat(logType, context, format, args);
		}

		public void LogException(Exception exception, UnityEngine.Object context)
		{
			m_DefaultLogHandler.LogException(exception, context);
		}
	}
}
