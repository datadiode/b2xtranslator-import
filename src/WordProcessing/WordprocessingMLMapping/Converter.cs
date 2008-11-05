using System;
using System.Collections.Generic;
using System.Text;
using DIaLOGIKa.b2xtranslator.DocFileFormat;
using System.Xml;
using DIaLOGIKa.b2xtranslator.OpenXmlLib.WordprocessingML;
using DIaLOGIKa.b2xtranslator.OpenXmlLib;
using System.IO;

namespace DIaLOGIKa.b2xtranslator.WordprocessingMLMapping
{
    public class Converter
    {
        public static WordprocessingDocumentType DetectOutputType(WordDocument doc)
        {
            WordprocessingDocumentType returnType = WordprocessingDocumentType.Document;

            //detect the document type
            if (doc.FIB.fDot)
            {
                //template
                if (doc.CommandTable.MacroDatas != null && doc.CommandTable.MacroDatas.Count > 0)
                {
                    //macro enabled template
                    returnType = WordprocessingDocumentType.MacroEnabledTemplate;
                }
                else
                {
                    //without macros
                    returnType = WordprocessingDocumentType.Template;
                }
            }
            else
            {
                //no template
                if (doc.CommandTable.MacroDatas != null && doc.CommandTable.MacroDatas.Count > 0)
                {
                    //macro enabled document
                    returnType = WordprocessingDocumentType.MacroEnabledDocument;
                }
                else
                {
                    returnType = WordprocessingDocumentType.Document;
                }
            }

            return returnType;
        }


        public static string GetConformFilename(string choosenFilename, WordprocessingDocumentType outType)
        {
            string outExt = ".docx";
            switch (outType)
            {
                case WordprocessingDocumentType.Document:
                    outExt = ".docx";
                    break;
                case WordprocessingDocumentType.MacroEnabledDocument:
                    outExt = ".docm";
                    break;
                case WordprocessingDocumentType.MacroEnabledTemplate:
                    outExt = ".dotm";
                    break;
                case WordprocessingDocumentType.Template:
                    outExt = ".dotx";
                    break;
                default:
                    outExt = ".docx";
                    break;
            }

            string inExt = Path.GetExtension(choosenFilename);
            if (inExt != null)
            {
                return choosenFilename.Replace(inExt, outExt);
            }
            else
            {
                return choosenFilename + outExt;
            }
        }


        public static void Convert(WordDocument doc, WordprocessingDocument docx)
        {
            ConversionContext context = new ConversionContext(doc);
            using (docx)
            {
                //Setup the writer
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = false;
                xws.CloseOutput = true;
                xws.Encoding = Encoding.UTF8;
                xws.ConformanceLevel = ConformanceLevel.Document;

                //Setup the context
                context.WriterSettings = xws;
                context.Docx = docx;

                //convert the macros
                if (docx.DocumentType == WordprocessingDocumentType.MacroEnabledDocument ||
                    docx.DocumentType == WordprocessingDocumentType.MacroEnabledTemplate)
                {
                    doc.Convert(new MacroBinaryMapping(context));
                    doc.Convert(new MacroDataMapping(context));
                }

                //convert the command table
                doc.CommandTable.Convert(new CommandTableMapping(context));

                //Write styles.xml
                doc.Styles.Convert(new StyleSheetMapping(context, doc, docx.MainDocumentPart.StyleDefinitionsPart));

                //Write numbering.xml
                doc.ListTable.Convert(new NumberingMapping(context, doc));

                //Write fontTable.xml
                doc.FontTable.Convert(new FontTableMapping(context, docx.MainDocumentPart.FontTablePart));

                //write document.xml and the header and footers
                doc.Convert(new MainDocumentMapping(context, context.Docx.MainDocumentPart));

                //write the footnotes
                doc.Convert(new FootnotesMapping(context));

                //write the comments
                doc.Convert(new CommentsMapping(context));

                //write settings.xml at last because of the rsid list
                doc.DocumentProperties.Convert(new SettingsMapping(context, docx.MainDocumentPart.SettingsPart));

                //convert the glossary subdocument
                if (doc.Glossary != null)
                {
                    doc.Glossary.Convert(new GlossaryMapping(context, context.Docx.MainDocumentPart.GlossaryPart));
                    doc.Glossary.FontTable.Convert(new FontTableMapping(context, docx.MainDocumentPart.GlossaryPart.FontTablePart));
                    //doc.Glossary.Styles.Convert(new StyleSheetMapping(context, doc.Glossary, docx.MainDocumentPart.GlossaryPart.StyleDefinitionsPart));

                    //write settings.xml at last because of the rsid list
                    doc.Glossary.DocumentProperties.Convert(new SettingsMapping(context, docx.MainDocumentPart.GlossaryPart.SettingsPart));
                }
            }
        }
    }
}