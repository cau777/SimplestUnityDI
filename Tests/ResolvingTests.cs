using System.Text;
using NUnit.Framework;
using SimplestUnityDI;
using SimplestUnityDI.Exceptions;
using static Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class ResolvingTests
    {
        [Test]
        public void ResolveSimple()
        {
            Assert.DoesNotThrow(() =>
            {
                DiContainer container = Container;
                container.Register<EmptyClass>().AsTransient();
                container.Resolve<EmptyClass>();
            });
        }

        [Test]
        public void ResolveInterface()
        {
            Assert.DoesNotThrow(() =>
            {
                DiContainer container = Container;
                container.Register<IEmptyClass, EmptyClass>().AsTransient();
                container.Resolve<IEmptyClass>();
            });
        }

        [Test]
        public void ResolveNotRegistered()
        {
            Assert.Throws<ContainerException>(() =>
            {
                DiContainer container = Container;
                container.Resolve<StringBuilder>();
            });
        }

        [Test]
        public void ResolveId()
        {
            DiContainer container = Container;
            StringBuilder b1 = new StringBuilder();
            StringBuilder b2 = new StringBuilder();

            container.Register<StringBuilder>().FromInstance(b1).WithId("Test").AsTransient();
            container.Register<StringBuilder>().FromInstance(b2).AsTransient();

            Assert.AreSame(b1, container.Resolve<StringBuilder>("Test"));
            Assert.AreSame(b2, container.Resolve<StringBuilder>());
        }

        [Test]
        public void ResolveNoIdMatch()
        {
            DiContainer container = Container;
            StringBuilder b1 = new StringBuilder();
            StringBuilder b2 = new StringBuilder();

            container.Register<StringBuilder>().FromInstance(b1).WithId("Test1").AsTransient();
            container.Register<StringBuilder>().FromInstance(b2).WithId("Test2").AsTransient();

            Assert.AreSame(b1, container.Resolve<StringBuilder>());
        }

        [Test]
        public void ResolveDependencyLine()
        {
            DiContainer container = Container;
            container.Register<ClassDependencyLine1>().AsTransient();
            container.Register<ClassDependencyLine2>().AsTransient();
            container.Register<ClassDependencyLine3>().AsTransient();

            Assert.DoesNotThrow(() =>
            {
                ClassDependencyLine3 result = container.Resolve<ClassDependencyLine3>();
                Assert.NotNull(result);
                Assert.NotNull(result.Line);
                Assert.NotNull(result.Line.Line);
            });
        }
    }
}