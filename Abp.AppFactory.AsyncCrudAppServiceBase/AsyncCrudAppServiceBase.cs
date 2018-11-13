using Abp.AppFactory.Interfaces;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System.Threading.Tasks;

namespace Abp.AppFactory
{
    public class AsyncCrudAppServiceBase<TEntity, TEntityDto> : AsyncCrudAppServiceBase<TEntity, TEntityDto, int>
        where TEntity : Entity
        where TEntityDto : EntityDto
    {
        public AsyncCrudAppServiceBase(IRepository<TEntity, int> repository, ISyncHub syncHub) : base(repository, syncHub)
        {
        }
    }

    public class AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey> : AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntity : Entity<TPrimaryKey>
        where TEntityDto : EntityDto<TPrimaryKey>
    {
        public AsyncCrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository, ISyncHub syncHub) : base(repository, syncHub)
        {
        }
    }

    public class AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput> : AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : Entity<TPrimaryKey>
        where TEntityDto : EntityDto<TPrimaryKey>
    {
        public AsyncCrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository, ISyncHub syncHub) : base(repository, syncHub)
        {
        }
    }

    public class AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> : AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        public AsyncCrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository, ISyncHub syncHub) : base(repository, syncHub)
        {
        }
    }

    public class AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput> : AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        public AsyncCrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository, ISyncHub syncHub) : base(repository, syncHub)
        {
        }
    }

    public class AsyncCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput> : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>

        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        private readonly ISyncHub syncHub;

        public AsyncCrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository, ISyncHub syncHub) : base(repository)
        {
            this.syncHub = syncHub;
        }

        /// <summary>
        /// When Sync() is called the UoW state will be saved and SignalR will transmit a key containing the type of <c>TEntityDto</c> the front end should refresh.
        /// </summary>
        protected void Sync()
        {
            CurrentUnitOfWork.SaveChanges();
            syncHub.Sync(typeof(TEntityDto)).Wait();
        }

        public override async Task<TEntityDto> Create(TCreateInput input)
        {
            var output = await base.Create(input);
            Sync();
            return output;
        }

        public override async Task<TEntityDto> Update(TUpdateInput input)
        {
            var output = await base.Update(input);
            Sync();
            return output;
        }

        public override async Task Delete(TDeleteInput input)
        {
            await base.Delete(input);
            Sync();
        }
    }
}