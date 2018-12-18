using System.Threading.Tasks;
using SkyCoreLib.Payments.Alipay.Parameters.Requests;

namespace SkyCoreLib.Payments.Alipay.Abstractions {
    /// <summary>
    /// 支付宝二维码支付
    /// </summary>
    public interface IAlipayQrCodePayService {
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="request">支付参数</param>
        Task<string> PayAsync( AlipayPrecreateRequest request );
    }
}
