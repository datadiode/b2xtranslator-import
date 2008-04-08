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
    public class ListTable : List<ListData>, IVisitable
    {
        private const int LSTF_LENGTH = 28;
        private const int LVLF_LENGTH = 28;

        public ListTable(FileInformationBlock fib, VirtualStream tableStream)
        {
            byte[] bytes = new byte[fib.lcbPlcfLst];
            tableStream.Read(bytes, 0, bytes.Length, fib.fcPlcfLst);

            //the ListTable is a plex.
            //it starts with a count, followed by the array of LST structs,
            //followed by the array of LVL structs

            //read count
            byte[] countBytes = new byte[2];
            tableStream.Read(countBytes, 0, 2, fib.fcPlcfLst);
            Int16 count = System.BitConverter.ToInt16(countBytes, 0);

            //read the LST structs
            int lvlPos = fib.fcPlcfLst + (int)fib.lcbPlcfLst;
            for (int i = 0; i < count; i++)
            {
                //read and parse
                int offset = fib.fcPlcfLst + 2 + (i * LSTF_LENGTH);
                byte[] lstf = new byte[LSTF_LENGTH];
                tableStream.Read(lstf, 0, LSTF_LENGTH, offset);
                ListData lst = new ListData(lstf);

                //read the LVL structs that belong to this LST
                for (int j = 0; j < lst.rglvl.Length; j++)
                {
                    ListLevel lvl = new ListLevel(tableStream, lvlPos);
                    lst.rglvl[j] = lvl;

                    lvlPos += (LVLF_LENGTH + lvl.cbGrpprlPapx + lvl.cbGrpprlChpx + 2 + lvl.xst.Length*2);
                }

                this.Add(lst);
            }
        }

        #region IVisitable Members

        public void Convert<T>(T mapping)
        {
            ((IMapping<ListTable>)mapping).Apply(this);
        }

        #endregion
    }
}