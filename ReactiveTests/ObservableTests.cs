using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive.Tests
{
    [TestClass()]
    public class ObservableTests
    {
        /// <summary>
        /// Test whether observerable emit data
        /// </summary>
        [TestMethod()]
        public void ObservableTest()
        {
            Observable<int> observer = new Observable<int>();

            Action<int> action = new Action<int>((int input) =>
            {
                Assert.IsTrue(input == 10);
            });

            observer.Subscribe(new OnNextAction<int>(action));
            observer.Emit(10);

        }

        /// <summary>
        /// Test whether observable actually filter data
        /// </summary>
        [TestMethod()]
        public void FilterTest()
        {
            Observable<int> observer = new Observable<int>();

            Action<int> action = new Action<int>((int input) =>
            {
                Assert.IsTrue(input >= 90);
            });

            Predicate<int> filter = new Predicate<int>((int input) =>
            {
                return input > 90;
            });

            observer.Subscribe(new OnNextAction<int>(action));
            observer.Filter(filter);
            observer.Emit(10);
            observer.Emit(20);
            observer.Emit(100);


        }

        [TestMethod()]
        public void MapTest()
        {
            Observable<int> observer = new Observable<int>();

            Action<char> action = new Action<char>((char input) =>
            {
                Assert.IsTrue(true);
            });

            Reactive.IObservable<char> tmp = observer.Map(Convert);
            tmp.Subscribe(new OnNextAction<char>(action));

            observer.Emit(10);

        }

        public char Convert(int data)
        {
            return 'c';
        }

        [TestMethod()]
        public void MergeTest()
        {
            Observable<int> observer1 = new Observable<int>();

            Observable<int> observer2 = new Observable<int>();

            Action<int> action = new Action<int>((int input) =>
            {
                Assert.IsTrue(true);
            });

            Observable<int>.Merge(observer1, observer2).Subscribe(new OnNextAction<int>(action));

            observer1.Emit(10);
            observer2.Emit(20);
        }

        [TestMethod()]
        public void ScanTest()
        {
            Observable<string> observer1 = new Observable<string>();

            Observable<int> observer2 = new Observable<int>();

            Action<string> action = new Action<string>((string input) =>
            {
                bool condition = (input == "A1" || input == "A12");

                Assert.IsTrue(condition);
            });

            observer1.Scan("A", (string previous, int current) =>
            {
                return previous.ToString() + current.ToString();
            }, observer2)
            .Subscribe(new OnNextAction<string>(action));

            observer2.Emit(1);
            observer2.Emit(2);

        }
    }
}