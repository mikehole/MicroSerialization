using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroSerialization.Tests
{

    [TestClass]
    public class BitConversionTests
    {
        [TestMethod]
        public void TestNumbers()
        {

            #region int

            int testint = int.MaxValue;

            byte[] intbytes = MicroSerialization.Mf.BitConverter.GetBytes(testint);

            var resultint = BitConverter.ToInt32(intbytes, 0);

            Assert.AreEqual(testint, resultint);

            #endregion //int

            #region bool

            bool testbool = true;

            byte[] boolbyte = MicroSerialization.Mf.BitConverter.GetBytes(testbool);

            var resultbool = BitConverter.ToBoolean(boolbyte, 0);

            Assert.AreEqual(testbool, resultbool);

            #endregion bool

            #region char

            char testchar = 'T';

            byte[] charbyte = MicroSerialization.Mf.BitConverter.GetBytes(testchar);

            var resultchar = BitConverter.ToChar(charbyte, 0);

            Assert.AreEqual(testchar, resultchar);

            #endregion

            #region short

            short testshort = short.MaxValue;

            byte[] shortbyte = MicroSerialization.Mf.BitConverter.GetBytes(testshort);

            short resultshort = BitConverter.ToInt16(shortbyte, 0);

            Assert.AreEqual(testshort, resultshort);

            #endregion

            #region ushort

            ushort testushort = ushort.MaxValue;

            byte[] ushortbyte = MicroSerialization.Mf.BitConverter.GetBytes(testushort);

            ushort resultushort = (ushort)BitConverter.ToInt16(ushortbyte, 0);

            Assert.AreEqual(testushort, resultushort);            

            #endregion
            
            #region uint

            uint testuint = uint.MaxValue;

            byte[] uintbyte = MicroSerialization.Mf.BitConverter.GetBytes(testuint);

            uint resultuint = BitConverter.ToUInt32(uintbyte, 0);

            Assert.AreEqual(testuint, resultuint);            
            
            #endregion
            
            #region long

            long testlong = long.MaxValue;

            byte[] longbyte = MicroSerialization.Mf.BitConverter.GetBytes(testlong);

            long resultlong = BitConverter.ToInt64(longbyte, 0);

            Assert.AreEqual(testlong, resultlong);            
            
            #endregion
            
            #region ulong

            ulong testulong = ulong.MaxValue;

            byte[] ulongbyte = MicroSerialization.Mf.BitConverter.GetBytes(testulong);

            ulong resultulong = BitConverter.ToUInt64(ulongbyte, 0);

            Assert.AreEqual(testulong, resultulong);            
            
            #endregion
            
            #region float

            float testfloat = float.MaxValue;

            byte[] floatbyte = MicroSerialization.Mf.BitConverter.GetBytes(testfloat);

            float resultfloat = BitConverter.ToSingle(floatbyte, 0);

            Assert.AreEqual(testfloat, resultfloat);            
            
            #endregion

            #region double

            double testdouble = double.MaxValue;

            byte[] doublebyte = MicroSerialization.Mf.BitConverter.GetBytes(testdouble);

            double resultdouble = BitConverter.ToDouble(doublebyte, 0);

            Assert.AreEqual(testdouble, resultdouble);            

            #endregion
        }
    }
}