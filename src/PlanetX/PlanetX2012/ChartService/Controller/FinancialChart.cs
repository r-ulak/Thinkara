using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.Data;

namespace ChartService.Controller
{
    public class FinancialChart : IDisposable
    {
        private Chart m_chart;

        public FinancialChart()
        {
            m_chart = new Chart();
        }

        public Chart DrawForeCastingChart(Int32 iType)
        {
            //Chart setting 
            m_chart.Height = Unit.Pixel(300);
            m_chart.Width = Unit.Pixel(450);
            m_chart.ImageType = ChartImageType.Png;
            m_chart.ImageLocation = "~/TempImages/ChartPic_#SEQ(300,3)";
            m_chart.BackColor = System.Drawing.ColorTranslator.FromHtml("#D3DFF0");
            m_chart.BorderlineDashStyle = ChartDashStyle.Solid;
            m_chart.Palette = ChartColorPalette.BrightPastel;
            m_chart.BackGradientStyle = GradientStyle.TopBottom;
            m_chart.BorderWidth = 2;
            m_chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            //Legend
            Legend mainLegend = new Legend();
            mainLegend.LegendStyle = LegendStyle.Row;
            mainLegend.IsTextAutoFit = false;
            mainLegend.Docking = Docking.Bottom;
            mainLegend.IsDockedInsideChartArea = false;
            mainLegend.Name = "Default";
            mainLegend.BackColor = Color.Transparent;
            mainLegend.Font = new Font("Trebuchet MS", 12.0f, FontStyle.Bold);
            mainLegend.Alignment = StringAlignment.Near;
            m_chart.Legends.Add(mainLegend);

            //BorderSkin
            BorderSkin borderskin = new BorderSkin();
            borderskin.SkinStyle = BorderSkinStyle.Emboss;
            m_chart.BorderSkin = borderskin;

            ChartArea mainArea = new ChartArea();
            mainArea.Name = "ChartArea1";
            mainArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
            mainArea.BorderDashStyle = ChartDashStyle.Solid;
            mainArea.BackSecondaryColor = Color.Transparent;
            mainArea.BackColor = Color.FromArgb(64, 165, 191, 228);
            mainArea.ShadowColor = Color.Transparent;
            mainArea.BackGradientStyle = GradientStyle.TopBottom;

            Axis axisy2 = new Axis();
            axisy2.IsLabelAutoFit = false;
            Axis axisx2 = new Axis();
            axisx2.IsLabelAutoFit = false;
            mainArea.AxisX2 = axisx2;
            mainArea.AxisY2 = axisy2;

            ChartArea3DStyle areaStyle = new ChartArea3DStyle();
            areaStyle.Rotation = 10;
            areaStyle.Perspective = 10;
            areaStyle.Inclination = 15;
            areaStyle.IsRightAngleAxes = false;
            areaStyle.WallWidth = 0;
            areaStyle.IsClustered = false;
            mainArea.Area3DStyle = areaStyle;

            Axis axisY = new Axis();
            axisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            axisY.IsLabelAutoFit = false;
            axisY.IsStartedFromZero = false;

            LabelStyle yaxisLabel = new LabelStyle();
            yaxisLabel.Font = new Font("Trebuchet MS", 12.0f, FontStyle.Bold);
            axisY.LabelStyle = yaxisLabel;

            Grid ymajorGrid = new Grid();
            ymajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            axisY.MajorGrid = ymajorGrid;

            mainArea.AxisY = axisY;

            Axis axisX = new Axis();
            axisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            axisX.IsLabelAutoFit = false;

            LabelStyle xaxisLabel = new LabelStyle();
            xaxisLabel.Font = new Font("Trebuchet MS", 12.0f, FontStyle.Bold);
            xaxisLabel.Format = "dd MMM";
            axisX.LabelStyle = xaxisLabel;

            Grid xmajorGrid = new Grid();
            xmajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            axisX.MajorGrid = xmajorGrid;



            mainArea.AxisX = axisX;


            m_chart.ChartAreas.Add(mainArea);

            //Series
            Series srRange = new Series("Range");
            srRange.YValuesPerPoint = 2;
            srRange.XValueType = ChartValueType.DateTime;
            srRange.ChartType = SeriesChartType.Range;
            srRange.BorderColor = Color.FromArgb(180, 26, 59, 105);
            srRange.Color = Color.FromArgb(128, 65, 140, 240);

            m_chart.Series.Add(srRange);

            Series srForecasting = new Series("Forecasting");
            srForecasting.BorderWidth = 2;
            srForecasting.XValueType = ChartValueType.DateTime;
            srForecasting.ChartType = SeriesChartType.Line;
            srForecasting.ShadowColor = Color.FromArgb(64, 0, 0, 0);
            srForecasting.Color = Color.FromArgb(252, 180, 65);
            srForecasting.ShadowOffset = 1;

            m_chart.Series.Add(srForecasting);


            Series srInput = new Series("Input");
            srInput.BorderWidth = 2;
            srInput.YValuesPerPoint = 4;
            srInput.XValueType = ChartValueType.DateTime;
            srInput.ChartType = SeriesChartType.Line;
            srInput.ShadowColor = Color.FromArgb(64, 0, 0, 0);
            srInput.Color = Color.FromArgb(224, 64, 10);
            srInput.ShadowOffset = 1;

            m_chart.Series.Add(srInput);

            Data();
            string typeRegression = "Power";
            int forecasting = 30;
            string error = "True";
            string forecastingError = "True";
            string parameters = typeRegression + ',' + forecasting + ',' + error + ',' + forecastingError;
            m_chart.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, parameters, "Input:Y", "Forecasting:Y,Range:Y,Range:Y2");

