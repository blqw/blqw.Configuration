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
    }
}
