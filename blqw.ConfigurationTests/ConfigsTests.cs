using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Configuration;

namespace blqw.ConfigurationTests
{
    [TestClass]
    public class ConfigsTests
    {
        [TestMethod]
        public void 重复项测试()
        {
            /*
             * <add key="test.a" value="1"/>
             * <add key="test.b" value="2"/>
             * <add key="test.c" value="3"/>
             * <add key="test.d" value="4"/>
             *
             * <add key="test" value="a=a;b=b;c=c;d=d"/>
             */

            Assert.AreEqual("a=a;b=b;c=c;d=d", Configs.AppSettings["test"].Value);

            Assert.AreEqual("a", Configs.AppSettings["test"]["a"].Value);
            Assert.AreEqual("1", Configs.AppSettings["test"]["a"][0].Value);
            Assert.AreEqual("a", Configs.AppSettings["test"]["a"][1].Value);

            Assert.AreEqual("b", Configs.AppSettings["test"]["b"].Value);
            Assert.AreEqual("2", Configs.AppSettings["test"]["b"][0].Value);
            Assert.AreEqual("b", Configs.AppSettings["test"]["b"][1].Value);

            Assert.AreEqual("c", Configs.AppSettings["test"]["c"].Value);
            Assert.AreEqual("3", Configs.AppSettings["test"]["c"][0].Value);
            Assert.AreEqual("c", Configs.AppSettings["test"]["c"][1].Value);

            Assert.AreEqual("d", Configs.AppSettings["test"]["d"].Value);
            Assert.AreEqual("4", Configs.AppSettings["test"]["d"][0].Value);
            Assert.AreEqual("d", Configs.AppSettings["test"]["d"][1].Value);
        }

        [TestMethod]
        public void 测试应用到实例属性()
        {

            /*
             *  <add key="MyClass.Name" value="blqw"/>
             *  <add key="Name" value="xxx"/>
             *  <add key="MyClass.ID" value="1"/>
             *  <add key="blqw.ConfigurationTests.ConfigsTests.MyClass.ID" value="398"/>
             *  <add key="MyClass" value="Age=30;Birthday=1986-10-29"/>  
             */

            var m = new MyClass();
            Configs.ApplyAppSettings(m);
            Assert.AreEqual("blqw", m.Name);
            Assert.AreEqual(398, m.ID);
            Assert.AreEqual(30m, m.Age);
            Assert.AreEqual(new DateTime(1986, 10, 29), m.Birthday);
        }

        class MyClass
        {
            public String Name { get; set; }
            public int ID { get; set; }
            public decimal Age { get; set; }

            public DateTime Birthday { get; set; }

        }

        [TestMethod]
        public void 测试应用到静态属性()
        {
            /*
             *  <add key="MyConfig.Version" value="1.0"/>
             *  <add key="MyConfig.DEBUG" value="true"/>
             *  <add key="MyConfig.TaskThreadCount" value="50"/>
             *  <add key="TaskConfig.Thread.Count" value="60"/>
             *  <add key="MyConfig" value="DEBUG=false;ver=1.2.3"/>
             */

            Configs.ApplyAppSettings(typeof(MyConfig));
            Assert.AreEqual("1.2.3", MyConfig.Version);
            Assert.AreEqual(false, MyConfig.DEBUG);
            Assert.AreEqual(60, MyConfig.TaskThreadCount);
        }

        [AppSettsing("TaskConfig")]
        static class MyConfig
        {
            [AppSettsing("ver")]
            public static string Version { get; private set; }

            public static bool DEBUG { get; private set; } = true;
            [AppSettsing("Thread.Count")]
            public static int TaskThreadCount { get; private set; }
        }
    }
}