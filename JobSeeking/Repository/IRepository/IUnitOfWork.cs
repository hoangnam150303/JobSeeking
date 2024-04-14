namespace JobSeeking.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IJobRepository JobRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        INewsRepository NewsRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        void Save();
    }
}
