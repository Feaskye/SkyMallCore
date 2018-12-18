using SkyCoreLib.Payments.Alipay.Configs;
using SkyCoreLib.Payments.Wechatpay.Configs;

namespace SkyCoreLib.Payments.Extensions {
    /// <summary>
    /// 支付配置
    /// </summary>
    public class PayOptions {
        /// <summary>
        /// 支付宝配置
        /// </summary>
        public AlipayConfig AlipayOptions { get; set; } = new AlipayConfig();
        /// <summary>
        /// 微信支付配置
        /// </summary>
        public WechatpayConfig WechatpayOptions { get; set; } = new WechatpayConfig();
    }
}
