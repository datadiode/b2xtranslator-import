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
using System.IO;
using DIaLOGIKa.b2xtranslator.StructuredStorageReader;
using DIaLOGIKa.b2xtranslator.CommonTranslatorLib;
using DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat.DataContainer; 

namespace DIaLOGIKa.b2xtranslator.Spreadsheet.XlsFileFormat
{
    public class XlsDocument :  IVisitable
    {
        /// <summary>
        /// Some constant strings 
        /// </summary>
        private const string WORKBOOK = "Workbook";

        /// <summary>
        /// The workbook streamreader 
        /// </summary>
        private VirtualStreamReader workBookStreamReader; 

        /// <summary>
        /// The Workbookextractor / container 
        /// </summary>
        private WorkbookExtractor workBookExtr;

        /// <summary>
        /// This attribute stores the hole Workbookdata 
        /// </summary>
        public WorkBookData workBookData; 


        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="file"></param>
        public XlsDocument(IStructuredStorageFile file)
        {
            this.workBookData = new WorkBookData(); 
            this.workBookStreamReader = new VirtualStreamReader(file.GetStream(WORKBOOK));

            this.workBookExtr = new WorkbookExtractor(this.workBookStreamReader, this.workBookData); 
        }


        #region IVisitable Members

        public void Convert<T>(T mapping)
        {
            ((IMapping<XlsDocument>)mapping).Apply(this);
        }

        #endregion
    }
}