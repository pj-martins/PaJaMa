using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace PaJaMa.Common
{
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
		#region IXmlSerializable Members
		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}
		void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
			bool wasEmpty = reader.IsEmptyElement;
			reader.Read();
			if (wasEmpty)
				return;
			while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");
				reader.ReadStartElement("key");
				TKey key = (TKey)keySerializer.Deserialize(reader);
				reader.ReadEndElement();
				reader.ReadStartElement("value");
				TValue value = (TValue)valueSerializer.Deserialize(reader);
				reader.ReadEndElement();
				this.Add(key, value);
				reader.ReadEndElement();
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}
		void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
			foreach (TKey key in this.Keys)
			{
				writer.WriteStartElement("item");
				writer.WriteStartElement("key");
				keySerializer.Serialize(writer, key);
				writer.WriteEndElement();
				writer.WriteStartElement("value");
				TValue value = this[key];
				valueSerializer.Serialize(writer, value);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}
		#endregion
	}

	[Serializable()]
	public class Pairs<TKey, TValue> : List<Pair<TKey, TValue>> where TValue : new()
	{
		public TValue this[TKey key]
		{
			get
			{
				Pair<TKey, TValue> pair = this.Find(p => p.Key.Equals(key));
				if (pair == null)
				{
					pair = new Pair<TKey, TValue>() { Key = key, Value = new TValue() };
					this.Add(pair);
				}
				return pair.Value;
			}
			set
			{
				Pair<TKey, TValue> pair = this.Find(p => p.Key.Equals(key));
				if (pair != null)
					pair.Value = value;
				else
					this.Add(new Pair<TKey, TValue>() { Key = key, Value = value });
			}
		}

		public new void Add(Pair<TKey, TValue> item)
		{
			if (this.Exists(p => p.Key.Equals(item.Key)))
				throw new Exception("Item with same key arleady exists.");

			base.Add(item);
		}

		public new void Insert(int index, Pair<TKey, TValue> item)
		{
			if (this.Exists(p => p.Key.Equals(item.Key)))
				throw new Exception("Item with same key arleady exists.");

			base.Insert(index, item);
		}
	}

	[Serializable()]
	public class Pair<TKey, TValue>
	{
		public TKey Key { get; set; }
		public TValue Value { get; set; }
	}

}
