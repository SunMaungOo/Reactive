using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive
{
    /// <summary>
    /// Helper class to emulate anoymous function
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OnNextAction<T> : OnNext<T>
    {
        private Action<T> action;

        public OnNextAction(Action<T> action)
        {
            this.action = action;
        }
        public void OnNext(T input)
        {
            action.Invoke(input);
        }
    }
}
