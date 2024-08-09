using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using UnityEngine;

public class EventService : MonoBehaviour
{
	readonly float _cooldownBeforeSend = 3f;
	List<Event> _events = new List<Event>();
	bool _isCooldown;
	const string _serverUrl = "";
	const string DefaultSaveFile = "events.json";

	private void Start()
	{
		LoadEventsFromFile();
		_isCooldown = false;
		TrySend();
	}

	public void TrackEvent(string type, string data)
	{
		_events.Add(new Event { Type = type, Data = data });

		TrySend();
	}

	private void TrySend()
	{
		if (_events.Count > 0 && !_isCooldown)
		{
			StartCoroutine(SendEventsWithCooldown());
		}
	}

	private IEnumerator SendEventsWithCooldown()
	{
		_isCooldown = true;
		yield return new WaitForSeconds(_cooldownBeforeSend);

		SendEvents();

		_isCooldown = false;
	}

	private async void SendEvents()
	{
		var eventsToSend = new List<Event>(_events);
		_events.Clear();

		var jsonData = JsonUtility.ToJson(new EventWrapper { events = eventsToSend });

		using (var client = new HttpClient())
		{
			try
			{
				var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

				var response = await client.PostAsync(_serverUrl, content);
				if (!response.IsSuccessStatusCode)
				{
					_events.AddRange(eventsToSend);
				}
			}
			catch (Exception)
			{
				_events.AddRange(eventsToSend);
			}
		}

		SaveEventsToFile();
	}

	private void SaveEventsToFile()
	{
		string path = Path.Combine(Application.persistentDataPath, DefaultSaveFile);
		string data = JsonUtility.ToJson(new EventWrapper { events = _events });

		using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
		{
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.Write(data);
			}
		}
	}

	private void LoadEventsFromFile()
	{
		string path = Path.Combine(Application.persistentDataPath, DefaultSaveFile);

		if (!File.Exists(path))
		{
			_events = new List<Event>();
		}
		else
		{
			using (FileStream stream = File.Open(path, FileMode.Open))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					var jsonData = reader.ReadToEnd();
					var eventWrapper = JsonUtility.FromJson<EventWrapper>(jsonData);
					_events = eventWrapper.events;
				}
			}
		}
	}

	private void OnApplicationQuit()
	{
		SaveEventsToFile();
	}

	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			SaveEventsToFile();
		}
		else
		{
			TrySend();
		}
	}
}
