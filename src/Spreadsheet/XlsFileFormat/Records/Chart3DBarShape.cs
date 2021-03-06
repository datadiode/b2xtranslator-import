/*
 * Copyright (c) 2009, DIaLOGIKa
 *
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without 
 * modification, are permitted provided that the following conditions are met:
 * 
 *     * Redistributions of source code must retain the above copyright 
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright 
 *       notice, this list of conditions and the following disclaimer in the 
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the names of copyright holders, nor the names of its contributors 
 *       may be used to endorse or promote products derived from this software 
 *       without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF 
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
 */

using System;
using System.Diagnostics;
using DIaLOGIKa.b2xtranslator.StructuredStorage.Reader;

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.Records
{
    [BiffRecordAttribute(RecordType.Chart3DBarShape)]
    public class Chart3DBarShape : BiffRecord
    {
        public const RecordType ID = RecordType.Chart3DBarShape;

        public enum RiserType : byte
        {
            Rectangle = 0x0,
            Ellipse = 0x1
        }

        public enum TaperType : byte
        {
            None = 0x0,
            TopEach = 0x1,
            TopMax = 0x2
        }

        /// <summary>
        /// A Boolean that specifies the shape of the base of the data points in a bar or column chart group. <br/>
        /// MUST be a value from the following table:<br/>
        /// false = The base of the data point is a rectangle.<br/>
        /// true = The base of the data point is an ellipse.
        /// </summary>
        public RiserType riser;

        /// <summary>
        /// An unsigned integer that specifies how the data points in a bar or column chart 
        /// group taper from base to tip. <br/>
        /// MUST be a value from the following table:<br/>
        /// 0 = The data points of the bar or column chart group do not taper. <br/>
        /// 1 = The data points of the bar or column chart group taper to a point at the maximum value of each data point.<br/>
        /// 2 = The data points of the bar or column chart group taper towards a projected point
        /// </summary>
        public TaperType taper;

        public Chart3DBarShape(IStreamReader reader, RecordType id, UInt16 length)
            : base(reader, id, length)
        {
            // assert that the correct record type is instantiated
            Debug.Assert(this.Id == ID);
            
            // initialize class members from stream
            this.riser = (RiserType)reader.ReadByte();
            this.taper = (TaperType)reader.ReadByte();

            // assert that the correct number of bytes has been read from the stream
            Debug.Assert(this.Offset + this.Length == this.Reader.BaseStream.Position);
        }
    }
}
