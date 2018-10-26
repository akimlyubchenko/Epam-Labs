using System;
using System.Threading;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cacher = CacherServices.CacherServices;

namespace CachingSystem.Test
{
    [TestClass]
    public class CacherServicesTests
    {
        private readonly CacherServices.ILoggingServices logger = new CacherServices.LoggingServices();

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

            Assert.AreEqual(akim,obj);
        }

        [TestMethod]
        public void ChangeTime_TimeChanged()
        {
            MyClass akim = new MyClass("Sergey", 21);

            Cacher cache = new Cacher(logger);

            cache.SetCache(akim, 5000, "sergey's tag");
            cache.ChangeTime("sergey's tag", 99999999);

            Thread.Sleep(5500);

            var obj = cache.GetCache("sergey's tag");

            Assert.AreEqual(akim, obj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveCache_SuccessfulDeleteon()
        {
            MyClass oleg = new MyClass("Ivan", 54);

            Cacher cache = new Cacher(logger);

            cache.SetCache(oleg, 99999999, "Ivan's tag");
            cache.RemoveCache("Ivan's tag");

            var objThrowException = cache.GetCache("Ivan's tag");
        }

        [TestMethod]
        public void FileWatcher_ShouldSendMessage()
        {
            var mock = new Mock<ISendService>();

            var sender = new FileService(mock.Object, @"C:\Users\Akim_\source\repos\MailSender\MailSender\bin\Debug\Messages");

            sender.FileWatcher();

            using (StreamWriter reader = new StreamWriter(@"C:\Users\Akim_\source\repos\MailSender\MailSender\bin\Debug\Messages\1.txt"))
            {
                reader.WriteLine("ggggggggggggggg");
            }


            mock.Verify(watcher => watcher.Send(@"C:\Users\Akim_\source\repos\MailSender\MailSender\bin\Debug\Messages\1.txt"), Times.AtMostOnce());
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
