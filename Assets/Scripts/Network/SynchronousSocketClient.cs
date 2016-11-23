using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SynchronousSocketClient : MonoBehaviour {

	/*
	 * https://cobe.io/blog/posts/kubernetes-watch-python/
	 * https://104.198.50.196/api/v1/componentstatuses/etcd-0
	 * https://msdn.microsoft.com/en-us/library/kb5kfec7(v=vs.110).aspx
	 */

	void Start() {
		StartClient();
	}

	public static void StartClient() {
		// Data buffer for incoming data.
		byte[] bytes = new byte[1024];
		
		// Connect to a remote device.
		try {
			// Establish the remote endpoint for the socket.
			// This example uses port 443 on 104.198.50.196.
			IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
			//IPAddress ipAddress = ipHostInfo.AddressList[0];
			byte[] add = new byte[4] {104,198,50,196};
			IPAddress ipAddress = new IPAddress(add);
			IPEndPoint remoteEP = new IPEndPoint(ipAddress,443);
			
			// Create a TCP/IP  socket.
			Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
			// Connect the socket to the remote endpoint. Catch any errors.
			try {
				sender.Connect(remoteEP);
				
				Debug.Log("Socket connected to "+sender.RemoteEndPoint.ToString());
				
				// Encode the data string into a byte array.
				//byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");
				//byte[] msg = Encoding.ASCII.GetBytes("'GET /api/v1/componentstatuses/etcd-0 HTTP/1.1\r\n' 'Host: 104.198.50.196\r\n' '\r\n'");
				byte[] msg = Encoding.ASCII.GetBytes("/api/v1/componentstatuses/etcd-0 HTTP/1.1\r\n Host: 104.198.50.196\r\n \r\n");

				// Send the data through the socket.
				int bytesSent = sender.Send(msg);
				Debug.Log ("Number of bytes sent: "+bytesSent);
				
				// Receive the response from the remote device.
				int bytesRec = sender.Receive(bytes);
				Debug.Log ("Number of bytes received: "+bytesRec);
				Debug.Log("Echoed test = " + (Encoding.ASCII.GetString(bytes,0,bytesRec)));
				
				// Release the socket.
				sender.Shutdown(SocketShutdown.Both);
				sender.Close();
				
			} catch (ArgumentNullException ane) {
				Debug.Log("ArgumentNullException : " + ane.ToString() );
			} catch (SocketException se) {
				Debug.Log("SocketException : "+ se.ToString() );
			} catch (Exception e) {
				Debug.Log("Unexpected exception :"+ e.ToString() );
			}
			
		} catch (Exception e) {
			Debug.Log( e.ToString());
		}
	}
	
	}