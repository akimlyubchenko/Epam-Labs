using System;
using System.Threading;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CachingSystem.Test
{
    [TestClass]
    public class CacherTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Set_Get_ThrowException()
        {
            MyClass oleg = new MyClass("Oleg", 54);

            Cacher cache = new Cacher();

            cache.SetCache(oleg, 1000, "oleg's tag");

            var obj2 = cache.GetCache("oleg's tag");

            Thread.Sleep(1005);

            var objThrowException = cache.GetCache("oleg's tag");
        }

        [TestMethod]
        public void Set_Get_CheckingForCoincidence()
        {
            MyClass akim = new MyClass("Akim", 19);

            Cacher cache = new Cacher();

            cache.SetCache(akim, 5000, "akim's tag");

            var obj = cache.GetCache("akim's tag");

            Assert.AreEqual(akim,obj);
        }
    }

    public class MyClass
    {
        public string Name { get; private set; }
        public int Age { get; private set; }

        public MyClass(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
