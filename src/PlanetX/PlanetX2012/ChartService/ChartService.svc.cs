using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using ChartService.Controller;

namespace ChartService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ChartService : IChartService
    {
        #region Private Members
        private string CACHEFOLDERPATH = "CacheFolder";
        private string cacheFolderPath;
        private bool alreadyLoadedCacheFolderPath;
        private string IMAGEURLPATH = "ImageUrl";
        private string imageUrlPath;
        private bool alreadyLoadedImageUrlPath;
        #endregion

        # region Properties
        public string GetCacheFolderPath
        {
            get
            {
                if (!alreadyLoadedCacheFolderPath)
                    LoadCacheFolderPath();

                return cacheFolderPath;
            }
            set
            {
                cacheFolderPath = value;
                alreadyLoadedCacheFolderPath = true;
            }
        }

        public string GetImageUrlPath
        {
            get
            {
                if (!alreadyLoadedImageUrlPath)
                    LoadImageUrlPath();

                return imageUrlPath;
            }
            set
            {
                imageUrlPath = value;
                alreadyLoadedImageUrlPath = true;
            }
        }
        #endregion

        #region Utilites
        protected void LoadCacheFolderPath()
        {
            if (cacheFolderPath == null)
            {
                cacheFolderPath = ConfigurationManager.AppSettings[CACHEFOLDERPATH].ToString();
            }
            alreadyLoadedCacheFolderPath = true;
        }

        protected void LoadImageUrlPath()
        {
            if (imageUrlPath == null)
            {
                imageUrlPath = ConfigurationManager.AppSettings[IMAGEURLPATH].ToString();
            }
            alreadyLoadedImageUrlPath = true;
        }
        #endregion

        public String GetChartUrl(string iType)
        {
            int chartType;
            if (Int32.TryParse(iType, out chartType))
            {
                //class that creates the Chart object
                //ChartController drawChart = new ChartController();
                //Chart m_chart = drawChart.DrawChart(chartType);

                FinancialChart drawFinancialChart = new FinancialChart();
                Chart m_chart = drawFinancialChart.DrawForeCastingChart(chartType);

                String tempFileName = String.Format("Chart_{0}.png", System.Guid.NewGuid().ToString());

                m_chart.SaveImage(GetCacheFolderPath + tempFileName);
                String strImageSrc = GetImageUrlPath + tempFileName;
                return strImageSrc;
            }
            return string.Empty;
        }
    }
}