            return m_chart;

        }

        /// <summary>
        /// Random Stock Data Generator
        /// </summary>
        /// <param name="series">Data series</param>
        private void Data()
        {
            Random rand;
            // Use a number to calculate a starting value for 
            // the pseudo-random number sequence
            Random randSeed = new Random();
            rand = new Random(randSeed.Next());


            // The number of days for stock data
            int period = 200;

            // The first High value
            double high = rand.NextDouble() * 40;
            if (high <= 0)
            {
                high = -1 * high + 1;
            }

            // The first Close value
            double close = high - rand.NextDouble();

            // The first Low value
            double low = close - rand.NextDouble();

            // The first Volume value
            double volume = 100 + 15 * rand.NextDouble();

            // The first day X and Y values
            m_chart.Series["Input"].Points.AddXY(DateTime.Parse("1/2/2002"), high);
            m_chart.Series["Input"].Points[0].YValues[1] = low;

            // The Open value is not used.
            m_chart.Series["Input"].Points[0].YValues[2] = close;
            m_chart.Series["Input"].Points[0].YValues[3] = close;

            // Days loop
            for (int day = 1; day <= period; day++)
            {

                // Calculate High, Low and Close values
                high = m_chart.Series["Input"].Points[day - 1].YValues[2] + rand.NextDouble();
                if (high <= 0)
                {
                    high = -1 * high + 1;
                }
                close = high - rand.NextDouble();
                low = close - rand.NextDouble();

                // The low cannot be less than yesterday close value.
                if (low > m_chart.Series["Input"].Points[day - 1].YValues[2])
                    low = m_chart.Series["Input"].Points[day - 1].YValues[2];

                // Set data points values
                m_chart.Series["Input"].Points.AddXY(day, high);
                m_chart.Series["Input"].Points[day].XValue = m_chart.Series["Input"].Points[day - 1].XValue + 1;
                m_chart.Series["Input"].Points[day].YValues[1] = low;
                m_chart.Series["Input"].Points[day].YValues[2] = close;
                m_chart.Series["Input"].Points[day].YValues[3] = close;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                m_chart.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}