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
    public class ChartController : IDisposable
    {
        private Chart m_chart;

        public ChartController()
        {
            m_chart = new Chart();
        }

        public Chart DrawChart(Int32 iType)
        {
            //Chart setting 
            m_chart.Height = Unit.Pixel(480);
            m_chart.Width = Unit.Pixel(640);
            m_chart.ImageType = ChartImageType.Png;
            m_chart.BorderlineColor = System.Drawing.ColorTranslator.FromHtml("#6F5F08");
            m_chart.BorderlineDashStyle = ChartDashStyle.Solid;
            m_chart.BorderWidth = 1;
            m_chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            m_chart.Palette = ChartColorPalette.BrightPastel;
            m_chart.BackSecondaryColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            m_chart.BackGradientStyle = GradientStyle.TopBottom;
            m_chart.BackColor = System.Drawing.ColorTranslator.FromHtml("#DDDBC7");
            m_chart.AntiAliasing = AntiAliasingStyles.All;
            BorderSkin borderskin = new BorderSkin();
            borderskin.SkinStyle = BorderSkinStyle.Emboss;
            m_chart.BorderSkin = borderskin;


            //Legend
            Legend mainLegend = new Legend();
            mainLegend.Name = "Default";
            mainLegend.IsTextAutoFit = false;
            mainLegend.BackColor = Color.Transparent;
            mainLegend.BorderWidth = 1;
            mainLegend.BorderColor = Color.Black;
            mainLegend.BorderDashStyle = ChartDashStyle.Solid;
            mainLegend.Font = new Font("Times New Roman", 12.0f, FontStyle.Bold);
            mainLegend.Alignment = StringAlignment.Near;
            mainLegend.Docking = Docking.Bottom;
            m_chart.Legends.Add(mainLegend);

            //Chart Area
            //ChartArea mainArea = new ChartArea();
            //mainArea.Name = "mainArea";
            //mainArea.BackColor = Color.FromArgb(255, 255, 192);
            //mainArea.BackGradientStyle = GradientStyle.TopBottom;
            //mainArea.BorderDashStyle = ChartDashStyle.Solid;
            //m_chart.ChartAreas.Add(mainArea);


            //Chart Area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "chartArea";
            chartArea.BorderColor = System.Drawing.ColorTranslator.FromHtml("#404040");
            chartArea.BackSecondaryColor = Color.White;
            chartArea.BackColor = Color.FromArgb(96, 189, 182, 124);
            chartArea.BackGradientStyle = GradientStyle.TopBottom;

            //Area 3D Style
            ChartArea3DStyle areaStyle = new ChartArea3DStyle();
            areaStyle.Rotation = 10;
            areaStyle.Perspective = 10;
            areaStyle.LightStyle = LightStyle.Realistic;
            areaStyle.Enable3D = true;
            areaStyle.Inclination = 10;
            areaStyle.PointDepth = 200;
            areaStyle.IsRightAngleAxes = false;
            areaStyle.WallWidth = 0;
            areaStyle.IsClustered = false;

            chartArea.Area3DStyle = areaStyle;

            Axis axisX = new Axis();
            axisX.LineColor = Color.FromArgb(32, 64, 64, 64);
            axisX.Title = "Date";
            axisX.TitleAlignment = StringAlignment.Center;
            axisX.TitleFont = new Font("Times New Roman", 12.0f);
            axisX.IntervalOffsetType = DateTimeIntervalType.Number;
            axisX.IntervalType = DateTimeIntervalType.Number;
            LabelStyle xaxisLabel = new LabelStyle();
            xaxisLabel.Font = new Font("Times New Roman", 12.0f);
            axisX.LabelStyle = xaxisLabel;

            Grid xmajorGrid = new Grid();
            xmajorGrid.LineColor = Color.FromArgb(32, 64, 64, 64);
            axisX.MajorGrid = xmajorGrid;

            chartArea.AxisX = axisX;

            Axis axisY = new Axis();
            axisY.LineColor = Color.FromArgb(32, 64, 64, 64);
            axisY.Interval = 1;

            LabelStyle yaxisLabel = new LabelStyle();
            yaxisLabel.Font = new Font("Times New Roman", 12.0f);
            axisY.LabelStyle = yaxisLabel;

            Grid ymajorGrid = new Grid();
            ymajorGrid.LineColor = Color.FromArgb(32, 64, 64, 64);
            axisY.MajorGrid = ymajorGrid;

            chartArea.AxisY = axisY;

            m_chart.ChartAreas.Add(chartArea);

            Series sr = new Series();

            sr.ChartType = SeriesChartType.Spline;
            sr.Name = "DataSeries";


            //generate some point for the chart
            for (Int32 i = 0; i < 50; i++)
            {
                Double x = (Double)i;
                Random rand = new Random();
                //value of the y depend on parameter iType
                switch (iType)
                {
                    case 1:
                        {
                            sr.Points.AddXY(x, rand.Next(500));
                        } break;
                    case 2:
                        {
                            sr.Points.AddXY(x, Math.Cos(x));
                        } break;
                    case 3:
                        {
                            sr.Points.AddXY(x, Math.Tan(x));
                        } break;
                }

            }
            m_chart.Series.Add(sr);

            return m_chart;
        }

        public Chart DrawChartColumn()
        {

            m_chart.Width = 900;

            m_chart.Height = 600;

            //It renders chart as an image and places it into an &lt;img&gt; html tag.

            m_chart.RenderType = RenderType.ImageTag;

            //Adds title to chart

            Title t = new Title("Sample Chart", Docking.Top, new System.Drawing.Font("Tahoma", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));

            m_chart.Titles.Add(t);

            return m_chart;

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
