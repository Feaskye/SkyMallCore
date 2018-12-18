using System.Threading.Tasks;
using SkyCoreLib.Payments.Alipay.Abstractions;
using SkyCoreLib.Payments.Alipay.Configs;
using SkyCoreLib.Payments.Alipay.Parameters;
using SkyCoreLib.Payments.Alipay.Parameters.Requests;
using SkyCoreLib.Payments.Alipay.Services.Base;
using SkyCoreLib.Payments.Core;

namespace SkyCoreLib.Payments.Alipay.Services {
    /// <summary>
    /// 支付宝App支付服务
    /// </summary>
    public class AlipayAppPayService : AlipayServiceBase, IAlipayAppPayService {
        /// <summary>
        /// 初始化支付宝App支付服务
        /// </summary>
        /// <param name="provider">支付宝配置提供器</param>
        public AlipayAppPayService( IAlipayConfigProvider provider ) : base( provider ) {
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="request">支付参数</param>
        public async Task<string> PayAsync( AlipayAppPayRequest request ) {
            var result = await PayAsync( request.ToParam() );
            return result.Result;
        }

        /// <summary>
        /// 请求结果
        /// </summary>
        protected override Task<PayResult> RequstResult( AlipayConfig config, AlipayParameterBuilder builder ) {
            var result = builder.Result( true );
            WriteLog( config, builder, result );
            return Task.FromResult( new PayResult { Result = result } );
        }

        /// <summary>
        /// 获取请求方法
        /// </summary>
        protected override string GetMethod() {
            return "alipay.trade.app.pay";
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        protected override PayWay GetPayWay() {
            return PayWay.AlipayAppPay;
        }
    }
}