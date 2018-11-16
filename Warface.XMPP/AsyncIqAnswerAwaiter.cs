using System;
using System.Threading;
using System.Threading.Tasks;

namespace Warface.XMPP
{
    public abstract class AsyncIqAnswerAwaiter
    {
        readonly TimeSpan              _timeout;
        string                         _id;
        TaskCompletionSource<object[]> _tcs;

        protected Task<object[]> BaseTask => _tcs.Task;

        protected AsyncIqAnswerAwaiter(TimeSpan timeout = default)
        {
            if (timeout == default)
                timeout = TimeSpan.FromSeconds(10);
            _timeout = timeout;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IQ ID to be used to send a GET query</returns>
        public string StartAwaiting()
        {
            //TODO ensure previous task is finished or wait for it
            _id = XmppMethods.GetRandomIqUid();
            var ct  = new CancellationTokenSource(_timeout);
            var tcs = new TaskCompletionSource<object[]>();
            ct.Token.Register(() => tcs.TrySetCanceled());
            _tcs = tcs;
            return _id;
        }

        protected void ProcessIq(string id, params object[] pars)
        {
            if (id != _id)
                return;
            _tcs.TrySetResult(pars);
        }
    }

    public class AsyncIqAnswerAwaiter<T> : AsyncIqAnswerAwaiter
    {
        public AsyncIqAnswerAwaiter(TimeSpan timeout = default) : base(timeout)
        {
        }

        public async Task<T> GetTask()
        {
            var taskResult = await BaseTask;
            var param      = (T) taskResult[0];
            return param;
        }

        public void ProcessIq(string id, T param)
        {
            base.ProcessIq(id, param);
        }
    }

    public class AsyncIqAnswerAwaiter<T1, T2> : AsyncIqAnswerAwaiter
    {
        public AsyncIqAnswerAwaiter(TimeSpan timeout = default) : base(timeout)
        {
        }

        public async Task<(T1, T2)> GetTask()
        {
            var taskResult = await BaseTask;
            var param1     = (T1) taskResult[0];
            var param2     = (T2) taskResult[1];
            return (param1, param2);
        }

        public void ProcessIq(string id, T1 param1, T2 param2)
        {
            base.ProcessIq(id, param1, param2);
        }
    }

    public class AsyncIqAnswerAwaiter<T1, T2, T3> : AsyncIqAnswerAwaiter
    {
        public AsyncIqAnswerAwaiter(TimeSpan timeout = default) : base(timeout)
        {
        }

        public async Task<(T1, T2, T3)> GetTask()
        {
            var taskResult = await BaseTask;
            var param1     = (T1) taskResult[0];
            var param2     = (T2) taskResult[1];
            var param3     = (T3) taskResult[2];
            return (param1, param2, param3);
        }

        public void ProcessIq(string id, T1 param1, T2 param2, T3 param3)
        {
            base.ProcessIq(id, param1, param2, param3);
        }
    }

    public class AsyncIqAnswerAwaiter<T1, T2, T3, T4> : AsyncIqAnswerAwaiter
    {
        public AsyncIqAnswerAwaiter(TimeSpan timeout = default) : base(timeout)
        {
        }

        public async Task<(T1, T2, T3, T4)> GetTask()
        {
            var taskResult = await BaseTask;
            var param1     = (T1) taskResult[0];
            var param2     = (T2) taskResult[1];
            var param3     = (T3) taskResult[2];
            var param4     = (T4) taskResult[3];
            return (param1, param2, param3, param4);
        }

        public void ProcessIq(string id, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            base.ProcessIq(id, param1, param2, param3, param4);
        }
    }
}