using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace DevAge.Security.Cryptography
{
	/// <summary>
	/// Utilities
	/// </summary>
	public class Utilities
	{
		/// <summary>
		/// Crypt and encrypt methods using DES
		/// </summary>
		public class DES
		{
			#region Cryptography Code

			/// <summary>
			/// Encrypt the specified string using DES
			/// </summary>
			/// <param name="p_strInput">String to encrypt</param>
			/// <param name="p_Key8chars">Must be of 8 characters length</param>
			/// <returns></returns>
			public static string EncryptString(string p_strInput, string p_Key8chars)
			{
				string tmp;
				using (MemoryStream l_InputStream = new MemoryStream())
				{
					StreamWriter l_InputStreamWriter = new StreamWriter(l_InputStream);
					l_InputStreamWriter.Write(p_strInput);
					l_InputStreamWriter.Flush();
					l_InputStream.Seek(0,SeekOrigin.Begin);

					using (MemoryStream l_OutputStream = new MemoryStream())
					{
						EncryptStream(l_InputStream,l_OutputStream,p_Key8chars);
						l_OutputStream.Flush();
						l_OutputStream.Seek(0,SeekOrigin.Begin);

						tmp = Convert.ToBase64String(l_OutputStream.ToArray());

						l_OutputStream.Close();
					}

					l_InputStreamWriter.Close();
				}

				return tmp;
			}

			/// <summary>
			/// Decrypt the specified string using DES
			/// </summary>
			/// <param name="p_strInput">String to decrypt</param>
			/// <param name="p_Key8chars">Must be of 8 characters length</param>
			/// <returns></returns>
			public static string DecryptString(string p_strInput, string p_Key8chars)
			{
				string tmp;
				using (MemoryStream l_InputStream = new MemoryStream())
				{
					byte[] l_InputArray = Convert.FromBase64String(p_strInput);
					l_InputStream.Write(l_InputArray,0,l_InputArray.Length);
					l_InputStream.Flush();
					l_InputStream.Seek(0,SeekOrigin.Begin);

					using (MemoryStream l_OutputStream = new MemoryStream())
					{
						DecryptStream(l_InputStream,l_OutputStream,p_Key8chars);

						StreamReader l_OutputStreamReader = new StreamReader(l_OutputStream);
						tmp = l_OutputStreamReader.ReadToEnd();

						l_OutputStreamReader.Close();
					}

					l_InputStream.Close();
				}

				return tmp;
			}
			/// <summary>
			/// Encrypt the specified stream using DES
			/// </summary>
			/// <param name="p_StreamInput"></param>
			/// <param name="p_StreamOutput"></param>
			/// <param name="p_Key8chars">Must be of 8 characters length</param>
			public static void EncryptStream(Stream p_StreamInput, Stream p_StreamOutput, string p_Key8chars)
			{
				DESCryptoServiceProvider DESProvider = new DESCryptoServiceProvider();
				DESProvider.Key = ASCIIEncoding.ASCII.GetBytes(p_Key8chars);
				DESProvider.IV = ASCIIEncoding.ASCII.GetBytes(p_Key8chars);
				ICryptoTransform DESEncrypt = DESProvider.CreateEncryptor();

				using (CryptoStream cryptoStream = new CryptoStream(p_StreamOutput, DESEncrypt, CryptoStreamMode.Write))
				{
					byte[] bytearrayinput = new byte[p_StreamInput.Length];
					p_StreamInput.Read(bytearrayinput, 0, bytearrayinput.Length);
					cryptoStream.Write(bytearrayinput, 0, bytearrayinput.Length);
					cryptoStream.FlushFinalBlock();
				}
			}

			/// <summary>
			/// Decrypt the specified stream using DES
			/// </summary>
			/// <param name="p_StreamInput"></param>
			/// <param name="p_StreamOutput"></param>
			/// <param name="p_Key8chars">Must be of 8 characters length</param>
			public static void DecryptStream(Stream p_StreamInput, Stream p_StreamOutput, string p_Key8chars)
			{
				DESCryptoServiceProvider DESProvider = new DESCryptoServiceProvider();
				DESProvider.Key = ASCIIEncoding.ASCII.GetBytes(p_Key8chars);
				DESProvider.IV = ASCIIEncoding.ASCII.GetBytes(p_Key8chars);
				ICryptoTransform desDecrypt= DESProvider.CreateDecryptor();

				using (CryptoStream cryptostreamDecr = new CryptoStream(p_StreamOutput, desDecrypt, CryptoStreamMode.Write))
				{
					byte[] buffer = new byte[p_StreamInput.Length];
					p_StreamInput.Read(buffer,0,buffer.Length);
					cryptostreamDecr.Write(buffer,0,buffer.Length);
					cryptostreamDecr.FlushFinalBlock();
				}
				p_StreamOutput.Seek(0,SeekOrigin.Begin);
			}
			#endregion
		}

		/// <summary>
		/// Password utilities using SH1 alghoritm
		/// </summary>
		public class SHA1
		{
			/// <summary>
			/// Hash the string p_Password using SH1 alghoritm (SHA1CryptoServiceProvider). 
			/// </summary>
			/// <param name="p_Password"></param>
			/// <returns></returns>
			public static string HashPassword(string p_Password)
			{
				SHA1CryptoServiceProvider l_shaProvider = new SHA1CryptoServiceProvider(); 

				byte[] data = Encoding.UTF8.GetBytes(p_Password);

				byte[] result = l_shaProvider.ComputeHash(data);

				return Convert.ToBase64String(result);
			}
		}


        /// <summary>
        /// An utility class with some method to signing and verify xml documents
        /// </summary>
        public class XmlDigitalSign
        {
            /// <summary>
            /// Generate the keys (public and private)
            /// </summary>
            /// <param name="keyPubPri">Public and private key</param>
            /// <param name="keyPub">Public key</param>
            public static void GenerateKeys(out string keyPubPri, out string keyPub)
            {
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(1024);
                keyPubPri = rsa.ToXmlString(true);
                keyPub = rsa.ToXmlString(false);
                rsa.Clear();
            }

            /// <summary>
            /// Create a signature xml element for the specified xml document and private key
            /// </summary>
            /// <param name="xmlToSign"></param>
            /// <param name="keyPubPri">Private+public key</param>
            /// <returns></returns>
            public static System.Xml.XmlElement CreateSignature(System.Xml.XmlDocument xmlToSign, string keyPubPri)
            {
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
                rsa.FromXmlString(keyPubPri);

                System.Security.Cryptography.Xml.SignedXml sx = new System.Security.Cryptography.Xml.SignedXml(xmlToSign);
                sx.SigningKey = rsa;

                // Create a reference to be signed
                System.Security.Cryptography.Xml.Reference reference = new System.Security.Cryptography.Xml.Reference("");

                // Set the canonicalization method for the document.
                sx.SignedInfo.CanonicalizationMethod = System.Security.Cryptography.Xml.SignedXml.XmlDsigCanonicalizationUrl; // No comments.

                // Add an enveloped transformation to the reference.
                System.Security.Cryptography.Xml.XmlDsigEnvelopedSignatureTransform env = new System.Security.Cryptography.Xml.XmlDsigEnvelopedSignatureTransform(false);
                reference.AddTransform(env);

                sx.AddReference(reference);

                sx.ComputeSignature();

                return sx.GetXml();
            }

            /// <summary>
            /// Create a signed xml document. Add a signature alement to the specified document using the specified private key.
            /// </summary>
            /// <param name="xmlToSign"></param>
            /// <param name="keyPubPri">Private+public key</param>
            /// <returns></returns>
            public static System.Xml.XmlDocument CreateSignedDoc(System.Xml.XmlDocument xmlToSign, string keyPubPri)
            {
                System.Xml.XmlElement signature = CreateSignature(xmlToSign, keyPubPri);

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.AppendChild(doc.ImportNode(xmlToSign.DocumentElement, true));

                //Append the signature tag to the root element
                doc.DocumentElement.PrependChild(doc.ImportNode(signature, true));

                return doc;
            }

            /// <summary>
            /// Check the signature of the specified signed document (created with CreateSignedDoc) using the specified public key.
            /// </summary>
            /// <param name="signedDoc"></param>
            /// <param name="keyPub">Public key</param>
            /// <returns></returns>
            public static bool CheckSignature(System.Xml.XmlDocument signedDoc, string keyPub)
            {
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
                rsa.FromXmlString(keyPub);

                // Create a new SignedXml object and pass it
                // the XML document class.
                System.Security.Cryptography.Xml.SignedXml sx = new System.Security.Cryptography.Xml.SignedXml(signedDoc);

                // Load the first <signature> node.  
                sx.LoadXml(GetSignatureFromSignedDoc(signedDoc));

                // Check the signature and return the result.
                return sx.CheckSignature(rsa);
            }

            /// <summary>
            /// Extract the signature element from the specified signed document.
            /// </summary>
            /// <param name="signedDoc"></param>
            /// <returns></returns>
            public static System.Xml.XmlElement GetSignatureFromSignedDoc(System.Xml.XmlDocument signedDoc)
            {
                // Find the "Signature" node and create a new
                // XmlNodeList object.
                System.Xml.XmlNodeList nodeList = signedDoc.GetElementsByTagName("Signature");

                // Throw an exception if no signature was found.
                if (nodeList.Count <= 0)
                {
                    throw new System.Security.Cryptography.CryptographicException("Verification failed: No Signature was found in the document.");
                }

                // Supports one signature for
                // the entire XML document.  Throw an exception 
                // if more than one signature was found.
                if (nodeList.Count >= 2)
                {
                    throw new System.Security.Cryptography.CryptographicException("Verification failed: More that one signature was found for the document.");
                }

                return (System.Xml.XmlElement)nodeList[0];
            }

            /// <summary>
            /// Create a new XmlDocument from the specified signed document removing the signature element.
            /// </summary>
            /// <param name="signedDoc"></param>
            /// <returns></returns>
            public static System.Xml.XmlDocument CreateDocWithoutSignature(System.Xml.XmlDocument signedDoc)
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.AppendChild(doc.ImportNode(signedDoc.DocumentElement, true));

                System.Xml.XmlElement signature = GetSignatureFromSignedDoc(doc);
                signature.ParentNode.RemoveChild(signature);

                return doc;
            }
        }

	}


}
