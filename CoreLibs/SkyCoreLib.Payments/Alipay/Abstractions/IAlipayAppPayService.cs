using System.Threading.Tasks;
using SkyCoreLib.Payments.Alipay.Parameters.Requests;

namespace SkyCoreLib.Payments.Alipay.Abstractions {
    /// <summary>
    /// 支付宝App支付服务
    /// </summary>
    public interface IAlipayAppPayService {
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="request">支付参数</param>
        Task<string> PayAsync( AlipayAppPayRequest request );
    }
}