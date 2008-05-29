/*
 * Copyright (c) 2008, DIaLOGIKa
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of DIaLOGIKa nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY DIaLOGIKa ''AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL DIaLOGIKa BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Text;
using DIaLOGIKa.b2xtranslator.CommonTranslatorLib;
using System.Xml;
using DIaLOGIKa.b2xtranslator.OpenXmlLib;
using DIaLOGIKa.b2xtranslator.SpreadsheetMLMapping;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat;
using System.Diagnostics;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.DataContainer;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.BiffRecords;

namespace DIaLOGIKa.b2xtranslator.SpreadsheetMLMapping
{
    public class WorksheetMapping : AbstractOpenXmlMapping,
          IMapping<BoundSheetData>
    {
        ExcelContext xlsContext;

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="xlsContext">The excel context object</param>
        public WorksheetMapping(ExcelContext xlsContext)
            : base(XmlWriter.Create(xlsContext.SpreadDoc.WorkbookPart.AddWorksheetPart().GetStream(), xlsContext.WriterSettings))
        {
            this.xlsContext = xlsContext;
        }

        /// <summary>
        /// The overload apply method 
        /// Creates the Worksheet xml document 
        /// </summary>
        /// <param name="bsd">BoundSheetData</param>
        public void Apply(BoundSheetData bsd)
        {
            _writer.WriteStartDocument();
            _writer.WriteStartElement("worksheet", OpenXmlNamespaces.WorkBookML);
            _writer.WriteStartElement("sheetData");
           //  bsd.rowDataTable.Values



            foreach (RowData row in bsd.rowDataTable.Values)
            {

                // write row start tag
                // Row 
                _writer.WriteStartElement("row");
                _writer.WriteAttributeString("r", (row.Row +1).ToString());
                foreach (AbstractCellData cell in row.Cells)
                {
                    // Col 
                    _writer.WriteStartElement("c");
                    _writer.WriteAttributeString("r", this.intToABCString((int)cell.Col, (cell.Row + 1).ToString()));
                    if (cell is StringCell)
                    {
                        _writer.WriteAttributeString("t", "s");
                    } 
                    // Data !!! 
                    _writer.WriteElementString("v", cell.getValue());

                    _writer.WriteEndElement();  // close cell (c)  
                }


                _writer.WriteEndElement();  // close row 
            }
            /*  
            /// Adding strings to the new worksheet 
            foreach (LABELSST cell in bsd.LABELSSTList)
            {
                // Row 
                _writer.WriteStartElement("row");
                _writer.WriteAttributeString("r", cell.rw.ToString());
                // Col 
                _writer.WriteStartElement("c");
                _writer.WriteAttributeString("r", this.intToABCString((int)cell.col, cell.rw.ToString()));
                _writer.WriteAttributeString("t", "s");
                // Data !!! 
                _writer.WriteElementString("v", cell.isst.ToString());

                _writer.WriteEndElement();  // close cell (c)  
                _writer.WriteEndElement();  // close row 

            } 
            
            /// adding the numbers to the new worksheet 
            foreach (MULRK cell in bsd.MULRKList)
            {
                // Row 
                _writer.WriteStartElement("row");
                _writer.WriteAttributeString("r", cell.rw.ToString());
                int count = 0; 
                foreach (double number in cell.rknumber)
                {
                    
                    // Col 
                    _writer.WriteStartElement("c");
                    _writer.WriteAttributeString("r", this.intToABCString((int)cell.colFirst + count, cell.rw.ToString()));
                    
                    // _writer.WriteAttributeString("t");
                    // Data !!! 
                    _writer.WriteElementString("v", number.ToString());

                    _writer.WriteEndElement();  // close cell (c)       
                    count++; 
                }

                _writer.WriteEndElement();  // close row 

            } 
            */ 

            

            // close tags 
            _writer.WriteEndElement();      // close sheetData 
            _writer.WriteEndElement();      // close worksheet
            _writer.WriteEndDocument();
            bsd.worksheetId = this.xlsContext.SpreadDoc.WorkbookPart.GetWorksheetPart().RelId;
            bsd.worksheetRef = this.xlsContext.SpreadDoc.WorkbookPart.GetWorksheetPart().RelIdToString;
            
            // close writer 
            _writer.Flush();
        }

        /// <summary>
        /// converts the integer column value to a string like AB 
        /// </summary>
        /// <returns>String</returns>
        public String intToABCString(int colnumber, string rownumber)
        {
            String value = "";
            int remain = 0;
            while (colnumber != 0)
            {
                remain = colnumber % 26;
                colnumber = colnumber / 26;
                remain += 65;
                value += (char)remain;
            }
            return value + rownumber;
        }

    }

}
