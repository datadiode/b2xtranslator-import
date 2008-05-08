using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DIaLOGIKa.b2xtranslator.StructuredStorageReader;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.BiffRecords;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat
{


    /// <summary>
    /// Extracts the workbook stream !!
    /// </summary>
    public class WorkbookExtractor : Extractor
    {    
        public string buffer;
        public long oldOffset; 

        public List<BOUNDSHEET> boundsheets;

        public List<SheetData> sheets;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public WorkbookExtractor(VirtualStreamReader reader) 
           : base(reader) 
        {         
            this.boundsheets = new List<BOUNDSHEET>();
            this.sheets = new List<SheetData>();
            this.oldOffset = 0; 

            this.extractData(); 
        }

        /// <summary>
        /// Extracts the data from the stream 
        /// </summary>
        public override void extractData()
        {
            BiffHeader bh;
            StreamWriter sw = null;
            sw = new StreamWriter(Console.OpenStandardOutput());
            
            try
            {
                while (this.StreamReader.BaseStream.Position < this.StreamReader.BaseStream.Length)
                {
                    bh.id = (RecordNumber)this.StreamReader.ReadUInt16();

                    bh.length = this.StreamReader.ReadUInt16();

                    if (bh.id == RecordNumber.BOUNDSHEET)
                    {
                        
                        BOUNDSHEET bs = new BOUNDSHEET(this.StreamReader, bh.id, bh.length);
                        
                        this.oldOffset = this.StreamReader.BaseStream.Position;
                        this.StreamReader.BaseStream.Seek(bs.lbPlyPos, SeekOrigin.Begin);
                        
                        SheetExtractor se = new SheetExtractor(this.StreamReader);
                        this.StreamReader.BaseStream.Seek(oldOffset, SeekOrigin.Begin); 
                        
                        sw.Write(bs.ToString());
                    } else if (bh.id == RecordNumber.SST)
                    {
                        SST sst = new SST(this.StreamReader, bh.id, bh.length);
                        sw.Write(sst.ToString()); 
                    }

                    else if (bh.id == RecordNumber.EOF)
                    {
                        // this.StreamReader.BaseStream.Seek(0, SeekOrigin.End); 
                        sw.Write("EOF"); 
                    } 
                    else
                    {


                        byte[] buffer = new byte[bh.length];
                        buffer = this.StreamReader.ReadBytes(bh.length);
                        if (bh.length != buffer.Length)
                            sw.WriteLine("EOF");

                        sw.Write("BIFF {0}\t{1}\t", bh.id, bh.length);
                        //Dump(buffer);
                        int count = 0;
                        foreach (byte b in buffer)
                        {
                            sw.Write("{0:X02} ", b);
                            count++;
                            if (count % 16 == 0 && count < buffer.Length)
                                sw.Write("\n\t\t\t");
                        }
                        sw.Write("\n");
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            sw.Close();
        }

        /// <summary>
        /// A normal overload ToString Method 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string returnvalue = "Workbook";
            return returnvalue;
        }
    }
}
