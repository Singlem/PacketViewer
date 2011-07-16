using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WowTools.Core
{
    public abstract class Parser
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        public void Append(string str)
        {
            stringBuilder.Append(str);
        }

        public void AppendLine()
        {
            stringBuilder.AppendLine();
        }

        public void AppendLine(string str)
        {
            stringBuilder.AppendLine(str);
        }

        public void AppendFormat(string format, params object[] args)
        {
            stringBuilder.AppendFormat(format, args);
        }

        public void AppendFormatLine(string format, params object[] args)
        {
            stringBuilder.AppendFormat(format, args).AppendLine();
        }

        public void CheckPacket()
        {
            if (Reader.BaseStream.Position != Reader.BaseStream.Length)
            {
                string msg = String.Format("{0}: Packet size changed, should be {1} instead of {2}", Packet.Code, Reader.BaseStream.Position, Reader.BaseStream.Length);
                MessageBox.Show(msg);
            }
        }

        protected Packet Packet { get; private set; }

        protected BinaryReader Reader { get; private set; }

        protected Parser packet;

        private byte _currentByte;
        // position of the next bit in _currentByte to be read
        private sbyte _bitPos = -1;

        protected Parser()
        {
            packet = this;  // an alias for parser compatibility with SilinoronParser
        }

        public virtual void Initialize(Packet packet)
        {
            Packet = packet;

            if (packet != null)
            {
                Reader = Packet.CreateReader();
                Parse();
                CheckPacket();
            }
        }

        public abstract void Parse();

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        public byte ReadUInt8(string format, params object[] args)
        {
            var ret = Reader.ReadByte();
            if (args.Length == 0 && !format.Contains("{0"))
                format += ": {0}";
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        public byte ReadByte(string format, params object[] args)
        {
            return ReadUInt8(format, args); // alias
        }

        // for enums
        public T ReadUInt8<T>(string format, params object[] args)
        {
            var obj = Enum.ToObject(typeof(T), Reader.ReadByte());
            if (args.Length == 0 && !format.Contains("{0"))
                format += ": {0}";
            AppendFormatLine(format, MergeArguments(args, obj));
            return (T)obj;
        }

        public uint ReadUInt16(string format, params object[] args)
        {
            var ret = Reader.ReadUInt16();
            if (args.Length == 0 && !format.Contains("{0"))
                format += ": {0}";
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        public int ReadInt32(string format, params object[] args)
        {
            var ret = Reader.ReadInt32();
            if(args.Length == 0 && !format.Contains("{0"))
                format += ": {0}";
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        public uint ReadUInt32(string format, params object[] args)
        {
            var ret = Reader.ReadUInt32();
            if (args.Length == 0 && !format.Contains("{0"))
                format += ": {0}";
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        public ulong ReadUInt64(string format, params object[] args)
        {
            var ret = Reader.ReadUInt64();
            if (args.Length == 0 && !format.Contains("{0"))
                format += ": {0}";
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        public string ReadCString(string format, params object[] args)
        {
            var ret = Reader.ReadCString();
            if (args.Length == 0 && !format.Contains("{0"))
                format += ": {0}";
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        public float ReadSingle(string format, params object[] args)
        {
            var ret = Reader.ReadSingle();
            if (args.Length == 0 && !format.Contains("{0"))
                format += ": {0}";
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        public ulong ReadPackedGuid(string format, params object[] args)
        {
            var ret = Reader.ReadPackedGuid();
            if (args.Length == 0 && !format.Contains("{0"))
                format += ": {0}";
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        public T Read<T>(string format, params object[] args) where T : struct
        {
            var ret = Reader.ReadStruct<T>();
            AppendFormatLine(format, MergeArguments(args, ret));
            return ret;
        }

        private object[] MergeArguments(object[] args, object arg)
        {
            var newArgs = new List<object>();
            newArgs.AddRange(args);
            newArgs.Add(arg);
            return newArgs.ToArray();
        }

        public void For(int count, Action func)
        {
            for (var i = 0; i < count; ++i)
                func();
        }

        public void For(int count, Action<int> func)
        {
            for (var i = 0; i < count; ++i)
                func(i);
        }

        private KeyValuePair<long, T> ReadEnum<T>(TypeCode code, byte bitsCount = (byte)0)
        {
            var type = typeof(T);
            object value = null;
            long rawVal = 0;

            if (code == TypeCode.Empty)
                code = Type.GetTypeCode(type.GetEnumUnderlyingType());

            switch (code)
            {
                case TypeCode.SByte:
                    rawVal = Reader.ReadSByte();
                    break;
                case TypeCode.Byte:
                    rawVal = Reader.ReadByte();
                    break;
                case TypeCode.Int16:
                    rawVal = Reader.ReadInt16();
                    break;
                case TypeCode.UInt16:
                    rawVal = Reader.ReadUInt16();
                    break;
                case TypeCode.Int32:
                    rawVal = Reader.ReadInt32();
                    break;
                case TypeCode.UInt32:
                    rawVal = Reader.ReadUInt32();
                    break;
                case TypeCode.Int64:
                    rawVal = Reader.ReadInt64();
                    break;
                case TypeCode.UInt64:
                    rawVal = (long)Reader.ReadUInt64();
                    break;
                case TypeCode.DBNull:
                    rawVal = ReadBits(bitsCount);
                    break;
            }
            value = System.Enum.ToObject(type, rawVal);

            return new KeyValuePair<long, T>(rawVal, (T)value);
        }

        public T ReadEnum<T>(string name, TypeCode code = TypeCode.Empty)
        {
            KeyValuePair<long, T> val = ReadEnum<T>(code);
            AppendFormatLine("{0}: {1} ({2})", name, val.Value, val.Key);
            return val.Value;
        }

        public DateTime ReadTime()
        {
            return Reader.ReadUInt32().AsUnixTime();
        }

        public DateTime ReadTime(string name)
        {
            var val = ReadTime();
            AppendFormatLine("{0}: {1}", name, val);
            return val;
        }

        public Coords3 ReadCoords3(string name = null)
        {
            Coords3 val;
            val.X = Reader.ReadSingle();
            val.Y = Reader.ReadSingle();
            val.Z = Reader.ReadSingle();
            if (name != null)
                AppendFormatLine("{0}: {1}", name, val);
            return val;
        }

        /* TODO: port GUID stuff
        public Guid ReadGuid()
        {
            return new Guid(Reader.ReadUInt64());
        }*/

        public bool ReadBit()
        {
            if (_bitPos < 0)
            {
                _currentByte = Reader.ReadByte();
                _bitPos = 7;
            }
            return ((_currentByte >> _bitPos--) & 1) != 0;
        }

        /// <summary>
        /// Reads an integer stored in bitsCount bits inside the bit stream.
        /// </summary>
        public uint ReadBits(byte bitsCount)
        {
            uint value = 0;
            for (int i = bitsCount - 1; i >= 0; i--)
            {
                if (ReadBit())
                    value |= (uint)1 << i;
            }
            return value;
        }

        public T ReadEnum<T>(string name, byte bitsCount)
        {
            KeyValuePair<long, T> val = ReadEnum<T>(TypeCode.DBNull, bitsCount);
            AppendFormatLine("{0}: {1} ({2})", name, val.Value, val.Key);
            return val.Value;
        }
    }
}
