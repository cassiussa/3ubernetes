using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Permissions;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

public class newhttptest : MonoBehaviour
{
	string s = "not set";
	// Use this for initialization
	/// <summary>
	///Solves known Mono/Unity3D issue where get requests fail due to receive server certification 
	///http://stackoverflow.com/questions/3285489/mono-problems-with-cert-and-mozroots
	////////////////////////////////////////////////////////////////////////////////////////////
	/// TlsException: Invalid certificate received from server. Error code: 0xffffffff80092012
	/// WebException: Error writing request: The authentication or decryption has failed.
	/// Thanks to rhollencamp of Stackoverflow 
	////////////////////////////////////////////////////////////////////////////////////////////
	/// </summary>
	/*void OnEnable()
	{
		ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
			if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors) {
				foreach (X509ChainStatus status in chain.ChainStatus) {
					if (status.Status != X509ChainStatusFlags.RevocationStatusUnknown) {
						return false;
					}
				}
				return true;
			}
			return false;
		};
	}*/


	void Start ()
	{
		X509Certificate2 adminClient = new X509Certificate2("Assets/Scripts/adminClient.p12","notasecret");

		ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback (ValidateServerCertificate);
		ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
		//ServicePointManager.ServerCertificateValidationCallback += CertificateValidationCallBack;

		try {
			Debug.Log ("a");
			
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create ("https://104.198.50.196/api/v1/pods?watch=true&timeoutSeconds=1&pretty=true");
			Debug.Log ("b");
			request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
			Debug.Log ("c");
			//request.ClientCertificates.Add (adminClient);
			Debug.Log ("d");
			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
			Debug.Log ("e");
			
			//XmlDocument xmlDoc = new XmlDocument();
			//xmlDoc.Load(response.GetResponseStream());
			Debug.Log(new StreamReader(response.GetResponseStream()).ReadToEnd());
			Debug.Log ("f");
			
			Debug.Log ("Success");
			
			s = "yippy!";
			
		} catch (WebException we) {
			var ex = we as Exception;
			s = "fail";
			while (ex != null) {
				Debug.Log (ex.ToString ());
				ex = ex.InnerException;
			}
		}
		
		
		
		
	}
	
	
	public static bool ValidateServerCertificate (
		object sender,
		System.Security.Cryptography.X509Certificates.X509Certificate certificate,
		X509Chain chain,
		System.Net.Security.SslPolicyErrors sslPolicyErrors)
	{
		
		return true;
	}
	
	void OnGUI(){
		GUI.Label (new Rect (0, 0, 100, 100), s);
		if (GUI.Button (new Rect (20, 20, 100, 100), s)) {
			Application.Quit();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}




	/*private static bool CertificateValidationCallBack(
		object sender,
		System.Security.Cryptography.X509Certificates.X509Certificate certificate,
		System.Security.Cryptography.X509Certificates.X509Chain chain,
		System.Net.Security.SslPolicyErrors sslPolicyErrors)
	{
		Debug.Log("aa");
		// If the certificate is a valid, signed certificate, return true.
		if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
		{
			return true;
		}
		
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
		{
			if (chain != null && chain.ChainStatus != null)
			{
				foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
				{
					if ((certificate.Subject == certificate.Issuer) &&
					    (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
					{
						// Self-signed certificates with an untrusted root are valid. 
						continue;
					}
					else
					{
						if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							// If there are any other errors in the certificate chain, the certificate is invalid,
							// so the method returns false.
							return false;
						}
					}
				}
			}
			
			// When processing reaches this line, the only errors in the certificate chain are 
			// untrusted root errors for self-signed certificates. These certificates are valid
			// for default Exchange server installations, so return true.
			return true;
		}
		else
		{
			// In all other cases, return false.
			return false;
		}
	}*/
	
}