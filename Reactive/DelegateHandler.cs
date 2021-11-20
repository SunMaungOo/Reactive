using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive
{
    class DelegateHandler<T> : OnNext<T>
    {
        private OnNext<T> handler;

        public DelegateHandler() : this(null)
        {

        }

        public DelegateHandler(OnNext<T> handler)
        {
            SetHandler(handler);
        }

        public void SetHandler(OnNext<T> handler)
        {
            this.handler = handler;
        }

        public void OnNext(T input)
        {
            if (handler == null)
            {
                return;
            }

            handler.OnNext(input);
        }
    }
}
