using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;

using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace OpenXmlUtilities
{
    public class ClsOpenXmlUtilities
    {
        public static void AddStyle(MainDocumentPart mainPart, string styleId, string styleName, string colore, string font, int fontSize, bool isBold, bool isItalic, bool isUnderlined)
        {
            //Set the properties
            RunProperties rPr = new RunProperties();
            Color color = new Color() { Val = colore };
            RunFonts rFont = new RunFonts();
            rFont.Ascii = font/*"Arial"*/; // the font is Arial
            rPr.Append(color);
            rPr.Append(rFont);
            rPr.Append(new FontSize() { Val = (fontSize * 2).ToString() });
            if (isBold) rPr.Append(new Bold()); // it is Bold
            if (isItalic) rPr.Append(new Italic());
            if (isUnderlined) rPr.Append(new Underline() { Val = UnderlineValues.Single });

            Style style = new Style();
            style.StyleId = styleId; //this is the ID of the style
            if (styleName == null || styleName.Length == 0) styleName = styleId;
            style.Append(new Name() { Val = styleName }); //this is the name of the new style
            style.Append(rPr); //we are adding properties previously defined

            // we have to add style that we have created to the StylePart
            StyleDefinitionsPart stylePart;
            if (mainPart.StyleDefinitionsPart == null)
            {
                stylePart = mainPart.AddNewPart<StyleDefinitionsPart>();
                stylePart.Styles = new Styles();
            }
            else
            {
                stylePart = mainPart.StyleDefinitionsPart;
            }

            stylePart.Styles.Append(style);
            stylePart.Styles.Save(); // we save the style part
        }

        public static Paragraph CreateParagraphWithStyle(string styleId, JustificationValues justification)
        {
            Paragraph paragraph = new Paragraph();
            ParagraphProperties pp = new ParagraphProperties();
            paragraph.Append(pp);
            // we set the style
            pp.ParagraphStyleId = new ParagraphStyleId() { Val = styleId };
            // we set the alignement
            pp.Justification = new Justification() { Val = justification };

            return paragraph;
        }

        public static void AddTextToParagraph(Paragraph paragraph, string content)
        {
            Run r = new Run();
            Text t = new Text(content);
            r.Append(t);
            paragraph.Append(r);
        }

        /*************TABLE*************/
        public static Table createTable(int nRow, int nCol)
        {
            Table table = new Table();
            // set table properties
            table.AppendChild(getTableProperties(BorderValues.Thick, "333333", BorderValues.Thick, "333333", BorderValues.Dotted, "999999", BorderValues.Dotted, "999999", BorderValues.Wave, "0000FF", BorderValues.Wave, "FF0000"));

            for (int i = 0; i < nRow; i++)
            {
                TableRow tr = new TableRow();
                for (int j = 0; j < nCol; j++)
                {
                    TableCell tc = new TableCell();
                    Paragraph p = CreateParagraphWithStyle("MyTypeScript", JustificationValues.Left);
                    AddTextToParagraph(p, "ok");
                    tc.Append(p);
                    tr.Append(tc);
                }
                table.Append(tr);
            }
            return table;
        }
        public static TableProperties getTableProperties(BorderValues bvTop, string colorTop, BorderValues bvBott, string colorBott, BorderValues bvRight, string colorRight, BorderValues bvLeft, string colorLeft, BorderValues bvinsIdeHBorder, string colorInsideHBorder, BorderValues bvInsideVBorder, string colorInsideVBorder)
        {
            TableProperties tblProperties = new TableProperties();
            TableBorders tblBorders = new TableBorders();

            TopBorder topBorder = new TopBorder();
            topBorder.Val = new EnumValue<BorderValues>(bvTop);
            topBorder.Color = colorTop;
            tblBorders.AppendChild(topBorder);

            BottomBorder bottomBorder = new BottomBorder();
            bottomBorder.Val = new EnumValue<BorderValues>(bvBott);
            bottomBorder.Color = colorBott;
            tblBorders.AppendChild(bottomBorder);

            RightBorder rightBorder = new RightBorder();
            rightBorder.Val = new EnumValue<BorderValues>(bvRight);
            rightBorder.Color = colorRight;
            tblBorders.AppendChild(rightBorder);

            LeftBorder leftBorder = new LeftBorder();
            leftBorder.Val = new EnumValue<BorderValues>(bvLeft);
            leftBorder.Color = colorLeft;
            tblBorders.AppendChild(leftBorder);

            InsideHorizontalBorder insideHBorder = new InsideHorizontalBorder();
            insideHBorder.Val = new EnumValue<BorderValues>(bvinsIdeHBorder);
            insideHBorder.Color = colorInsideHBorder;
            tblBorders.AppendChild(insideHBorder);

            InsideVerticalBorder insideVBorder = new InsideVerticalBorder();
            insideVBorder.Val = new EnumValue<BorderValues>(bvInsideVBorder);
            insideVBorder.Color = colorInsideVBorder;
            tblBorders.AppendChild(insideVBorder);

            tblProperties.AppendChild(tblBorders);

            return tblProperties;
        }

        public static void InsertPicture(WordprocessingDocument wordprocessingDocument, string fileName)
        {
            MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;
            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                imagePart.FeedData(stream);
            }
            AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart));
        }
        //PARAMWTRI(nome, )
        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 990000L, Cy = 792000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }
    }
}
