using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;


public class WeatherManager : MonoBehaviour, IGameManager {
 public ManagerStatus status {get; private set;}
 public float cloudValue {get; private set;} 

 // Add cloud value here (listing 10.8)
 private NetworkService network;


 public void Startup(NetworkService service) {
 Debug.Log("Weather manager starting...");
 network = service;
 StartCoroutine(network.GetWeatherXML(OnXMLDataLoaded));
 status = ManagerStatus.Initializing;
}
// public void OnXMLDataLoaded(string data) {
//  Debug.Log(data);
//  status = ManagerStatus.Started;
// }

public void OnXMLDataLoaded(string data) {
 XmlDocument doc = new XmlDocument();
 doc.LoadXml(data);
 XmlNode root = doc.DocumentElement;
 XmlNode node = root.SelectSingleNode("clouds");
string value = node.Attributes["value"].Value;
//string value = "1";   // to make cloud value manualy
 cloudValue = Convert.ToInt32(value) / 100f;
 Debug.Log($"Value: {cloudValue}");
 Messenger.Broadcast(GameEvent.WEATHER_UPDATED);
 status = ManagerStatus.Started;
}



}