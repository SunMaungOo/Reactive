using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive
{
    public interface IObservable<T>
    {
        IObservable<T> Subscribe(OnNext<T> handler);

        void Unsubscribe();

        IObservable<T> Filter(Predicate<T> filter);

        IObservable<Result> Map<Result>(Func<T, Result> func);

        void Emit(T data);

        IObservable<T> Merge(IObservable<T> observable);

        IObservable<T> Scan<Result>(T initialData, Func<T, Result, T> func, IObservable<Result> observerable);
    }
}
