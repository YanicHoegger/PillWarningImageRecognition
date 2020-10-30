using System;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Telerik.JustMock;
using Utilities.Aspects;

namespace Utilities.UnitTests
{
    public class HostedServiceDecoratorTest
    {
        [Test]
        public void ExceptionWhenAccessPropertyBeforeStart()
        {
            GivenDecoratedService();
            WhenAccessPropertyThenException();
        }

        [Test]
        public void ExceptionWhenAccessMethodBeforeStart()
        {
            GivenDecoratedService();
            WhenAccessMethodThenException();
        }

        [Test]
        public void AccessPropertyAfterStart()
        {
            GivenDecoratedService();
            WhenStart();
            ThenPropertyAccessible();
        }

        [Test]
        public void AccessMethodAfterStart()
        {
            GivenDecoratedService();
            WhenStart();
            ThenMethodAccessible();
        }

        [Test]
        public void ExceptionAccessPropertyAfterStop()
        {
            GivenDecoratedService();
            WhenStart();
            WhenStop();
            WhenAccessPropertyThenException();
        }

        [Test]
        public void ExceptionAccessMethodAfterStop()
        {
            GivenDecoratedService();
            WhenStart();
            WhenStop();
            WhenAccessMethodThenException();
        }

        private IServiceMock _service;
        private IHostedService _hostedService;

        private void GivenDecoratedService()
        {
            (_service, _hostedService) = HostedServiceDecorator<IServiceMock>.Create(new HostedServiceMock(),
                Mock.Create<ILogger<HostedServiceMock>>());
        }

        private void WhenAccessPropertyThenException()
        {
            // ReSharper disable once NotAccessedVariable : Needed to make assignment call
            object temp;
            Assert.Throws<InvalidOperationException>(() => temp = _service.Property);
        }

        private void WhenAccessMethodThenException()
        {
            Assert.Throws<InvalidOperationException>(() => _service.Method());
        }

        private void WhenStart()
        {
            _hostedService.StartAsync(CancellationToken.None).Wait();
        }

        private void WhenStop()
        {
            _hostedService.StopAsync(CancellationToken.None);
        }

        private void ThenPropertyAccessible()
        {
            // ReSharper disable once UnusedVariable : Needed to make assignment call
#pragma warning disable IDE0059 // Unnecessary assignment of a value : Needed to make assignment call
            var temp = _service.Property;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        private void ThenMethodAccessible()
        {
            _service.Method();
        }
    }
}
