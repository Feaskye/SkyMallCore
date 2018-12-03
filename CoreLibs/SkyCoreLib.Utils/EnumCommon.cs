using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SkyMallCore.Core
{
    public class EnumCommon
    {
        public static string GetDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }



        public static Dictionary<string,string> GetEnumList<T>()
        {
            var list = new Dictionary<string, string>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                var desc = e.ToString();
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    desc = da.Description;
                }
                list.Add(Convert.ToInt32(e).ToString(), desc);
            }
            return list;
        }

        //public static T GetEnum<T>(string val)
        //{
        //    T result = default(T);
        //    if (Enum.TryParse(val, out result))
        //    {
        //        return result;
        //    }
        //    return result;
        //}
    }
}
