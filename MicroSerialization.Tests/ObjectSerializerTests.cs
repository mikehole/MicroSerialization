using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MicroSerialization.Tests
{

    public class TestClass
    {
        public int TestInt;

        public int TestInt2;

        public string testString;

        public string testStringNull;

        public bool testBool;

        public float testFloat;

        public double testDouble;

        public uint TestUint;

    }
    
    
    [TestClass]
    public class ObjectSerializerTests
    {
        [TestMethod]
        public void TestObjectSerialisation()
        {
            MemoryStream testStream = new MemoryStream();
            
            var testSer = new MicroSerialization.Pcl.ObjectSerializer<TestClass>(
                testStream);

            testSer.SaveToStream(new TestClass() { TestInt = int.MaxValue
                , TestInt2 = 500
                , testString = "Hello Mike"
                , testBool = true 
                ,testDouble = double.MaxValue
            });

            testStream.Seek(0, SeekOrigin.Begin);

            Assert.AreNotEqual(testStream.Length, 0);

            byte[] streamData = new byte[testStream.Length];

            testStream.Read(streamData, 0, streamData.Length);

            var test2 = new MicroSerialization.Mf.ObjectSerializer();

            test2.LoadFromBytes(streamData);

        }
    }
}
