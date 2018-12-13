using SkyCore.GlobalProvider;
using SkyCoreLib.Utils;
using SkyMallCore.Data;
using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SkyMallCore.Respository
{
    /// <summary>
    /// 数据审计    （目前是增加、修改当前数据审计，即记录日志）
    /// 大型企业订单项目可考虑录入数据审计表
    /// </summary>
    /// <typeparam name="TCreatorEntity"></typeparam>
    public class AuditedRespository<TCreatorEntity> : RespositoryBase<TCreatorEntity> where TCreatorEntity : CreatorEntity, new()
    {
        public string TableScName = null;
        ISysLogRespository _SysLogRespository;//取出表名Name
        public AuditedRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        {
            _SysLogRespository = CoreContextProvider.GetService<ISysLogRespository>();
            var modelType = typeof(TCreatorEntity).GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault();
            if (modelType != null)
            {
                TableScName =((DisplayNameAttribute)modelType).DisplayName;
            }
        }


        public override bool Insert(TCreatorEntity entity)
        {
            var createrEntity= GetCreatorEntity(entity);
            if (createrEntity.GetType().BaseType == typeof(ModelEntity))
            {
                var modelEntity = GetModelEntity(entity as ModelEntity);
            }
            var b = base.Insert(createrEntity);
            AuditData(b,$"插入{TableScName}编号："+ entity.Id);
            return b;
        }
        

        public override int Insert(List<TCreatorEntity> entitys)
        {
            return base.Insert(entitys.Select(u => GetCreatorEntity(u)).ToList());
        }

        public override bool Update(TCreatorEntity entity)
        {
            var b= base.Update(UpdateCreatorEntity(entity));
            AuditData(b, $"修改{TableScName}编号：" + entity.Id);
            return b;
        }

        //删除暂不需要
        public override bool Delete(TCreatorEntity entity)
        {
            var b= base.Delete(entity);
            AuditData(b, $"删除{TableScName}编号：" + entity.Id);
            return b;
        }



        public virtual void AuditData(bool result,string message)
        {
            if (CoreContextProvider.CurrentSysUser == null)
                return;
            var sysLog = new SysLog();
            sysLog.Id = Common.GuId();
            sysLog.Date = DateTime.Now;
            sysLog.Account = CoreContextProvider.CurrentSysUser == null ? "" : CoreContextProvider.CurrentSysUser.Account;
            sysLog.NickName = CoreContextProvider.CurrentSysUser == null ? "" : CoreContextProvider.CurrentSysUser.RealName;
            sysLog.IPAddress = CoreContextProvider.HttpContext.GetIP();
            sysLog.IPAddressName = NetClient.GetLocation(sysLog.IPAddress);
            sysLog.Result = result;
            sysLog.Description = message;
            System.Threading.Tasks.Task.Factory.StartNew(() => {
                try
                {
                    var logService = CoreContextProvider.GetService<ISysLogRespository>(true);
                    logService.OperatLog(sysLog);
                }
                catch (Exception ex)
                { }
            });
        }



        private TModelEntity GetModelEntity<TModelEntity>(TModelEntity entity) where TModelEntity : ModelEntity
        {
            if (!entity.EnabledMark.HasValue)
            {
                entity.EnabledMark = true;
            }
            return entity;
        }

        private TCreatorEntity UpdateCreatorEntity(TCreatorEntity entity)
        {
            if (entity.GetType().BaseType == typeof(ModelEntity))
            {
                var updateEntity = entity as ModelEntity;
                if (updateEntity.LastModifyTime == null)
                {
                    updateEntity.LastModifyTime = DateTime.Now;
                    updateEntity.LastModifyUserId = CoreContextProvider.CurrentSysUser == null ? entity.CreatorUserId : CoreContextProvider.CurrentSysUser.UserId;
                    return updateEntity as TCreatorEntity;
                }
            }
            return entity;
        }


        private TCreatorEntity GetCreatorEntity(TCreatorEntity entity)
        {
            if (entity.CreatorTime == null)
            {
                entity.CreatorTime = DateTime.Now;
                entity.CreatorUserId = CoreContextProvider.CurrentSysUser == null ? entity.CreatorUserId : CoreContextProvider.CurrentSysUser.UserId;
            }
            return entity;
        }






    }
}
