/*
 * Copyright (c) 2008, DIaLOGIKa
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *        notice, this list of conditions and the following disclaimer.
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
using DIaLOGIKa.b2xtranslator.StructuredStorageReader;
using DIaLOGIKa.b2xtranslator.CommonTranslatorLib;

namespace DIaLOGIKa.b2xtranslator.DocFileFormat
{
    public class FormattedDiskPage : IVisitable
    {
        public enum FKPType
        {
            Character,
            Paragraph
        }

        /// <summary>
        /// Specifies the type of the FKP
        /// </summary>
        public FKPType Type;

        /// <summary>
        /// The WordDocument stream
        /// </summary>
        public VirtualStream WordStream;

        /// <summary>
        /// The offset of the page in the WordDocument stream
        /// </summary>
        public Int32 Offset;

        /// <summary>
        /// Count of runs for that FKP
        /// </summary>
        public byte crun;

        /// <summary>
        /// Each value is the limit of a paragraph or run of exception text
        /// </summary>
        public Int32[] rgfc;

        /// <summary>
        /// Returns the hex dump of the FKP
        /// </summary>
        /// <returns>The hex dump of the FKP as string</returns>
        public override string ToString()
        {
            int colCount = 16;

            byte[] bytes = new byte[512];
            this.WordStream.Read(bytes, 512, this.Offset);

            return Utils.GetHashDump(bytes);
        }

        #region IVisitable Members

        public void Convert<T>(T mapping)
        {
            ((IMapping<FormattedDiskPage>)mapping).Apply(this);
        }

        #endregion
    }
}
