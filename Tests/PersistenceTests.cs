using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;
using SimplestUnityDI;
using static Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class PersistenceTests
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        [Test]
        public void Dispose()
        {
            DiContainer container = Container;
            
            DisposingEvent disp = new DisposingEvent();
            container.CurrentDisposingEvent = disp;
            
            container.Register<int>().FromInstance(-99).AsTransient();
            container.Register<ISerializable, StringBuilder>().FromInstance(_stringBuilder).AsTransient();
            container.Register<EmptyClass>().AsTransient();
            
            disp.OnDisposing();
            Assert.AreEqual(0, container.Count);
        }
    }
}