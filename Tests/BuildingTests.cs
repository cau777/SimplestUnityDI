using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;
using SimplestUnityDI;
using SimplestUnityDI.Exceptions;
using static Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class BuildingTests
    {
        [Test]
        public void Valid()
        {
            Assert.DoesNotThrow(() => Container.Register<int, int>().FromInstance(0).AsTransient());
        }
        
        [Test]
        public void Valid2()
        {
            Assert.DoesNotThrow(() => Container.Register<ISerializable, StringBuilder>().FromInstance(new StringBuilder()).AsTransient());
        }

        [Test]
        public void InvalidAbstract()
        {
            Assert.Throws<ContainerException>(() => Container.Register<MethodBase, MethodInfo>().AsTransient());
        }
        
        [Test]
        public void InvalidInterfaces()
        {
            Assert.Throws<ContainerException>(() => Container.Register<IEnumerable, ICollection>().AsTransient());
        }

        [Test]
        public void ValidWithIds1()
        {
            Assert.DoesNotThrow(() =>
            {
                DiContainer container = Container;
                container.Register<EmptyClass>().WithId("1").AsTransient();
                container.Register<EmptyClass>().WithId("2").AsTransient();
                container.Register<EmptyClass>().WithId("3").AsTransient();
            });
        }
        
        [Test]
        public void ValidWithIds2()
        {
            Assert.DoesNotThrow(() =>
            {
                DiContainer container = Container;
                container.Register<EmptyClass>().AsTransient();
                container.Register<EmptyClass>().WithId("2").AsTransient();
            });
        }

        [Test]
        public void InvalidWithIds1()
        {
            Assert.Throws<ContainerException>(() =>
            {
                DiContainer container = Container;
                container.Register<EmptyClass>().AsTransient();
                container.Register<EmptyClass>().AsTransient();
            });
        }
        
        [Test]
        public void InvalidWithIds2()
        {
            Assert.Throws<ContainerException>(() =>
            {
                DiContainer container = Container;
                container.Register<EmptyClass>().WithId("1").AsTransient();
                container.Register<EmptyClass>().WithId("1").AsTransient();
            });
        }
    }
}