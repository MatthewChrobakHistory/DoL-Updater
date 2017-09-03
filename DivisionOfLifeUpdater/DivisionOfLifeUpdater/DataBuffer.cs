using System.IO;
namespace DivisionOfLifeUpdater
{
    public class DataBuffer
    {
        private MemoryStream _buffer;
        private BinaryReader _reader;

        public DataBuffer(byte[] array) {
            // Initialize the memory stream and reader to enable 
            // immediate reading capabilities.
            _buffer = new MemoryStream(array);
            _reader = new BinaryReader(_buffer);
        }

        // Manual disposing of the objects.
        public void Dispose() {
            if (_reader != null) {
                _reader.Dispose();
            }
            if (_buffer != null) {
                _buffer.Dispose();
            }
        }

        // Methods that read from the buffer.
        public int ReadInt() {
            if (_reader == null) {
                return 0;
            }
            int value = _reader.ReadInt32();
            if (_buffer.Position == _buffer.Length) {
                Dispose();
            }
            return value;
        }
        public string ReadString() {
            if (_reader == null) {
                return "";
            }
            string value = _reader.ReadString();
            if (_buffer.Position == _buffer.Length) {
                Dispose();
            }
            return value;
        }

        public long ReadLong() {
            if (_reader == null) {
                return 0;
            }
            long value = _reader.ReadInt64();
            if (_buffer.Position == _buffer.Length) {
                Dispose();
            }
            return value;
        }
    }
}
