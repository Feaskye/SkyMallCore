using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [DisplayName("来电记录")]
    [Table("LineMessage")]
    public class LineMessage : ModelEntity
    {

        /// <summary>
        /// 产品大类	
        /// </summary>
        public string ProType { get; set; }
        // 产品型号	  
        public string ProCode { get; set; }
        // 服务区域	  
        public string ServiceArea { get; set; }
        // 解决办法	  
        public string Solutions { get; set; }
        // 服务备忘	  
        public string ServiceMark { get; set; }
        // 购买价格	  
        public string BuyPrice { get; set; }
        // 产品序列号
        public string ProLience { get; set; }
        // 会员类别	  
        public string MemType { get; set; }
        // 会员账号   
        public string MemAccount { get; set; }
        // 常用配件   
        public string Accessories { get; set; }
        // 购买商场   
        public string BuyMarket { get; set; }
        // 反馈意见   
        public string Feedback { get; set; }
        // 发票号       
        public string InvoiceNum { get; set; }
        // 保修时间   
        public string XiuTime { get; set; }
        // 登记人员   
        public string RecodMan { get; set; }
        // 进展阶段   
        public string ProgresStage { get; set; }
        // 相关图片   
        public string Pictures { get; set; }
        //反馈问题    
        public string FeedQuestion { get; set; }
        public string AddFiled1 { get; set; }
        public string AddFiled2 { get; set; }
        public string AddFiled3 { get; set; }
        public string AddFiled4 { get; set; }
        public string AddFiled5 { get; set; }
        public string AddFiled6 { get; set; }
        public string AddFiled7 { get; set; }
        public string AddFiled8 { get; set; }
        public string AddFiled9 { get; set; }
        public string AddFiled10 { get; set; }
        public string AddFiled11 { get; set; }
        public string AddFiled12 { get; set; }
        public string MemberId { get; set; }
    }
}
