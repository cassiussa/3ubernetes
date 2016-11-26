using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

// State object for receiving data from remote device.
public class StateObject {
	public Socket workSocket = null;  // Client socket.
	public const int bufferSize = 256;  // Size of receive buffer.
	public byte[] buffer = new byte[bufferSize];  // Receive buffer.
	public StringBuilder sb = new StringBuilder();  // Received data string.
}

public class AsyncrhonousSocketClient : MonoBehaviour {
	public enum Protocol { none, HTTP, HTTPS }
	public Protocol protocol = Protocol.HTTP;
	Protocol _cachedProtocol = Protocol.none;

	public int serverPort;
	public string domainName;
	public string connectMsg;


	IEnumerator Start() {
		while (true) {
			switch (protocol) {
			case Protocol.HTTP:
				Http ();
				break;
			case Protocol.HTTPS:
				Https ();
				break;
			}
			yield return null;
		}
	}

	/*
	 * 
	 * Finite State Machine Definitions
	 * Http()
	 * Https()
	 */

	public void Http() {
		if (_cachedProtocol != protocol) {
			_cachedProtocol = protocol;
			Debug.Log ("HTTP " + Time.deltaTime);
			StartCoroutine ("StartClient");
		}
	}

	public void Https() {
		if (_cachedProtocol != protocol) {
			_cachedProtocol = protocol;
			Debug.Log ("HTTPS " + Time.deltaTime);
			StartCoroutine ("StartClient");
		}
	}
	
	// ManualResetEvent instances signal completion.
	private static ManualResetEvent connectDone = new ManualResetEvent(false);
	private static ManualResetEvent sendDone = new ManualResetEvent(false);
	private static ManualResetEvent receiveDone = new ManualResetEvent(false);

	private String response = String.Empty;  // The response from the remote device.

	// Connect to a remote device.
	IEnumerator StartClient() {
		try {
			// Establish the remote endpoint for the socket.
			IPHostEntry ipHostInfo = Dns.GetHostEntry(domainName);  // Get the Host Information for the domain name
			IPAddress IPAddress = ipHostInfo.AddressList[0];  // Get the IP address of the domain from the Host Info
			IPEndPoint serverDomain = new IPEndPoint(IPAddress,serverPort);

			Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  // Create a TCP/IP socket.

			client.BeginConnect(serverDomain, new AsyncCallback(ConnectCallback), client);  // Connect to the remote endpoint.
			connectDone.WaitOne();

			Send(client,connectMsg);  // Send data to the server
			sendDone.WaitOne();

			Receive(client);  // Receive response from the server
			receiveDone.WaitOne();

			Debug.Log("Response received : "+ response+" "+Time.deltaTime);  // Write the response to the console.

			client.Shutdown(SocketShutdown.Both);  // Release the socket.
			client.Close();
			
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
		yield return null;
	}
	
	private static void ConnectCallback(IAsyncResult ar) {
		try {
			Socket client = (Socket) ar.AsyncState;  // Retrieve the socket from the state object
			client.EndConnect(ar);  // Complete the connection.
			Debug.Log("Socket connected to "+client.RemoteEndPoint.ToString());
			connectDone.Set();  // Signal that the connection has been made
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}
	
	private void Receive(Socket client) {
		try {
			StateObject state = new StateObject();  // Create the state object
			state.workSocket = client;
			client.BeginReceive( state.buffer, 0, StateObject.bufferSize, 0, new AsyncCallback(ReceiveCallback), state);  // Begin receiving the data from the server
			//Debug.Log ("Receiving data "+Time.deltaTime);
		} catch (Exception e) {
			Debug.LogError(e.ToString());
		}
	}
	
	private void ReceiveCallback( IAsyncResult ar ) {
		try {
			StateObject state = (StateObject) ar.AsyncState;  // Retrieve the state object and the client socket from the asynchronous state object.
			Socket client = state.workSocket;
			int bytesRead = client.EndReceive(ar);  // Read data from the remote device
			
			if (bytesRead > 0) {
				state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));  // There might be more data, so store the data received so far
				client.BeginReceive(state.buffer,0,StateObject.bufferSize,0, new AsyncCallback(ReceiveCallback), state);  // Get the rest of the data
			} else {
				if (state.sb.Length > 1) {  // All the data has arrived; put it in response
					response = state.sb.ToString();  // Create the response variable data
				}
				receiveDone.Set();  // Signal that all bytes have been received
			}
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}
	
	private static void Send(Socket client, String data) {
		byte[] byteData = Encoding.ASCII.GetBytes(data);  // Convert the string data to byte data using ASCII encoding
		//Debug.Log ("send "+Time.deltaTime);
		client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);  // Begin sending the data to the remote device
	}
	
	private static void SendCallback(IAsyncResult ar) {
		try {
			Socket client = (Socket) ar.AsyncState;  // Retrieve the socket from the state object
			int bytesSent = client.EndSend(ar);  // Complete sending the data to the remote device
			Debug.Log("Sent "+bytesSent+" bytes to server.");
			sendDone.Set();  // Signal that all bytes have been sent
			Debug.Log ("All bytes sent");
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}

}
