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
            Assert.AreEqual(node, config[0]);


            node = config[1];
            Assert.AreEqual(true, node.IsTemporary);
            Assert.AreEqual(null, node.Value);
            node.Value = 200;
            Assert.AreEqual(false, node.IsTemporary);
            Assert.AreEqual(node, config[1]);
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

        [TestMethod]
        public void 单值变数组测试()
        {
            var root = new Config(true);
            root["a"]["b"]["c"].Add(1);
            Assert.AreEqual(1, root["a"]["b"]["c"][0].Value);
            root["a"]["b"]["c"].Add(2);
            Assert.AreEqual(2, root["a"]["b"]["c"][1].Value);

        }

        [TestMethod]
        public void 路径测试()
        {
            var root = new Config(true);
            root["a"][0]["c"][0].Value = 1;
            Assert.AreEqual(false, root.Select("a[0].c[0]").IsTemporary);
            Assert.AreEqual(1, root.Select("a[0].c[0]").Value);
            Assert.AreEqual(root["a"][0]["c"][0], root.Select("a[0].c[0]"));
        }

        [TestMethod]
        public void 测试无效的key()
        {
            var root = new Config();
            try
            {
                root[null].Value = 1;
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            when (ex.Message == "当前节点无效")
            {

            }

            try
            {
                root[""].Value = 1;
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            when (ex.Message == "当前节点无效")
            {

            }


            root[" "].Value = 1;
        }

        [TestMethod]
        public void 测试null值错误()
        {
            var root = new Config();
            root["a"].Value = 1;
            root["a"][0].Value = 2;
            root["a"][0]["b"].Value = 3;
            try
            {
                root["a"] = null;
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            when (ex.ParamName == "value")
            {

            }

            try
            {
                root["a"][0] = null;
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            when (ex.ParamName == "value")
            {

            }

            try
            {
                root["a"][0]["b"] = null;
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            when (ex.ParamName == "value")
            {

            }

            Assert.AreEqual(1, root["a"].Value);
            Assert.AreEqual(2, root["a"][0].Value);
            Assert.AreEqual(3, root["a"][0]["b"].Value);
        }


        [TestMethod]
        public void 测试被顶掉的节点()
        {
            var root = new Config();
            var x = root["x"];
            x.Value = null;
            Assert.AreEqual(false, x.IsTemporary);

            foreach (var node in new[] { x, root })
            {
                var a = node["a"];
                var b = node["b"];
                a.Value = 1;
                b.Value = 2;
                Assert.AreEqual(a, node["a"]);
                Assert.AreEqual(1, node["a"].Value);
                Assert.AreEqual(b, node["b"]);
                Assert.AreEqual(2, node["b"].Value);
                Assert.AreEqual(false, a.IsTemporary);
                Assert.AreEqual(node, a.Parent);
                node["a"] = node["b"];
                Assert.AreEqual(true, a.IsTemporary);
                Assert.AreEqual(null, a.Parent);
                Assert.AreEqual(b, node["a"]);
                Assert.AreEqual(2, node["a"].Value);
            }

        }

        [TestMethod]
        public void 测试被索引顶掉的节点()
        {
            var root = new Config();
            var x = root["x"];
            x.Value = null;
            Assert.AreEqual(false, x.IsTemporary);

            foreach (var node in new[] { x, root })
            {
                var a = node[0];
                var b = node[1];
                a.Value = 1;
                b.Value = 2;
                Assert.AreEqual(a, node[0]);
                Assert.AreEqual(1, node[0].Value);
                Assert.AreEqual(b, node[1]);
                Assert.AreEqual(2, node[1].Value);
                Assert.AreEqual(false, a.IsTemporary);
                Assert.AreEqual(node, a.Parent);
                node[0] = node[1];
                Assert.AreEqual(true, a.IsTemporary);
                Assert.AreEqual(null, a.Parent);
                Assert.AreEqual(b, node[0]);
                Assert.AreEqual(2, node[0].Value);
                Assert.AreEqual(true, node[1].IsTemporary);
                Assert.AreEqual(null, node[1].Value);
            }

        }
    }
}