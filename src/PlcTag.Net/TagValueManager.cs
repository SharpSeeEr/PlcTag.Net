using System;
using System.Collections.Generic;
using System.Text;

namespace PlcTag
{
    /// <summary>
    /// Tag local manipulaton value
    /// </summary>
    internal class TagValueManager
    {
        public Func<int, int, object> GetValue { get; }
        public Func<int, object, int, int> SetValue { get; }
        public int Size { get; }

        private TagValueManager(Func<int, int, object> getValue, Func<int, object, int, int> setValue, int size)
        {
            GetValue = getValue;
            SetValue = setValue;
            Size = size;
        }

        private const int INT8 = 1;

        private const int UINT8 = INT8;
        private const int BOOL = 1;
        private const int INT16 = 2;
        private const int UINT16 = INT16;
        private const int INT32 = 4;
        private const int UINT32 = INT32;
        private const int INT64 = 8;
        private const int UINT64 = INT64;
        private const int FLOAT32 = 4;
        private const int FLOAT64 = 8;
        //private const int STRING = 88;

        private static readonly IReadOnlyDictionary<Type, TagValueManager> _map = new Dictionary<Type, TagValueManager>()
        {
            { typeof(Int64), new TagValueManager(GetInt64, SetInt64, INT64) },
            { typeof(UInt64), new TagValueManager(GetUInt64, SetUInt64, UINT64) },
            { typeof(Int32), new TagValueManager(GetInt32, SetInt32, INT32) },
            { typeof(UInt32), new TagValueManager(GetUInt32, SetUInt32, UINT32) },
            { typeof(Int16), new TagValueManager(GetInt16, SetInt16, INT16) },
            { typeof(UInt16), new TagValueManager(GetUInt16, SetUInt16, UINT16) },
            { typeof(sbyte), new TagValueManager(GetInt8, SetInt8, INT8) },
            { typeof(byte), new TagValueManager(GetUInt8, SetUInt8, UINT8) },
            { typeof(float), new TagValueManager(GetFloat32, SetFloat32, FLOAT32) },
            { typeof(double), new TagValueManager(GetFloat64, SetFloat64, FLOAT64) },
            //{ typeof(string), new TagValueManager(GetString, SetString, STRING) },
            { typeof(bool), new TagValueManager(GetBool, SetBool, BOOL) },
        };

        public static TagValueManager GetTagValueManager<T>()
        {
            try
            {
                return _map[typeof(T)];
            }
            catch (Exception)
            {
                throw new InvalidOperationException("GetTagValueManager: Unknown data size");
            }
        }

        /// <summary>
        /// Get local value UInt16
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetUInt16(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_uint16(tagHandle, offset);
            if (result == ushort.MaxValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value UInt16
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetUInt16(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_uint16(tagHandle, offset, (UInt16)value);
        }

        /// <summary>
        /// Get local value Int16
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetInt16(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_int16(tagHandle, offset);
            if (result == short.MinValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value Int16
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetInt16(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_int16(tagHandle, offset, (Int16)value);
        }

        /// <summary>
        /// Get local value UInt8
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetUInt8(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_uint8(tagHandle, offset);
            if (result == byte.MaxValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value UInt8
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetUInt8(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_uint8(tagHandle, offset, (byte)value);
        }

        /// <summary>
        /// Get local value Int8
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetInt8(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_int8(tagHandle, offset);
            if (result == sbyte.MinValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value Int8
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetInt8(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_int8(tagHandle, offset, (sbyte)value);
        }

        /// <summary>
        /// Get local value UInt32
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetUInt32(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_uint32(tagHandle, offset);
            if (result == uint.MaxValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value UInt32
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetUInt32(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_uint32(tagHandle, offset, (UInt32)value);
        }

        /// <summary>
        /// Get local value Int32
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetInt32(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_int32(tagHandle, offset);
            if (result == int.MinValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value Int32
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetInt32(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_int32(tagHandle, offset, (Int32)value);
        }

        /// <summary>
        /// Get local value UInt64
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetUInt64(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_uint64(tagHandle, offset);
            if (result == ulong.MaxValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value UInt64
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetUInt64(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_uint64(tagHandle, offset, (UInt64)value);
        }

        /// <summary>
        /// Get local value Int64
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static private object GetInt64(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_int64(tagHandle, offset);
            if (result == long.MinValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value Int64
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetInt64(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_int64(tagHandle, offset, (Int64)value);
        }

        /// <summary>
        /// Get local value Float32
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetFloat32(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_float32(tagHandle, offset);
            if (result == float.MinValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value Float32
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetFloat32(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_float32(tagHandle, offset, (float)value);
        }

        /// <summary>
        /// Get local value Float
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetFloat64(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_float64(tagHandle, offset);
            if (result == double.MinValue) throw new TagGetValueException();
            return result;
        }

        /// <summary>
        /// Set local value Float
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetFloat64(int tagHandle, object value, int offset)
        {
            return NativeLibrary.plc_tag_set_float64(tagHandle, offset, (double)value);
        }

        /// <summary>
        /// Get bit from index
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object GetBool(int tagHandle, int offset)
        {
            var result = NativeLibrary.plc_tag_get_uint8(tagHandle, offset);
            if (result == byte.MaxValue) throw new TagGetValueException();
            return result > 0;
        }

        /// <summary>
        /// Set bit from index and value
        /// </summary>
        /// <param name="tagHandle"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private static int SetBool(int tagHandle, object value, int offset)
        {
            byte byteValue = (byte)((bool)value ? 1 : 0);
            return NativeLibrary.plc_tag_set_uint8(tagHandle, offset, byteValue);
        }
    }
}
