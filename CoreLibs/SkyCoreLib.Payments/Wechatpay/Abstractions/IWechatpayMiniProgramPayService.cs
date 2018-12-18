using System.Threading.Tasks;
using SkyCoreLib.Payments.Core;
using SkyCoreLib.Payments.Wechatpay.Parameters.Requests;

namespace SkyCoreLib.Payments.Wechatpay.Abstractions {
    /// <summary>
    /// 微信小程序支付服务
    /// </summary>
    public interface IWechatpayMiniProgramPayService {
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="request">支付参数</param>
        Task<PayResult> PayAsync( WechatpayMiniProgramPayRequest request );
    }
}