using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive
{
    public class Observable<T> : IObservable<T>
    {
        private OnNext<T> handler;

        private TaskFactory taskFactory;

        private Predicate<T> filter;


        public Observable(bool isUIThread = true) : this(null, isUIThread)
        {

        }

        /// <summary>
        /// Internal usage only
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="isUIThread"></param>
        public Observable(OnNext<T> handler, bool isUIThread = true)
        {
            this.handler = handler;

            Init(isUIThread);
        }

        public static IObservable<T> Create()
        {
            return new Observable<T>();
        }

        private void Init(bool isUIThread)
        {
            if (isUIThread)
            {
                taskFactory = new TaskFactory(new UIScheduler());
            }
            else
            {
                taskFactory = new TaskFactory();
            }

            //default filter which put everything

            filter = new Predicate<T>((T data) => { return true; });
        }

        public IObservable<T> Subscribe(OnNext<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("We cannot have null handler");
            }

            //For merge operation
            //if it is a delgate handler , we just delegate it
            if (this.handler != null && this.handler is DelegateHandler<T>)
            {
                DelegateHandler<T> innerHandler = this.handler as DelegateHandler<T>;

                innerHandler.SetHandler(handler);
            }
            else
            {
                this.handler = handler;
            }

            return this;
        }

        public void Unsubscribe()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter">Non-null</param>
        public IObservable<T> Filter(Predicate<T> filter)
        {
            if (filter == null)
            {
                throw new Exception("We cannot have null filter");
            }

            this.filter = filter;

            return this;
        }

        public void Emit(T data)
        {

            if (handler == null)
            {
                return;
            }

            if (filter.Invoke(data))
            {
                handler.OnNext(data);
            }

        }

        public IObservable<Result> Map<Result>(Func<T, Result> func)
        {
            if (func == null)
            {
                throw new Exception("func must not be null");
            }

            IObservable<Result> inner = new Observable<Result>(false);

            Mapper<T, Result> mapper = new Mapper<T, Result>(func, this, inner);

            return mapper.GetObservable();
        }

        public IObservable<T> Merge(IObservable<T> observable)
        {
            return Merge(this, observable);
        }

        public static IObservable<T> Merge(IObservable<T> observable1, IObservable<T> observable2)
        {

            OnNext<T> handler = new DelegateHandler<T>();

            observable1.Subscribe(handler);
            observable2.Subscribe(handler);

            return new Observable<T>(handler);
        }

        public IObservable<T> Scan<Result>(T initialData, Func<T, Result, T> func, IObservable<Result> observerable)
        {
            if (func == null)
            {
                throw new Exception("func must not be null");
            }

            Scanner<T, Result> scanner = new Scanner<T, Result>(initialData, func, this, observerable);

            return scanner;
        }

        public IList<IObservable<T>> Share(int branchCount)
        {
            if(branchCount<=0)
            {
                throw new Exception("brach count must be greater than 1");
            }

            List<IObservable<T>> branches = new List<IObservable<T>>();

            for(int i=0;i<branchCount;i++)
            {
                branches.Add(Create());
            }

            Subscribe(new OnNextAction<T>((T data) =>
            {
                branches.ForEach((IObservable<T> observer) =>
                {
                    observer.Emit(data);
                });
            }
            ));

            return branches;
        }
    }
}
