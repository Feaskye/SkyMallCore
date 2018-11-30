using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SkyMallCore.Core
{
    public class NpoiWorker
    {
        
        /// <summary>
        /// 导出Execl
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static byte[] ExportToExcel<T>(List<T> datas, string sheetName = "导出数据") where T : new()
        {
            byte[] buffer = null;
            MemoryStream ms = new MemoryStream();
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);

            var propertys = Reflector.GetProperties<T>();
            string displayName = string.Empty;
            int piIndex = 0, rowIndex = 1;
            foreach (var pi in propertys)
            {
                //不导出属性
                //if (pi.GetCustomAttribute<NotExportAttribute>() != null)
                //{
                //    piIndex++;
                //    continue;
                //}
                //需要类反射出字段属性名
                displayName = pi.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                if (!displayName.Equals(string.Empty))
                {//如果该属性指定了DisplayName，则输出
                    try
                    {
                        headerRow.CreateCell(piIndex).SetCellValue(displayName);
                    }
                    catch (Exception)
                    {
                        headerRow.CreateCell(piIndex).SetCellValue("");
                    }
                }
                piIndex++;
            }
            foreach (T data in datas)
            {
                piIndex = 0;
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (var pi in propertys)
                {
                    //if (pi.GetCustomAttribute<NotExportAttribute>() != null)
                    //{
                    //    piIndex++;
                    //    continue;
                    //}
                    try
                    {
                        dataRow.CreateCell(piIndex).SetCellValue(pi.GetValue(data, null).ToString());
                    }
                    catch (Exception)
                    {
                        dataRow.CreateCell(piIndex).SetCellValue("");
                    }
                    piIndex++;
                }
                rowIndex++;
            }
            workbook.Write(ms);
            buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);
            ms.Flush();
            return buffer;
        }

    }
}
