using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAmadeus
{
    public sealed class AmadeusCloseStrategy : ICloseStrategy<ITab>
    {
        private readonly ICloseStrategy<ITab> _defaultStrategy = new DefaultCloseStrategy<ITab>();

        public async void Execute(IEnumerable<ITab> toClose, Action<Boolean, IEnumerable<ITab>> callback)
        {
            foreach (IAsyncShutdown tab in toClose.Where(x => x is IAsyncShutdown))
            {
                await tab.ShutdownAsync();
            }

            _defaultStrategy.Execute(toClose, callback);
        }
    }
}
