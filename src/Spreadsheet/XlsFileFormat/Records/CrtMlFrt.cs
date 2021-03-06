using System;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Structures;
using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Records
{
    [BiffRecordAttribute(RecordType.CrtMlFrt)]
    public class CrtMlFrt : BiffRecord
    {
        public FrtHeader frtHeader;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, 
        /// of the XmlTkChain structure starting in the xmltkChain field, 
        /// including the data contained in the optional CrtMlFrtContinue records.<br/> 
        /// MUST be less than or equal to 0x7FFFFFEB.
        /// </summary>
        public UInt32 cb;

        public XmlTkChain xmltkChain;

        public CrtMlFrt(IStreamReader reader, RecordType id, UInt16 length)
            : base(reader, id, length)
        {
            long pos = reader.BaseStream.Position;

            this.frtHeader = new FrtHeader(reader);
            this.cb = reader.ReadUInt32();
            this.xmltkChain = new XmlTkChain(reader);
            reader.ReadBytes(4); // unused

            reader.BaseStream.Position = pos + length;
        }
    }
}
