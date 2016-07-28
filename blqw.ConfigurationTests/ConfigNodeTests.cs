using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Configuration.Tests
{
    [TestClass()]
    public class ConfigNodeTests
    {
        [TestMethod]
        public void 索引临时节点测试()
        {
            var config = new Config();
            var node = config[0];
            Assert.AreEqual(true, node.IsTemporary);
            Assert.AreEqual(null, node.Value);
            node.Value = 100;
            Assert.AreEqual(false, node.IsTemporary);
            node = config[0];
            Assert.AreEqual(false, node.IsTemporary);
            Assert.AreEqual(100, node.Value);


            node = config[1];
            Assert.AreEqual(true, node.IsTemporary);
            Assert.AreEqual(null, node.Value);
            node.Value = 200;
            Assert.AreEqual(false, node.IsTemporary);
            node = config[1];
            Assert.AreEqual(false, node.IsTemporary);
            Assert.AreEqual(200, node.Value);
        }

        [TestMethod]
        public void 键值临时节点测试()
        {
            var config = new Config();
            var node = config["a"];
            Assert.AreEqual(true, node.IsTemporary);
            Assert.AreEqual(null, node.Value);
            node.Value = 100;
            Assert.AreEqual(false, node.IsTemporary);
            node = config["a"];
            Assert.AreEqual(false, node.IsTemporary);
            Assert.AreEqual(100, node.Value);


            node = config["b"];
            Assert.AreEqual(true, node.IsTemporary);
            Assert.AreEqual(null, node.Value);
            node.Value = 200;
            Assert.AreEqual(false, node.IsTemporary);
            node = config["b"];
            Assert.AreEqual(false, node.IsTemporary);
            Assert.AreEqual(200, node.Value);
        }

        [TestMethod]
        public void 单类型节点替换测试()
        {
            var root = new Config(true);
            root["a"]["b"]["c"].Value = 1;
            Assert.AreEqual(1, root["a"]["b"]["c"].Value);
            root["a"][0].Value = 2;
            Assert.AreEqual(null, root["a"]["b"]["c"].Value);
            Assert.AreEqual(2, root["a"][0].Value);


        }
    }
}