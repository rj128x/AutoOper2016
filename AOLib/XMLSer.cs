﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AOLib {
	public class XMLSer<T> {
		public static void toXML(T obj, string fileName) {
			XmlSerializer mySerializer = new XmlSerializer(typeof(T));
			// To write to a file, create a StreamWriter object.
			StreamWriter myWriter = new StreamWriter(fileName);
			mySerializer.Serialize(myWriter, obj);
			myWriter.Close();
		}

		public static T fromXML(string fileName) {
			try {
				XmlSerializer mySerializer = new XmlSerializer(typeof(T));
				// To read the file, create a FileStream.
				FileStream myFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				// Call the Deserialize method and cast to the object type.
				T data = (T)mySerializer.Deserialize(myFileStream);
				myFileStream.Close();
				return data;
			}
			catch (Exception e) {
				return default(T);
			}
		}
	}
}