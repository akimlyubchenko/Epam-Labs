using System;
using System.Threading;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cacher = CacherServices.CacherServices;
using ILoggingServices = CacherServices.ILoggingServices;

namespace CachingSystem.Test
{
    [TestClass]
    public class CacherServicesTests
    {
        private readonly ILoggingServices logger = new CacherServices.LoggingServices();

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Set_Get_ThrowException()
        {
            MyClass oleg = new MyClass("Oleg", 54);


            Cacher cache = new Cacher(logger);

            cache.SetCache(oleg, 1000, "oleg's tag");

            var obj2 = cache.GetCache("oleg's tag");

            Thread.Sleep(1005);

            var objThrowException = cache.GetCache("oleg's tag");
        }

        [TestMethod]
        public void Set_Get_CheckingForCoincidence()
        {
            MyClass akim = new MyClass("Akim", 19);

            Cacher cache = new Cacher(logger);

            cache.SetCache(akim, 5000, "akim's tag");

            var obj = cache.GetCache("akim's tag");

            Assert.AreEqual(akim, obj);
        }

        [TestMethod]
        public void ChangeTime_TimeChanged()
        {
            MyClass sergey = new MyClass("Sergey", 21);

            Cacher cache = new Cacher(logger);

            cache.SetCache(sergey, 5000, "sergey's tag");
            cache.ChangeTime("sergey's tag", 99999999);

            Thread.Sleep(5500);

            var obj = cache.GetCache("sergey's tag");

            Assert.AreEqual(sergey, obj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveCache_SuccessfulDeleteon()
        {
            MyClass ivan = new MyClass("Ivan", 54);

            Cacher cache = new Cacher(logger);

            cache.SetCache(ivan, 99999999, "Ivan's tag");
            cache.RemoveCache("Ivan's tag");

            var objThrowException = cache.GetCache("Ivan's tag");
        }

        [TestMethod]
        public void SetCache_MockTest()
        {
            var mock = new Mock<ILoggingServices>();

            MyClass mockItem = new MyClass("MockName", 99);
            Cacher cacher = new Cacher(mock.Object);
            cacher.SetCache(mockItem, 10000, "mock's tag");

            mock.Verify(log => log.Log("Cache with tag: mock's tag was created"), Times.AtLeastOnce());
        }
    }

    public class MyClass
    {
        public string Name { get; private set; }
        public int Age { get; private set; }

        public MyClass(string name, int age)
        {
            Name = name ?? throw new ArgumentNullException($"Write {nameof(name)}");
            Age = age > 0 ? age : throw new ArgumentNullException($"Write {nameof(age)} more then 0");
        }
    }
}
