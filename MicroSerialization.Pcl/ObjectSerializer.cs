﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MicroSerialization.Pcl
{
    public class ObjectSerializer<T>
    {
        private Stream _stream;

        private Stream _streamToWriteTo;

        
        List<byte[]> _objectBytes;

        public ObjectSerializer(Stream stream)
        {
            _streamToWriteTo = stream;
            
            _stream = new MemoryStream();

            _objectBytes = new List<byte[]>();
        }
        
        public long SaveToStream(T objectToSave)
        {
            GetTypeInfo();

            var fieldInfos = typeof(T).GetFields();

            foreach (var field in fieldInfos)
            {
                GetFieldBytes(field, objectToSave);
            }

            //WriteTypeLength();

            WriteTypeData();

            _stream.Flush();

            _stream.Seek(0, 0);

            byte[] dataToSend = new byte[_stream.Length];

            _stream.Read(dataToSend, 0, dataToSend.Length);

            _streamToWriteTo.Write(dataToSend, 0, dataToSend.Length);
            _streamToWriteTo.Flush();

            return dataToSend.Length;
        }

        public void GetTypeInfo()
        {
            byte[] stringbyts = System.Text.UTF8Encoding.UTF8.GetBytes(typeof(T).FullName + "\0");
            _objectBytes.Add(stringbyts);
        }

        public void WriteTypeLength()
        {
            Int32  contentLength = (Int32) _objectBytes.Sum(b => b.Length);
            byte[] contentLengthBytes = BitConverter.GetBytes((Int32)contentLength);
            _stream.Write(contentLengthBytes, 0, contentLengthBytes.Length);
        }

        public void WriteTypeData()
        {
            foreach (var databytes in _objectBytes)
            {
                _stream.Write(databytes, 0, databytes.Length);
            }
        }

        public void GetFieldBytes(FieldInfo propertyInto, T objectToSave)
        {
            object value = propertyInto.GetValue(objectToSave);

            byte[] valueBytes = null;

            switch (propertyInto.FieldType.Name)
            {
                case "Int32":
                    valueBytes = BitConverter.GetBytes((int)value);
                    break;
                case "Boolean":
                    valueBytes = BitConverter.GetBytes((bool)value);
                    break;
                case "String":
                    valueBytes = System.Text.UTF8Encoding.UTF8.GetBytes((string)value + "\0");
                    break;
            }

            if (valueBytes != null)
                _objectBytes.Add(valueBytes);
            else
                throw new Exception("Unsupported field type in class");
        }
    }
}
