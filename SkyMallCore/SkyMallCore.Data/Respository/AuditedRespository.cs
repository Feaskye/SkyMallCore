using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyMallCore.Data.Respository
{
    /// <summary>
    /// 数据审计    （目前是增加、修改当前数据审计）
    /// 大型企业订单项目可考虑录入数据审计表
    /// </summary>
    /// <typeparam name="TCreatorEntity"></typeparam>
    public class AuditedRespository<TCreatorEntity> : RespositoryBase<TCreatorEntity> where TCreatorEntity : CreatorEntity, new()
    {
        public AuditedRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        { }


        public override int Insert(TCreatorEntity entity)
        {
            return base.Insert(GetCreatorEntity(entity));
        }

        public override int Insert(List<TCreatorEntity> entitys)
        {
            return base.Insert(entitys.Select(u => GetCreatorEntity(u)).ToList());
        }

        public override int Update(TCreatorEntity entity)
        {
            return base.Update(UpdateCreatorEntity(entity));
        }

        //删除暂不需要
        //public override int Delete(TCreatorEntity entity)
        //{
        //    return base.Delete(entity);
        //}




        private TCreatorEntity UpdateCreatorEntity(TCreatorEntity entity)
        {
            if (entity.GetType().BaseType == typeof(ModelEntity))
            {
                var updateEntity = entity as ModelEntity;
                if (updateEntity.LastModifyTime == null)
                {
                    updateEntity.LastModifyTime = DateTime.Now;
                    updateEntity.LastModifyUserId = Core.CoreContextProvider.CurrentSysUser.UserId;
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
                entity.CreatorUserId = Core.CoreContextProvider.CurrentSysUser.UserId;
            }
            return entity;
        }


    }
}
