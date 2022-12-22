using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive
{
    public class Mapper<Input, Result> : IObservable<Result>
    {
        private Func<Input, Result> func;

        private IObservable<Input> observerable;

        private IObservable<Result> inner;

        public Mapper(Func<Input, Result> func, IObservable<Input> observerable, IObservable<Result> inner)
        {
            this.func = func;

            this.observerable = observerable;

            
            Action<Input> action = new Action<Input>((Input data) =>
            {
                Emit(func.Invoke(data));
            });
            
            this.observerable.Subscribe(new OnNextAction<Input>(action));

            this.inner = inner;
        }

        public void Emit(Result data)
        {
            inner.Emit(data);
        }

        public IObservable<Result> Filter(Predicate<Result> filter)
        {
            inner.Filter(filter);

            return inner;
        }

        public IObservable<Result> Subscribe(OnNext<Result> handler)
        {
            inner.Subscribe(handler);

            return inner;
        }

        public void Unsubscribe()
        {
            inner.Unsubscribe();
        }

        public IObservable<Result> Merge(IObservable<Result> observable)
        {
            return inner.Merge(observable);
        }

        public IObservable<Result> Scan<Result1>(Result initialData, Func<Result, Result1, Result> func, IObservable<Result1> observerable)
        {
            return inner.Scan(initialData, func, observerable);
        }

        public IObservable<Result> GetObservable()
        {
            return inner;
        }

        public IObservable<Result1> Map<Result1>(Func<Result, Result1> func)
        {
            return inner.Map(func);
        }

        public IList<IObservable<Result>> Share(int branchCount)
        {
            return inner.Share(branchCount);
        }
    }
}
