using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp13
{
    class Program
    {

        static void Main(string[] args)
        {         

            var GivenItems = new List<ItemDetail>();
            var isFiveItems = false;
            AddAllItems(GivenItems);
            if (GivenItems.Count >= 5)
                isFiveItems = true;

            Document doc = new Document(PageSize.A4, 0, 0, 0, 0);
            Document.Compress = true;
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("../../Sample.pdf", FileMode.Create, FileAccess.Write));

            try
            {
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                float pageWidth = doc.PageSize.Width;
                float pageHeight = doc.PageSize.Height;
                doc.Open();
                string fontpath = "../../fonts";
                FontFactory.RegisterDirectory(fontpath);

                FirstPage(doc, writer, pageWidth, pageHeight);

                PdfStartEvent events = new PdfStartEvent();
                writer.PageEvent = events;

                MenuPage(GivenItems, isFiveItems, doc, writer, pageHeight);

                PdfEndEvent endEvent = new PdfEndEvent();
                writer.PageEvent = endEvent;

                CompetencySummaryPage(10, doc, writer, pageWidth, pageHeight, "Manpreet");
            }
            catch (Exception ex)
            {


            }
            finally
            {
                writer.Flush();
                doc.Close();

            }

            Console.ReadKey();
        }
        private static void CompetencySummaryPage(int userId, Document doc, PdfWriter writer, float pageWidth, float pageHeight, string username)
        {
            try
            {
                var Ratings = new List<RatingsDetail> {
                    new RatingsDetail{
                    RatingId=1,
                    Name ="Neatness",
                    BenchmarkRarting="50",
                    DesiredRating="5",
                    MaxRating="4",
                    Meaninig="What are the qualities of an effective digital logo? ",
                    TotalScore ="2.5" },

                    new RatingsDetail{
                    RatingId=2,
                    Name ="Time",
                    BenchmarkRarting="40",
                    DesiredRating="4",
                    MaxRating="4",
                    Meaninig="One of the biggest trends we have seen in logo preferences is the trend",
                    TotalScore ="3.5" },

                    new RatingsDetail{
                    RatingId=3,
                    Name ="Quality",
                    BenchmarkRarting="34",
                    DesiredRating="4",
                    MaxRating="7",
                    Meaninig="Quality of the mealality of the mealality of the mealality of the mealality of the mealality of the meal",
                    TotalScore ="2" },

                    new RatingsDetail{
                    RatingId=4,
                    Name ="Taste",
                    BenchmarkRarting="65",
                    DesiredRating="5",
                    MaxRating="4",
                    Meaninig="Logo preferences have changed with the times, too. Digital marketing requires",
                    TotalScore ="4" },

                    new RatingsDetail{
                    RatingId=5,
                    Name ="Quantity",
                    BenchmarkRarting="25",
                    DesiredRating="5",
                    MaxRating="4",
                    Meaninig="As time passed and technological advances became commonplace, marketing ",
                    TotalScore ="2.5" },

                    new RatingsDetail{
                    RatingId=6,
                    Name ="Intelliegne",
                    BenchmarkRarting="15",
                    DesiredRating="7",
                    MaxRating="6",
                    Meaninig="As logo design has evolved, so too has marketing.",
                    TotalScore ="6" },

                    new RatingsDetail{
                    RatingId=7,
                    Name ="Iteration",
                    BenchmarkRarting="29",
                    DesiredRating="6",
                    MaxRating="4",
                    Meaninig="Gradually, logo designs changed. As graphic designers embraced new technology,",
                    TotalScore ="3" },

                    new RatingsDetail{
                    RatingId=8,
                    Name ="Property",
                    BenchmarkRarting="25",
                    DesiredRating="9",
                    MaxRating="4",
                    Meaninig="As mentioned earlier, the first company logos were designed and.",
                    TotalScore ="2.5" },

                    new RatingsDetail{
                    RatingId=9,
                    Name ="Serving",
                    BenchmarkRarting="25",
                    DesiredRating="7",
                    MaxRating="8",
                    Meaninig="With logos more widely seen than ever before – and the accessibility of digital.",
                    TotalScore ="5.5" },

                    new RatingsDetail{
                    RatingId=10,
                    Name ="Outlook",
                    BenchmarkRarting="25",
                    DesiredRating="8",
                    MaxRating="8",
                    Meaninig="In today’s world of digital marketing and design, it has become more difficult.",
                    TotalScore ="5" },

                    new RatingsDetail{
                    RatingId=11,
                    Name ="Presentation",
                    BenchmarkRarting="25",
                    DesiredRating="6",
                    MaxRating="6",
                    Meaninig="What was the quantity servwa which has to be dlievred at the same time.",
                    TotalScore ="3.5" },

                    new RatingsDetail{
                    RatingId=12,
                    Name ="Feedback",
                    BenchmarkRarting="25",
                    DesiredRating="9",
                    MaxRating="8",
                    Meaninig="Prior to the digital revolution, it wasn’t much of a challenge for companies.",
                    TotalScore ="6.5" },

                };

                List<RatingsDetail> allRatingList = new List<RatingsDetail>();

                foreach (var rating in Ratings)
                {
                    var ratimgId = rating.RatingId;
                    var name = rating.Name;
                    double score = Convert.ToDouble(rating.TotalScore);
                    double maxMarks = Convert.ToDouble(rating.MaxRating);
                    double desiredMarks = Convert.ToDouble(rating.DesiredRating);
                    double perc = (score / maxMarks * 100);
                    perc = Math.Round(perc, MidpointRounding.AwayFromZero);

                    allRatingList.Add(new RatingsDetail
                    {
                        RatingId = ratimgId,
                        Name = name,
                        TotalScore = score.ToString(),
                        PercentScore = perc.ToString(),
                        MaxRating = maxMarks.ToString(),
                        DesiredRating = desiredMarks.ToString(),
                        Weightage = rating.Weightage,
                        BenchmarkRarting = rating.BenchmarkRarting,
                        Meaninig = rating.Meaninig
                    });
                }

                if (allRatingList.Count == 0)
                    return;

                List<RatingsDetail> remainingRatingsList = allRatingList.OrderBy(x => x.Name).ToList();
                Image headerImage = null;
                float compTableEndYPoint = 0f;

                while (remainingRatingsList.Count > 0)
                {
                    var compDetailList = remainingRatingsList;

                    doc.NewPage();
                    headerImage = Image.GetInstance(("../../Images/ratings.png"));
                    headerImage.ScaleToFit(pageWidth, headerImage.Height);
                    headerImage.SetAbsolutePosition(0, pageHeight - 188f);
                    doc.Add(headerImage);

                    PdfPTable table = new PdfPTable(7);
                    PdfPCell cell;
                    table.TotalWidth = 530;
                    float[] widths = new float[] { 3.2f, 1f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f };
                    table.SetWidths(widths);

                    var headerFont = FontFactory.GetFont("arial_2", 13, Font.BOLDITALIC, new BaseColor(136, 100, 21));
                    var compNameFont = FontFactory.GetFont("arial_2", 15, Font.BOLD, new BaseColor(136, 0, 21));
                    var definitionFont = FontFactory.GetFont("arial_2", 12, Font.NORMAL, new BaseColor(136, 100, 21));
                    var percentFont = FontFactory.GetFont("arial_2", 15, Font.BOLD, new BaseColor(136, 0, 21));
                    var darkGreenColor = new BaseColor(55, 98, 83);
                    var heightArray = new List<float>();

                    CalculateOverallScoreAtTheTop(writer, pageWidth, pageHeight, allRatingList);

                    cell = CreateHeaderRowForCompetencyTable(table, headerFont, darkGreenColor);
                    var compfilled = CreateCompetencyMarksRow(remainingRatingsList, table, cell, compNameFont, definitionFont,
                        percentFont, heightArray, pageHeight, "SummaryReport");
                    AddTargetScoreRowAtTheEnd(table, username);
                    compTableEndYPoint = table.WriteSelectedRows(0, -1, 33f, pageHeight - 220f, writer.DirectContent);
                    DrawGraphsWithScoreAndTargetOnCanvas(writer, compfilled, heightArray, "SummaryReport");
                    remainingRatingsList = compDetailList.Except(compfilled).ToList();
                }

                // OverusedCompetencySection(doc, writer, allRatingList, headerImage, compTableEndYPoint, "SummaryReport");
            }
            catch (Exception ex)
            {
                // Add to log file
            }
        }

        private static void DrawGraphsWithScoreAndTargetOnCanvas(PdfWriter writer, List<RatingsDetail> compDetailList, List<float> heightArray,
           string reportType)
        {
            try
            {
                PdfContentByte canvas = writer.DirectContent;
                Image scoreImg = Image.GetInstance(("../../Images/Score.jpg"));
                scoreImg.ScaleToFit(17, 17);
                Image targetImg = Image.GetInstance(("../../Images/Target.png"));
                targetImg.ScaleToFit(20, 20);

                var xStarting = 230;
                var yStarting = 10;
                var xEnding = 555;
                var height = 10;
                var radius = 3.5f;
                var shadingRectWidth = 325;
                PdfShading shading = PdfShading.SimpleAxial(writer, xStarting - 50, yStarting, xEnding - 20, height, new BaseColor(255, 255, 128), new BaseColor(249, 122, 0)); //red & green
                PdfShadingPattern shadingPattern = new PdfShadingPattern(shading);

                var score = compDetailList.Select(x => x.PercentScore).ToList();
                var target = compDetailList.Select(x => x.BenchmarkRarting).ToList();

                float y = 0f;

                if (reportType == "SummaryReport")
                    y = 617 - heightArray[0];

                for (int i = 0; i < score.Count; i++)
                {
                    y = y - heightArray[i + 1] / 2;

                    var scFloat = float.Parse(score[i]);
                    if (scFloat == 0)
                        scFloat = 1;
                    else if (scFloat > 100)
                        scFloat = 100;
                    var num = 325 - scFloat * 3.25f;

                    canvas.SetShadingFill(shadingPattern);
                    canvas.RoundRectangle(xStarting, y, shadingRectWidth, height, radius);
                    canvas.Fill();

                    canvas.SetColorFill(new BaseColor(128, 128, 128)); //grey
                    canvas.RoundRectangle(xEnding, y, -num, height, radius);
                    canvas.Fill();

                    if (scFloat == 1)
                        scoreImg.SetAbsolutePosition(xEnding - num - scoreImg.Width / 2.5f, y - scoreImg.Height / 4);
                    else
                        scoreImg.SetAbsolutePosition(xEnding - num - scoreImg.Width / 1.5f, y - scoreImg.Height / 4);
                    canvas.AddImage(scoreImg);

                    var showBenchmarkDiamond = false;
                    if (showBenchmarkDiamond)
                    {
                        var tarFloat = float.Parse(target[i]);
                        if (tarFloat == 0)
                            tarFloat = 1;
                        var t = 325 - tarFloat * 3.25f;

                        if (tarFloat == 1)
                            targetImg.SetAbsolutePosition(xEnding - t - targetImg.Width / 2.5f, y - targetImg.Height / 4);
                        else
                            targetImg.SetAbsolutePosition(xEnding - t - targetImg.Width / 1.5f, y - targetImg.Height / 4);
                        canvas.AddImage(targetImg);
                    }

                    y = y - heightArray[i + 1] / 2;
                }
            }
            catch (Exception ex)
            {
                // Add to log file
            }
        }


        private static void AddTargetScoreRowAtTheEnd(PdfPTable table, string username)
        {
            PdfPCell cell;
            Image targetImg = Image.GetInstance(("../../Images/Target.png"));
            targetImg.ScaleToFit(20, 20);
            Chunk targetImgChunk = new Chunk(targetImg, 0, -5);
            var rowFont = FontFactory.GetFont("arial_2", 9, Font.BOLDITALIC, new BaseColor(121, 122, 124));

            Image scoreImg = Image.GetInstance(("../../Images/Score.jpg"));
            scoreImg.ScaleToFit(17, 17);
            Chunk scoreImgChunk = new Chunk(scoreImg, 0, -5);

            var scoreText = "  " + " Rating";
            var targetText = "  Target Rating        ";
            var showBenchmarkDiamond = false;
            if (showBenchmarkDiamond)
            {
                var targetScoreText = new Chunk(targetText, rowFont);
                var candidateScoreText = new Chunk(scoreText, rowFont);
                var phrase = new Phrase();
                phrase.Add(targetImgChunk);
                phrase.Add(targetScoreText);
                phrase.Add(scoreImgChunk);
                phrase.Add(candidateScoreText);
                cell = new PdfPCell(phrase);
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 10f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);
            }
            else
            {
                var candidateScoreText = new Chunk(scoreText, rowFont);
                var phrase = new Phrase();
                phrase.Add(scoreImgChunk);
                phrase.Add(candidateScoreText);
                cell = new PdfPCell(phrase);
                cell.Colspan = 7;
                cell.Border = 0;
                cell.PaddingTop = 10f;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);
            }
        }

        private static List<RatingsDetail> CreateCompetencyMarksRow(List<RatingsDetail> compDetailList, PdfPTable table, PdfPCell cell, Font compNameFont,
           Font definitionFont, Font percentFont, List<float> heightArray, float pageHeight, string pageForReport)
        {
            var competenciesFilledTillNow = new List<RatingsDetail>();
            try
            {
                var tableStartAndEndPointsSum = 80f;
                var totalHeightOccupied = 0f;
                var tableStartPoint = 0f;
                tableStartPoint = 220f;

                tableStartAndEndPointsSum += tableStartPoint;
                for (int i = 0; i < compDetailList.Count; i++)
                {
                    totalHeightOccupied = tableStartAndEndPointsSum + table.TotalHeight;
                    if (pageHeight - totalHeightOccupied > 0)
                    {
                        competenciesFilledTillNow.Add(compDetailList[i]);
                        heightArray.Add(table.Rows[i].MaxHeights);

                        var definition = new Chunk(compDetailList[i].Meaninig, definitionFont);
                        var competencyPhrase = new Phrase(compDetailList[i].Name + "\n\n", compNameFont);
                        competencyPhrase.Add(definition);
                        cell = new PdfPCell(competencyPhrase);
                        cell.HorizontalAlignment = Image.LEFT_ALIGN;
                        cell.Padding = 10f;
                        cell = GetCustomCell(TableCellSpecification.AllDarkGrey, cell);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(compDetailList[i].PercentScore + "%", percentFont));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell = GetCustomCell(TableCellSpecification.AllDarkGrey, cell);
                        table.AddCell(cell);

                        var cellSpec = TableCellSpecification.LeftDarkGrey;
                        if (i == 0)
                        {
                            cellSpec = TableCellSpecification.LeftNTopDarkGrey;
                        }
                        else if (i == compDetailList.Count - 1)
                        {
                            cellSpec = TableCellSpecification.LeftNBottomDarkGrey;
                        }
                        cell = new PdfPCell();
                        cell = GetCustomCell(cellSpec, cell);
                        table.AddCell(cell);


                        cellSpec = TableCellSpecification.AllLightGrey;
                        if (i == 0)
                        {
                            cellSpec = TableCellSpecification.TopDarkGrey;
                        }
                        else if (i == compDetailList.Count - 1)
                        {
                            cellSpec = TableCellSpecification.BottomDarkGrey;
                        }
                        cell = new PdfPCell();
                        cell = GetCustomCell(cellSpec, cell);
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        cell = GetCustomCell(cellSpec, cell);
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        cell = GetCustomCell(cellSpec, cell);
                        table.AddCell(cell);


                        cellSpec = TableCellSpecification.RightDarkGrey;
                        if (i == 0)
                        {
                            cellSpec = TableCellSpecification.TopNRightDarkGrey;
                        }
                        else if (i == compDetailList.Count - 1)
                        {
                            cellSpec = TableCellSpecification.BottomNRightDarkGrey;
                        }
                        cell = new PdfPCell();
                        cell = GetCustomCell(cellSpec, cell);
                        table.AddCell(cell);
                    }
                    else
                    {
                        table.DeleteLastRow();
                        competenciesFilledTillNow.RemoveAt(competenciesFilledTillNow.Count - 1);
                        break;
                    }
                }

                totalHeightOccupied = tableStartAndEndPointsSum + table.TotalHeight;
                //All items are added to table, and last one exceeds the height
                if (compDetailList.Count == competenciesFilledTillNow.Count && pageHeight - totalHeightOccupied < 0)
                {
                    table.DeleteLastRow();
                    competenciesFilledTillNow.RemoveAt(competenciesFilledTillNow.Count - 1);
                }

                heightArray.Add(table.Rows[competenciesFilledTillNow.Count].MaxHeights);
                return competenciesFilledTillNow;
            }
            catch (Exception ex)
            {
                return competenciesFilledTillNow;
                //Add to log file
            }
        }


        private static PdfPCell GetCustomCell(TableCellSpecification specification, PdfPCell cell)
        {
            var lightBrownColor = new BaseColor(136, 100, 21);
            var darkBrownColor = new BaseColor(136, 0, 21);
            var darkSkinBgColor = new BaseColor(242, 222, 217);
            var lightskinbgColor = new BaseColor(244, 230, 225);

            switch (specification)
            {
                case TableCellSpecification.AllDarkGrey:
                    cell.BorderWidth = 1f;
                    cell.BorderColor = darkBrownColor;
                    cell.BackgroundColor = darkSkinBgColor;
                    break;

                case TableCellSpecification.LeftNTopDarkGrey:
                    cell.UseVariableBorders = true;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthTop = 0f;
                    cell.BorderColorBottom = lightBrownColor;
                    cell.BorderColorRight = lightBrownColor;
                    cell.BackgroundColor = lightskinbgColor;
                    break;

                case TableCellSpecification.TopDarkGrey:
                    cell.UseVariableBorders = true;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthTop = 0f;
                    cell.BorderColorRight = lightBrownColor;
                    cell.BorderColorLeft = lightBrownColor;
                    cell.BorderColorBottom = lightBrownColor;
                    cell.BackgroundColor = lightskinbgColor;
                    break;

                case TableCellSpecification.TopNRightDarkGrey:
                    cell.UseVariableBorders = true;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthRight = 1f;
                    cell.BorderWidthTop = 0f;
                    cell.BorderColorRight = darkBrownColor;
                    cell.BorderColorLeft = lightBrownColor;
                    cell.BorderColorBottom = lightBrownColor;
                    cell.BackgroundColor = lightskinbgColor;
                    break;

                case TableCellSpecification.LeftDarkGrey:
                    cell.UseVariableBorders = true;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderColorTop = lightBrownColor;
                    cell.BorderColorBottom = lightBrownColor;
                    cell.BorderColorRight = lightBrownColor;
                    cell.BackgroundColor = lightskinbgColor;
                    break;

                case TableCellSpecification.AllLightGrey:
                    cell.BorderWidth = 1f;
                    cell.BorderColor = lightBrownColor;
                    cell.BackgroundColor = lightskinbgColor;
                    break;

                case TableCellSpecification.RightDarkGrey:
                    cell.UseVariableBorders = true;
                    cell.BorderWidthBottom = 0.5f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthRight = 1f;
                    cell.BorderWidthTop = 0.5f;
                    cell.BorderColorRight = darkBrownColor;
                    cell.BorderColorTop = lightBrownColor;
                    cell.BorderColorBottom = lightBrownColor;
                    cell.BorderColorLeft = lightBrownColor;
                    cell.BackgroundColor = lightskinbgColor;
                    break;

                case TableCellSpecification.LeftNBottomDarkGrey:
                    cell.UseVariableBorders = true;
                    cell.BorderWidthBottom = 1f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthTop = 0f;
                    cell.BorderWidthLeft = 0f;
                    cell.BorderColorBottom = darkBrownColor;
                    cell.BorderColorRight = lightBrownColor;
                    cell.BackgroundColor = lightskinbgColor;
                    break;

                case TableCellSpecification.BottomDarkGrey:
                    cell.UseVariableBorders = true;
                    cell.BorderWidthBottom = 1f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthRight = 0.5f;
                    cell.BorderWidthTop = 0f;
                    cell.BorderColorLeft = lightBrownColor;
                    cell.BorderColorRight = lightBrownColor;
                    cell.BorderColorBottom = darkBrownColor;
                    cell.BackgroundColor = lightskinbgColor;
                    break;

                case TableCellSpecification.BottomNRightDarkGrey:
                    cell.UseVariableBorders = true;
                    cell.BorderWidthBottom = 1f;
                    cell.BorderWidthLeft = 0.5f;
                    cell.BorderWidthRight = 1f;
                    cell.BorderWidthTop = 0f;
                    cell.BorderColorRight = darkBrownColor;
                    cell.BorderColorLeft = lightBrownColor;
                    cell.BorderColorBottom = darkBrownColor;
                    cell.BackgroundColor = lightskinbgColor;
                    break;

                default:
                    break;
            }
            return cell;
        }

        private static PdfPCell CreateHeaderRowForCompetencyTable(PdfPTable table, Font headerFont, BaseColor darkGreyColor)
        {
            PdfPCell cell = new PdfPCell();
            cell = GetCustomCell(TableCellSpecification.AllDarkGrey, cell);
            table.AddCell(cell);

            cell = new PdfPCell();
            cell = GetCustomCell(TableCellSpecification.AllDarkGrey, cell);
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Very Upset", headerFont));
            cell = GetCustomCell(TableCellSpecification.AllDarkGrey, cell);
            cell.PaddingTop = cell.PaddingBottom = 15f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Upset", headerFont));
            cell = GetCustomCell(TableCellSpecification.AllDarkGrey, cell);
            cell.PaddingTop = cell.PaddingBottom = 15f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nothing", headerFont));
            cell = GetCustomCell(TableCellSpecification.AllDarkGrey, cell);
            cell.PaddingTop = cell.PaddingBottom = 15f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Good", headerFont));
            cell = GetCustomCell(TableCellSpecification.AllDarkGrey, cell);
            cell.BorderColor = darkGreyColor;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Yummy", headerFont));
            cell = GetCustomCell(TableCellSpecification.AllDarkGrey, cell);
            cell.PaddingTop = cell.PaddingBottom = 15f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            return cell;
        }

        private static void CalculateOverallScoreAtTheTop(PdfWriter writer, float pageWidth, float pageHeight, List<RatingsDetail> ratingDetail)
        {
            try
            {
                var overallScore = 0f;
                bool hasEmptyWeightage = ratingDetail.Any(x => string.IsNullOrEmpty(x.Weightage));
                if (!hasEmptyWeightage)
                {
                    foreach (var item in ratingDetail)
                    {
                        overallScore += float.Parse(item.PercentScore) * float.Parse(item.Weightage);
                    }
                }
                else
                {
                    var weightage = 1f / ratingDetail.Count;
                    foreach (var item in ratingDetail)
                    {
                        overallScore += float.Parse(item.PercentScore) * weightage;
                    }
                }
                var font = FontFactory.GetFont("MinionPro-Regular_0", 14, Font.BOLD, new BaseColor(69, 85, 96));

                var of = new Chunk("Overall Rating - " + Math.Round(overallScore, MidpointRounding.AwayFromZero) + "*s", font);
                var phrase = new Phrase(of);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, phrase, pageWidth / 2, pageHeight - 210f, 0);
            }
            catch (Exception ex)
            {
                // Add to log file
            }
        }



        private static void MenuPage(List<ItemDetail> GivenItems, bool isFiveItems, Document doc, PdfWriter writer, float pageHeight)
        {
            doc.NewPage();
            Image menuImg = Image.GetInstance(("../../Images/menu.png"));
            if (isFiveItems)
                menuImg.SetAbsolutePosition(190f, pageHeight - (150f + menuImg.Height));
            else
                menuImg.SetAbsolutePosition(190f, pageHeight - (250f + menuImg.Height));
            doc.Add(menuImg);

            Image graySeparatorImg = Image.GetInstance(("../../Images/separator.png"));
            graySeparatorImg.SetAbsolutePosition(220f, pageHeight - 326.5f);
            doc.Add(graySeparatorImg);

            PdfPTable table = new PdfPTable(4);
            PdfPCell cell;
            table.TotalWidth = 500;
            table.LockedWidth = true;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            float[] widths = new float[] { 1f, 2f, 1f, 2f };
            table.SetWidths(widths);
            var itemeFont = FontFactory.GetFont("arial_2", 14, Font.BOLD, new BaseColor(136, 0, 21));
            var quantFont = FontFactory.GetFont("arial_2", 10, Font.BOLD, new BaseColor(136, 100, 21));

            Chunk line = new Chunk(graySeparatorImg, 0, -15);

            int cellCount = 0;
            foreach (var item in GivenItems)
            {
                Image icon = GetIconImage(item);
                cell = new PdfPCell(icon, true);
                cell.BorderWidth = 0;
                if (cellCount >= 2)
                {
                    cell.PaddingTop = isFiveItems ? 25f : 30f;
                }
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                Phrase p = GetPhraseText(item, itemeFont, quantFont, line);
                cell = new PdfPCell(p);
                cell.BorderWidth = 0;

                if (isFiveItems)
                    cell.PaddingTop = cellCount < 2 ? 5f : 30f;
                else
                    cell.PaddingTop = cellCount < 2 ? 5f : 35f;

                cell.PaddingLeft = 10f;
                table.AddCell(cell);

                if (GivenItems.Count == 1 || (GivenItems.Count == 3 && cellCount == 2) || (GivenItems.Count == 5 && cellCount == 4))
                    table.CompleteRow();

                cellCount++;
            }
            if (isFiveItems)
                table.WriteSelectedRows(0, -1, 50f, pageHeight - 360f, writer.DirectContent);
            else
                table.WriteSelectedRows(0, -1, 50f, pageHeight - 390f, writer.DirectContent);
        }

        private static Image GetIconImage(ItemDetail item)
        {
            Image iconImg = null;
            switch (item.Name)
            {
                case "Rice":
                    iconImg = Image.GetInstance(("../../Images/Rice.png"));
                    break;
                case "Dal":
                    iconImg = Image.GetInstance(("../../Images/Dal.png"));
                    break;
                case "Tea":
                    iconImg = Image.GetInstance(("../../Images/Tea.png"));
                    break;
                case "Sugar":
                    iconImg = Image.GetInstance(("../../Images/Sugar.png"));
                    break;
                case "Salt":
                    iconImg = Image.GetInstance(("../../Images/Salt.png"));
                    break;
                case "Pepper":
                    iconImg = Image.GetInstance(("../../Images/Pepper.png"));
                    break;
                default:
                    break;
            }
            return iconImg;
        }

        private static Phrase GetPhraseText(ItemDetail item, Font fontTestName, Font timeFont, Chunk line)
        {
            Chunk testname = null;
            switch (item.Name)
            {
                case "Tea":
                    testname = new Chunk("Tea- Ginger \nMasala\n", fontTestName);
                    break;
                case "Rice":
                    testname = new Chunk("Rice- Hot \nSpice\n", fontTestName);
                    break;
                case "Dal":
                    testname = new Chunk("Dal- Makhani \nKhichdi\n", fontTestName);
                    break;
                case "Sugar":
                    testname = new Chunk("Sugar- Sweet \nSyrup\n", fontTestName);
                    break;
                case "Salt":
                    testname = new Chunk("Salt- Taste \nSalty\n", fontTestName);
                    break;
                case "Pepper":
                    testname = new Chunk("Pepper- Pick \nSoon\n", fontTestName);
                    break;
                default:
                    break;
            }

            var timeTextLine1 = new Chunk("\n\n\nAvailable  -  " + item.TotalWeightKg + "Kg & " + item.TotalWeightGm + "Gm", timeFont);
            var timeTextLine2 = new Chunk("\n\nRequired  -  " + item.RequiredWeightKg + "Kg & " + item.RequiredWeightGm + "Gm", timeFont);
            var phrase = new Phrase(testname);
            phrase.Add(line);
            phrase.Add(timeTextLine1);
            phrase.Add(timeTextLine2);
            return phrase;
        }
        private static void AddAllItems(List<ItemDetail> GivenItems)
        {
            GivenItems.Add(
                       new ItemDetail
                       {
                           Name = "Rice",
                           TotalWeightKg = "5",
                           TotalWeightGm = "20",
                           RequiredWeightKg = "2",
                           RequiredWeightGm = "10"
                       });

            GivenItems.Add(
                           new ItemDetail
                           {
                               Name = "Dal",
                               TotalWeightKg = "9",
                               TotalWeightGm = "29",
                               RequiredWeightKg = "92",
                               RequiredWeightGm = "90"
                           });

            GivenItems.Add(
                           new ItemDetail
                           {
                               Name = "Tea",
                               TotalWeightKg = "65",
                               TotalWeightGm = "6",
                               RequiredWeightKg = "26",
                               RequiredWeightGm = "16"
                           });

            GivenItems.Add(
                           new ItemDetail
                           {
                               Name = "Sugar",
                               TotalWeightKg = "45",
                               TotalWeightGm = "24",
                               RequiredWeightKg = "2",
                               RequiredWeightGm = "14"
                           });
            GivenItems.Add(
                           new ItemDetail
                           {
                               Name = "Salt",
                               TotalWeightKg = "15",
                               TotalWeightGm = "10",
                               RequiredWeightKg = "12",
                               RequiredWeightGm = "1"
                           });
            GivenItems.Add(
                          new ItemDetail
                          {
                              Name = "Pepper",
                              TotalWeightKg = "65",
                              TotalWeightGm = "16",
                              RequiredWeightKg = "62",
                              RequiredWeightGm = "16"
                          });
        }

        private static void FirstPage(Document doc, PdfWriter writer, float pageWidth, float pageHeight)
        {
            Image bgImg = Image.GetInstance("../../Images/Cover.jpg");
            bgImg.ScaleAbsolute(pageWidth, pageHeight);
            bgImg.Alignment = Image.UNDERLYING;
            doc.Add(bgImg);

            Image logoImg = Image.GetInstance("../../Images/logo.jpg");
            logoImg.SetAbsolutePosition(408f, pageHeight - 84f);
            doc.Add(logoImg);

            var welcomefont = FontFactory.GetFont("arial_2", 34, Font.BOLDITALIC, BaseColor.WHITE);
            var textwelocme = new Chunk("WELCOME!", welcomefont);
            var welcomePhrase = new Phrase(textwelocme);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, welcomePhrase, pageWidth / 2, pageHeight / 2, 0);

            var userDetailNormalFont = FontFactory.GetFont("arial_2", 13.5f, Font.NORMAL, BaseColor.WHITE);
            var userDetailBoldFont = FontFactory.GetFont("arial_2", 13.5f, Font.BOLD, BaseColor.YELLOW);
            var line1Text = new Chunk("Hi: ", userDetailNormalFont);
            var usernameText = new Chunk("Manpreet", userDetailBoldFont);
            var line1Phrase = new Phrase(line1Text);
            line1Phrase.Add(usernameText);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, line1Phrase, 558f, pageHeight - 769f, 0);

            var line2Text = new Chunk("Today is: ", userDetailNormalFont);
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var dateTimeNowIst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
            var time = dateTimeNowIst.ToString("dd/MM/yyyy | h:mm:ss tt");
            var timeText = new Chunk(time, userDetailBoldFont);
            var line2Phrase = new Phrase(line2Text);
            line2Phrase.Add(timeText);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, line2Phrase, 558f, pageHeight - 786f, 0);
        }
    }

    public class PdfStartEvent : PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document doc)
        {
            float pageWidth = doc.PageSize.Width;
            float pageHeight = doc.PageSize.Height;

            PdfContentByte canvas = writer.DirectContent;
            canvas.MoveTo(0, pageHeight);
            canvas.LineTo(pageWidth, pageHeight);
            canvas.LineTo(pageWidth, pageHeight - 65f);
            canvas.LineTo(0, pageHeight - 65f);
            canvas.SetColorFill(new BaseColor(55, 98, 83));
            canvas.Fill();

            Image logoImg = Image.GetInstance("../../Images/logo.jpg");
            logoImg.ScaleToFit(80f, 25f);
            logoImg.SetAbsolutePosition(pageWidth / 2 - 40f, pageHeight - 45f);
            canvas.AddImage(logoImg);

            //Image logo2 = Image.GetInstance("../../Images/logo2.png");
            //logo2.ScaleToFit(80f, 25f);
            //logo2.SetAbsolutePosition(460f, pageHeight - 45f);
            //canvas.AddImage(logo2);
        }

    }

    public class PdfEndEvent : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            float pageWidth = document.PageSize.Width;
            float pageHeight = document.PageSize.Height;

            //Footer gray line
            PdfContentByte canvas = writer.DirectContent;
            canvas.SetColorStroke(BaseColor.DARK_GRAY);
            canvas.MoveTo(37f, pageHeight - 807f);
            canvas.LineTo(560f, pageHeight - 807f);
            canvas.SetLineWidth(1f);
            canvas.Stroke();

            //Footer blue box for page number
            canvas.MoveTo(276.75f, 34f);
            canvas.LineTo(318.52f, 34f);
            canvas.LineTo(318.52f, 0);
            canvas.LineTo(276.75f, 0);
            canvas.SetColorFill(new BaseColor(55, 98, 83));
            canvas.Fill();

            var pgFont = FontFactory.GetFont("MinionPro-Regular_0", 14, Font.ITALIC, BaseColor.WHITE);
            var pgNo = new Chunk(document.PageNumber.ToString(), pgFont);
            var pgPhrase = new Phrase(pgNo);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, pgPhrase, pageWidth / 2, pageHeight - 830f, 0);

        }
    }

    public class ItemDetail
    {
        public string Name { get; set; }
        public string TotalWeightKg { get; set; }
        public string TotalWeightGm { get; set; }
        public string RequiredWeightKg { get; set; }
        public string RequiredWeightGm { get; set; }
    }

    public class RatingsDetail
    {
        public int RatingId { get; set; }
        public string Name { get; set; }
        public string Meaninig { get; set; }
        public string TotalScore { get; set; }
        public string PercentScore { get; set; }
        public string MaxRating { get; set; }
        public string DesiredRating { get; set; }
        public string BenchmarkRarting { get; set; }
        public string Weightage { get; set; }
    }
    public enum TableCellSpecification
    {
        AllDarkGrey,
        LeftNTopDarkGrey,
        TopDarkGrey,
        TopNRightDarkGrey,
        LeftDarkGrey,
        RightDarkGrey,
        LeftNBottomDarkGrey,
        BottomDarkGrey,
        BottomNRightDarkGrey,
        AllLightGrey
    }

}
