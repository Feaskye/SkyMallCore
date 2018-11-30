using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCoreWeb.AppCode
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ChartSeriesAttribute : Attribute
    {
        public SeriesType SeriesType;
        public ChartSeriesAttribute(SeriesType seriesType)
        {
            SeriesType = seriesType;
        }
    }



    public enum SeriesType
    {
        //柱状图
        bar,
        //折线图
        line,
        //饼形图
        pie
    }

}
