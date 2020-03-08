using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlUtilities;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;

namespace OpenXmlPlayground
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnSimpleWordTest_Click(object sender, EventArgs e)
        {
            try
            {
                string filepath = "test.docx";
                string msg = "Hello World!";
                using (WordprocessingDocument doc = WordprocessingDocument.Create(filepath,
                                    DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
                {
                    // Add a main document part. 
                    MainDocumentPart mainPart = doc.AddMainDocumentPart();

                    // Create the document structure and add some text.
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    // Define the styles
                    //addHeading1Style(mainPart, "FF0000", "Arial", "28");
                    ClsOpenXmlUtilities.AddStyle(mainPart, "MyHeading1", "Titolone", "0000FF", "Verdana", 28, false, true, true);
                    ClsOpenXmlUtilities.AddStyle(mainPart, "MyTypeScript", "Macchina da scrivere", "333333", "Consolas", 12, true, false, false);

                    // Add heading
                    Paragraph headingPar = ClsOpenXmlUtilities.CreateParagraphWithStyle("MyHeading1", JustificationValues.Center);
                    ClsOpenXmlUtilities.AddTextToParagraph(headingPar, "Titolo con stile applicato");
                    body.AppendChild(headingPar);

                    //Add MyTypeScript
                    Paragraph typeScriptPar = ClsOpenXmlUtilities.CreateParagraphWithStyle("MyTypeScript", JustificationValues.Center);
                    ClsOpenXmlUtilities.AddTextToParagraph(typeScriptPar, "Testo lungo lungo lungo lungo lungo lungo lungo lungo lungo lungo lungo lungo lungo lungo lungo lungo lungo lungo");
                    body.AppendChild(typeScriptPar);

                    // Add simple text
                    Paragraph para = body.AppendChild(new Paragraph());
                    Run run = para.AppendChild(new Run());
                    // String msg contains the text, "Hello, Word!"
                    run.AppendChild(new Text(msg));

                    // Add heading
                    //headingPar = ClsOpenXmlUtilities.createHeading("Testo con stili");
                    //body.AppendChild(headingPar);

                    // Append a paragraph with styles
                    Paragraph newPar = createParagraphWithStyles();
                    body.AppendChild(newPar);

                    // Append a table
                    Table myTable = ClsOpenXmlUtilities.createTable(3, 3, "ok");
                    body.Append(myTable);

                    // Append bullet list
                    ClsOpenXmlUtilities.createBulletNumberingPart(mainPart, "-");
                    List<Paragraph> bulletList = ClsOpenXmlUtilities.createList(3, "bullet", "bullet", "0", "100", "200");/*createBulletList(4, "yes");*/
                    foreach (Paragraph paragraph in bulletList)
                    {
                        body.Append(paragraph);
                    }

                    // Append numbered list
                    List<Paragraph> numberedList = ClsOpenXmlUtilities.createList(3, "numbered", "numbered", "0", "100", "240");/*createNumberedList();*/
                    foreach (Paragraph paragraph in numberedList)
                    {
                        body.Append(paragraph);
                    }

                    // Append image
                    ClsOpenXmlUtilities.InsertPicture(doc, "panorama.jpg");
                }
                MessageBox.Show("Il documento è pronto!");
                Process.Start(filepath);
            }
            catch (Exception)
            {
                MessageBox.Show("Problemi col documento. Se è aperto da un altro programma, chiudilo e riprova...");
            }
        }

        private Paragraph createParagraphWithStyles()
        {
            Paragraph p = new Paragraph();
            // Set the paragraph properties
            ParagraphProperties pp = new ParagraphProperties(new ParagraphStyleId() { Val = "Titolo1" });
            pp.Justification = new Justification() { Val = JustificationValues.Center };
            // Add paragraph properties to your paragraph
            p.Append(pp);

            // Run 1
            Run r1 = new Run();
            Text t1 = new Text("Pellentesque ") { Space = SpaceProcessingModeValues.Preserve };
            // The Space attribute preserve white space before and after your text
            r1.Append(t1);
            p.Append(r1);

            // Run 2 - Bold
            Run r2 = new Run();
            RunProperties rp2 = new RunProperties();
            rp2.Bold = new Bold();
            // Always add properties first
            r2.Append(rp2);
            Text t2 = new Text("grassetto ") { Space = SpaceProcessingModeValues.Preserve };
            r2.Append(t2);
            p.Append(r2);

            // Run 3
            Run r3 = new Run();
            Text t3 = new Text("rhoncus ") { Space = SpaceProcessingModeValues.Preserve };
            r3.Append(t3);
            p.Append(r3);

            // Run 4 – Italic
            Run r4 = new Run();
            RunProperties rp4 = new RunProperties();
            rp4.Italic = new Italic();
            // Always add properties first
            r4.Append(rp4);
            Text t4 = new Text("italico") { Space = SpaceProcessingModeValues.Preserve };
            r4.Append(t4);
            p.Append(r4);

            // Run 5
            Run r5 = new Run();
            Text t5 = new Text(", sit ") { Space = SpaceProcessingModeValues.Preserve };
            r5.Append(t5);
            p.Append(r5);

            // Run 6 – Italic , bold and underlined
            Run r6 = new Run();
            RunProperties rp6 = new RunProperties();
            rp6.Italic = new Italic();
            rp6.Bold = new Bold();
            rp6.Underline = new Underline() { Val = UnderlineValues.WavyDouble };
            // Always add properties first
            r6.Append(rp6);
            Text t6 = new Text("grasseto, italico, sottolineato ") { Space = SpaceProcessingModeValues.Preserve };
            r6.Append(t6);
            p.Append(r6);

            // Run 7
            Run r7 = new Run();
            Text t7 = new Text("faucibus arcu ") { Space = SpaceProcessingModeValues.Preserve };
            r7.Append(t7);
            p.Append(r7);

            // Run 8 – Red color
            Run r8 = new Run();
            RunProperties rp8 = new RunProperties();
            rp8.Color = new Color() { Val = "FF0000" };
            // Always add properties first
            r8.Append(rp8);
            Text t8 = new Text("rosso ") { Space = SpaceProcessingModeValues.Preserve };
            r8.Append(t8);
            p.Append(r8);

            // Run 9
            Run r9 = new Run();
            Text t9 = new Text("pharetra. Maecenas quis erat quis eros iaculis placerat ut at mauris.") { Space = SpaceProcessingModeValues.Preserve };
            r9.Append(t9);
            p.Append(r9);

            // return the new paragraph
            return p;
        }

        

        //private List<Paragraph> createBulletList(int nRighe, string txt)
        //{
        //    List<Paragraph> retVal = new List<Paragraph>();
        //    SpacingBetweenLines sbl = new SpacingBetweenLines() { After = "0" };
        //    Indentation indent = new Indentation() { Left = "100", Hanging = "200" };
        //    NumberingProperties np = new NumberingProperties(
        //        new NumberingLevelReference() { Val = 0 },
        //        new NumberingId() { Val = 1 }
        //    );
        //    ParagraphProperties ppUnordered = new ParagraphProperties(np, sbl, indent);
        //    ppUnordered.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };
        //    for (int i = 0; i < nRighe; i++)
        //    {
        //        Paragraph p = new Paragraph();
        //        p.ParagraphProperties = new ParagraphProperties(ppUnordered.OuterXml);
        //        p.Append(new Run(new Text(txt)));
        //        retVal.Add(p);
        //    }

        //    return retVal;
        //}

        //private List<Paragraph> createNumberedList()
        //{
        //    List<Paragraph> retVal = new List<Paragraph>();
        //    SpacingBetweenLines sbl = new SpacingBetweenLines() { After = "0" };
        //    Indentation indent = new Indentation() { Left = "100", Hanging = "240" };
        //    NumberingProperties np = new NumberingProperties(
        //        new NumberingLevelReference() { Val = 1 },
        //        new NumberingId() { Val = 2 }
        //    );
        //    ParagraphProperties ppOrdered = new ParagraphProperties(np, sbl, indent);
        //    ppOrdered.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };

        //    // Pargraph
        //    Paragraph p1 = new Paragraph();
        //    p1.ParagraphProperties = new ParagraphProperties(ppOrdered.OuterXml);
        //    p1.Append(new Run(new Text("First elementttt")));
        //    retVal.Add(p1);
        //    Paragraph p2 = new Paragraph();
        //    p2.ParagraphProperties = new ParagraphProperties(ppOrdered.OuterXml);
        //    p2.Append(new Run(new Text("Second Element")));
        //    retVal.Add(p2);
        //    Paragraph p3 = new Paragraph();
        //    p3.ParagraphProperties = new ParagraphProperties(ppOrdered.OuterXml);
        //    p3.Append(new Run(new Text("Third Element")));
        //    retVal.Add(p3);

        //    return retVal;
        //}
    }
}
