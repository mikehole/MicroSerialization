using System;
using System.Reflection;

namespace MicroSerialization.Mf
{
    public class ObjectSerializer
    {
        private byte[] newline = new byte[] { 13, 10 };

        public object LoadFromBytes(byte[] objectData)
        {
            int CurrentIndex = 0;

            byte[] typnameByts = GetNextString(objectData, CurrentIndex);

            string typeName = new string(System.Text.UTF8Encoding.UTF8.GetChars(typnameByts));

            Type[] x = new Type[0];

            ConstructorInfo typeConstructor = Type.GetType(typeName).GetConstructor(x);

            int lengthOfData = BitConverter.ToInt32(objectData, typnameByts.Length + 2);

            //do a little check
            if (typnameByts.Length + 2 + 4 + lengthOfData != objectData.Length)
                return null;

            Int32 currentPos = objectData.Length - lengthOfData;

            object item = typeConstructor.Invoke(null);

            foreach (var field in item.GetType().GetFields())
            {
                int movement = 0;

                object val = GetFieldValue(objectData, field, currentPos, out movement);

                currentPos += movement;

                field.SetValue(item, val);
            }

            return item;
        }

        byte[] GetNextString(byte[] objectData, int offeset)
        {
            int delimiter = Array.IndexOf(objectData, (byte)13);

            byte[] stringData = new byte[delimiter];

            Array.Copy(objectData, stringData, delimiter);

            return stringData;
        }

        object GetFieldValue(byte[] objectData, FieldInfo fieldInfo, int position, out int movement)
        {
            object returnValue = null;

            movement = 0;

            switch (fieldInfo.FieldType.Name)
            {
                case "Int16":
                    returnValue = BitConverter.ToInt16(objectData, position);
                    movement = 2;
                    break;
                case "Int32":
                    returnValue = BitConverter.ToInt32(objectData, position);
                    movement = 4;
                    break;
                case "Int64":
                    returnValue = BitConverter.ToInt64(objectData, position);
                    movement = 8;
                    break;

                case "Single":
                    returnValue = BitConverter.ToSingle(objectData, position);
                    movement = 4;
                    break;

                case "Double":
                    returnValue = BitConverter.ToDouble(objectData, position);
                    movement = 8;
                    break;
                
                case "Boolean":
                    returnValue = BitConverter.ToBoolean(objectData, position);
                    movement = 1;
                    break;






                case "String":
                    int EndIndex = Array.IndexOf(objectData, (byte)0, position);

                    byte[] stringData = new byte[EndIndex - position];

                    Array.Copy(objectData, position, stringData, 0, EndIndex - position);

                    returnValue = new string(System.Text.UTF8Encoding.UTF8.GetChars(stringData));

                    movement = EndIndex - position + 1;

                    break;

            }

            return returnValue;
        }
        
    }
}
