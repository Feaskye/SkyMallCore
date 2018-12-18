using System.Threading.Tasks;
using SkyCoreLib.Payments.Core;
using SkyCoreLib.Payments.Wechatpay.Parameters.Requests;

namespace SkyCoreLib.Payments.Wechatpay.Abstractions {
    /// <summary>
    /// 微信App支付服务
    /// </summary>
    public interface IWechatpayAppPayService {
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="request">支付参数</param>
        Task<PayResult> PayAsync( WechatpayAppPayRequest request );
    }
}
