using System;
using SkyCoreLib.Payments.Wechatpay.Configs;
using SkyCoreLib.Payments.Wechatpay.Enums;
using SkyCoreLib.Utils.Signatures;

namespace SkyCoreLib.Payments.Wechatpay.Signatures {
    /// <summary>
    /// 微信支付签名工厂
    /// </summary>
    public class SignManagerFactory {
        /// <summary>
        /// 创建签名服务
        /// </summary>
        /// <param name="config">微信支付配置</param>
        /// <param name="builder">参数生成器</param>
        public static ISignManager Create( WechatpayConfig config, ParameterBuilder builder ) {
            if( config.SignType == WechatpaySignType.Md5 )
                return new Md5SignManager( new SignKey( config.PrivateKey ), builder );
            if( config.SignType == WechatpaySignType.HmacSha256 )
                return new HmacSha256SignManager( new SignKey( config.PrivateKey ), builder );
            throw new NotImplementedException( $"未实现签名算法:{config.SignType.Description()}" );
        }
    }
}
