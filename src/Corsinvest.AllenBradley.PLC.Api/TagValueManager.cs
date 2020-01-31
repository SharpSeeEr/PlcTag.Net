using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Tag local manipulaton value
    /// </summary>
    public class TagValueManager<T>
    {
        private readonly ITag<T> _tag;

        private readonly IReadOnlyDictionary<Type, Func<int, object>> _getFuncMap;
        private readonly IReadOnlyDictionary<Type, Action<object, int>> _setFuncMap;

        /// <summary>
        /// Byte Length string in header
        /// </summary>
        private const byte BYTE_HEADER_LENGTH_STRING = 4;

        private const byte MAX_LENGTH_STRING = 82;

        internal TagValueManager(ITag<T> tag)
        {
            _tag = tag;
            _getFuncMap = new Dictionary<Type, Func<int, object>>()
            {
                { typeof(Int64), GetInt64 },
                { typeof(UInt64), GetUInt64 },
                { typeof(Int32), GetInt32 },
                { typeof(UInt32), GetUInt32 },
                { typeof(Int16), GetInt16 },
                { typeof(UInt16), GetUInt16 },
                { typeof(sbyte), GetInt8 },
                { typeof(byte), GetUInt8 },
                { typeof(float), GetFloat32 },
                { typeof(double), GetFloat64 },
                { typeof(string), GetString },
                { typeof(bool), GetBool },
            };
            _setFuncMap = new Dictionary<Type, Action<object, int>>()
            {
                { typeof(Int64), SetInt64 },
                { typeof(UInt64), SetUInt64 },
                { typeof(Int32), SetInt32 },
                { typeof(UInt32), SetUInt32 },
                { typeof(Int16), SetInt16 },
                { typeof(UInt16), SetUInt16 },
                { typeof(sbyte), SetInt8 },
                { typeof(byte), SetUInt8 },
                { typeof(float), SetFloat32 },
                { typeof(double), SetFloat64 },
                { typeof(string), SetString },
                { typeof(bool), SetBool },
            };
        }

        /// <summary>
        /// Get local value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal T Get(int offset = 0)
        {
            try
            {
                return (T)_getFuncMap[_tag.ValueType](offset);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("GetValue: Unknown Tag data size");
            }
        }

        /// <summary>
        /// Set local value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        internal void Set(T value, int offset = 0)
        {
            try
            {
                _setFuncMap[_tag.ValueType](value, offset);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("SetValue: Unknown Tag data size");
            }
        }

        /// <summary>
        /// Get local value UInt16
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetUInt16(int offset)
        {
             return NativeLibrary.plc_tag_get_uint16(_tag.Handle, offset);
        }

        /// <summary>
        /// Set local value UInt16
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetUInt16(object value, int offset)
        {
             NativeLibrary.plc_tag_set_uint16(_tag.Handle, offset, (UInt16)value);
        }

        /// <summary>
        /// Get local value Int16
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetInt16(int offset)
        {
             return NativeLibrary.plc_tag_get_int16(_tag.Handle, offset);
        }

        /// <summary>
        /// Set local value Int16
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetInt16(object value, int offset)
        {
             NativeLibrary.plc_tag_set_int16(_tag.Handle, offset, (Int16)value);
        }

        /// <summary>
        /// Get local value UInt8
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetUInt8(int offset)
        { 
            return NativeLibrary.plc_tag_get_uint8(_tag.Handle, offset); 
        }

        /// <summary>
        /// Set local value UInt8
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetUInt8(object value, int offset)
        {
            NativeLibrary.plc_tag_set_uint8(_tag.Handle, offset, (byte)value);
        }

        /// <summary>
        /// Get local value Int8
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetInt8(int offset)
        {
             return NativeLibrary.plc_tag_get_int8(_tag.Handle, offset);
        }

        /// <summary>
        /// Set local value Int8
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetInt8(object value, int offset)
        {
             NativeLibrary.plc_tag_set_int8(_tag.Handle, offset, (sbyte)value);
        }

        /// <summary>
        /// Get local value UInt32
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetUInt32(int offset)
        {
             return NativeLibrary.plc_tag_get_uint32(_tag.Handle, offset);
        }

        /// <summary>
        /// Set local value UInt32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetUInt32(object value, int offset)
        {
             NativeLibrary.plc_tag_set_uint32(_tag.Handle, offset, (UInt32)value);
        }

        /// <summary>
        /// Get local value Int32
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetInt32(int offset)
        {
             return NativeLibrary.plc_tag_get_int32(_tag.Handle, offset);
        }

        /// <summary>
        /// Set local value Int32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetInt32(object value, int offset)
        {
             NativeLibrary.plc_tag_set_int32(_tag.Handle, offset, (Int32)value);
        }

        /// <summary>
        /// Get local value UInt64
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetUInt64(int offset)
        {
             return NativeLibrary.plc_tag_get_uint64(_tag.Handle, offset);
        }

        /// <summary>
        /// Set local value UInt64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetUInt64(object value, int offset)
        {
             NativeLibrary.plc_tag_set_uint64(_tag.Handle, offset, (UInt64)value);
        }

        /// <summary>
        /// Get local value Int64
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public object GetInt64(int offset)
        {
             return NativeLibrary.plc_tag_get_int64(_tag.Handle, offset);
        }

        /// <summary>
        /// Set local value Int64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetInt64(object value, int offset)
        {
             NativeLibrary.plc_tag_set_int64(_tag.Handle, offset, (Int64)value);
        }

        /// <summary>
        /// Get local value Float32
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetFloat32(int offset)
        {
             return NativeLibrary.plc_tag_get_float32(_tag.Handle, offset);
        }

        /// <summary>
        /// Set local value Float32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetFloat32(object value, int offset)
        {
             NativeLibrary.plc_tag_set_float32(_tag.Handle, offset, (float)value);
        }

        /// <summary>
        /// Get local value Float
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetFloat64(int offset)
        {
             return NativeLibrary.plc_tag_get_float64(_tag.Handle, offset);
        }

        /// <summary>
        /// Set local value Float
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetFloat64(object value, int offset)
        {
             NativeLibrary.plc_tag_set_float64(_tag.Handle, offset, (double)value);
        }

        /// <summary>
        /// Get local value String
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetString(int offset)
        {
            var sb = new StringBuilder();

            //max length string
            var length = NativeLibrary.plc_tag_get_int32(_tag.Handle, offset);

            //read only length of string
            for (var i = 0; i < length; i++)
            {
                var charOffset = offset + BYTE_HEADER_LENGTH_STRING + i;
                var character = (char)NativeLibrary.plc_tag_get_uint8(_tag.Handle, charOffset);
                sb.Append(character);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Set local value String
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetString(object value, int offset)
        {
            var strValue = value as string;
            if (strValue.Length > MAX_LENGTH_STRING) { throw new ArgumentOutOfRangeException($"Length strign <= {MAX_LENGTH_STRING}!"); }

            //set length
            NativeLibrary.plc_tag_set_int32(_tag.Handle, strValue.Length, offset);

            var strIdx = 0;

            //copy data
            for (strIdx = 0; strIdx < strValue.Length; strIdx++)
            {
                var charOffset = offset + BYTE_HEADER_LENGTH_STRING + strIdx;
                NativeLibrary.plc_tag_set_uint8(_tag.Handle, charOffset, (byte)strValue[strIdx]);
            }

            // pad with zeros
            for (; strIdx < MAX_LENGTH_STRING; strIdx++) 
            {
                var charOffset = offset + BYTE_HEADER_LENGTH_STRING + strIdx;
                NativeLibrary.plc_tag_set_uint8(_tag.Handle, charOffset, 0);
            }
        }

        /// <summary>
        /// Get bit from index
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private object GetBool(int offset) {
            return NativeLibrary.plc_tag_get_uint8(_tag.Handle, offset) > 0;
        }

        /// <summary>
        /// Set bit from index and value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        private void SetBool(object value, int offset)
        {
            byte byteValue = (byte)((bool)value ? 1 : 0);
            NativeLibrary.plc_tag_set_uint8(_tag.Handle, offset, byteValue);
        }
    }
}
