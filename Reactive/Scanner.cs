using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive
{
    public class Scanner<T, Result> : IObservable<T>
    {
        private T current;

        private Func<T, Result, T> func;

        private IObservable<T> inner;

        private IObservable<Result> observerable;

        public Scanner(T initial, Func<T, Result, T> func, IObservable<T> inner, IObservable<Result> observerable)
        {
            this.current = initial;

            this.func = func;

            this.inner = inner;

            this.observerable = observerable;

            Action<Result> action = new Action<Result>((Result data) =>
            {

                current = func.Invoke(current, data);

                Emit(current);
            });

            this.observerable.Subscribe(new OnNextAction<Result>(action));

        }

        public void Emit(T data)
        {
            inner.Emit(data);

        }

        public IObservable<T> Filter(Predicate<T> filter)
        {
            inner.Filter(filter);

            return this;
        }

        public IObservable<Result> Map<Result>(Func<T, Result> func)
        {
            return inner.Map(func);
        }

        public IObservable<T> Merge(IObservable<T> observable)
        {
            return inner.Merge(observable);
        }

        public IObservable<T> Scan<Result>(T initialData, Func<T, Result, T> func, IObservable<Result> observerable)
        {
            return inner.Scan(initialData, func, observerable);
        }

        public IList<IObservable<T>> Share(int branchCount)
        {
            return inner.Share(branchCount);
        }

        public IObservable<T> Subscribe(OnNext<T> handler)
        {
            inner.Subscribe(handler);

            return inner;
        }

        public void Unsubscribe()
        {
            inner.Unsubscribe();
        }
    }
}
