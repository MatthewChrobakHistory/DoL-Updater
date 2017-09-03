namespace Indexer
{
    public class SoftwareFile
    {
        public int Revision;
        public string Value;
        public long Size;

        public SoftwareFile(string value, int revision, long size) {
            this.Value = value;
            this.Revision = revision;
            this.Size = size;
        }
    }
}
