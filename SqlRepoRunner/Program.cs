using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using SqlRepoRunner.Model;
using SqlRepository;
using Xunit;

namespace SqlRepoRunner
{
    public class MainSqlDb : SqlDbEntity
    {
    }

    // This is not a test nor is it designed to be just to ensure i have not broken anything

    public class Program
    {
        static IContainer ContainerBuilder()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register<IRepositoryBase<MainSqlDb>>(x => new SqlRepository<MainSqlDb>(new TestDb()))
                .InstancePerLifetimeScope();
            return containerBuilder.Build();
        }

        /*
         *
         * 
delete from BookOrders
delete from Orders

delete from Books
         */

        [Fact]
        public async Task Main()
        {
            var container = ContainerBuilder();
            var repo = container.Resolve<IRepositoryBase<MainSqlDb>>();

            // insure we can insert
            var bookOrder = new BookOrders
            {
                PriceAtCapture = 32,
                Book = new Books
                {
                    Name = "big book",
                    Cost = 12,
                    Authour = "test person",
                    Blurb = "a love story "
                },
                Order = new Orders
                {
                    Address = "123 fakeStreet",
                    CreateDate = DateTime.Now,
                    Customer = "mike",
                }
            };
            await repo.Add(bookOrder);
            await repo.Save();
            var info = await repo.GetAll<Orders>().FirstAsync();
            info.ShouldBe(bookOrder.Order);


            //insure we can update
            var repo2 = container.Resolve<IRepositoryBase<MainSqlDb>>();
            info.Customer = "rock";
            await repo2.Save();
            info = await repo.GetAll<Orders>().FirstAsync();
            info.Customer.ShouldBe("rock");

            // ensure projections work
            var test = await repo.GetAll<BookOrders, String>(f => true, f => f.PriceAtCapture.ToString("N")).FirstOrDefaultAsync();
            test.ShouldBe(bookOrder.PriceAtCapture.ToString("N"));

            var bookOrder2 = await repo.GetAll<BookOrders>().FirstAsync();
            // ensure we can get child objects
            bookOrder2.Book.Authour.ShouldBe(bookOrder.Book.Authour);

            // insure we can delete
            await repo.Delete(bookOrder2);
            await repo.Save();
            bookOrder = await repo.GetAll<BookOrders>().FirstOrDefaultAsync();
            bookOrder.ShouldBeNull();
        }
    }
}