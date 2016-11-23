using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

public class TCPSocket : MonoBehaviour {
	//http://answers.unity3d.com/questions/15422/unity-project-and-3rd-party-apps.html#answer-15477
	internal Boolean socketReady = false;
	TcpClient mySocket;
	NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;
	String Host = "104.198.50.196";
	Int32 Port = 443;

	public void Start() {
		SetupSocket ();
		ReadSocket ();
	}

	public void SetupSocket() {
		try {
			mySocket = new TcpClient(Host, Port);
			theStream = mySocket.GetStream();
			theWriter = new StreamWriter(theStream);
			theReader = new StreamReader(theStream);
			socketReady = true;
		}
		catch (Exception failure) {
			Debug.LogError("Socket error: " + failure);
		}
	}

	public void WriteSocket(string theLine) {
		if (!socketReady) {
			return;
		}
		String foo = theLine + "\r\n";
		theWriter.Write(foo);
		theWriter.Flush();
	}

	public String ReadSocket() {
		if (!socketReady) {
			Debug.Log ("a");
			return "";
		}
		if (theStream.DataAvailable) {
			Debug.Log ("b");
			return theReader.ReadLine ();
		}
		Debug.Log ("c");
		return "";
	}

	public void CloseSocket() {
		if (!socketReady) {
			return;
		}
		theWriter.Close();
		theReader.Close();
		mySocket.Close();
		socketReady = false;
	}

}
